using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.Configuration;
using Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql;

internal sealed class CollectorDataReader : ICollectorDataReader
{
    private readonly ILogger<CollectorDataReader> _logger;
    private readonly AzureSqlConfiguration _configuration;

    public CollectorDataReader(
        ILogger<CollectorDataReader> logger,
        AzureSqlConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<IEnumerable<CollectorDataRecord>> ReadAllAsync(string collectorId)
    {
        _logger.LogDebug("Reading all records for collector: {CollectorId}", collectorId);

        List<CollectorDataRecord> records = [];

        string query = @"
            SELECT CollectorId, SetId, CardId, Unmodified, Signed, Proof, Altered
            FROM CollectorData
            WHERE CollectorId = @CollectorId";

        if (Guid.TryParse(collectorId, out Guid collectorGuid) is false)
        {
            _logger.LogError("Invalid GUID format for CollectorId: {CollectorId}", collectorId);
            throw new ArgumentException($"CollectorId must be a valid GUID. Received: {collectorId}", nameof(collectorId));
        }

        SqlConnection connection = new(_configuration.ConnectionString);
        try
        {
            await connection.OpenAsync().ConfigureAwait(false);

            SqlCommand command = new(query, connection);
            try
            {
                command.Parameters.Add("@CollectorId", System.Data.SqlDbType.UniqueIdentifier).Value = collectorGuid;

                SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                try
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        CollectorDataRecord record = new()
                        {
                            CollectorId = reader.GetGuid(0).ToString(),
                            SetId = reader.GetGuid(1).ToString(),
                            CardId = reader.GetGuid(2).ToString(),
                            Unmodified = reader.GetInt32(3),
                            Signed = reader.GetInt32(4),
                            Proof = reader.GetInt32(5),
                            Altered = reader.GetInt32(6)
                        };

                        records.Add(record);
                    }

                    _logger.LogInformation("Read {Count} records for collector {CollectorId}", records.Count, collectorId);

                    return records;
                }
                finally
                {
                    await reader.DisposeAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                await command.DisposeAsync().ConfigureAwait(false);
            }
        }
        finally
        {
            await connection.DisposeAsync().ConfigureAwait(false);
        }
    }

    public async Task<int> GetTotalCountAsync(string collectorId)
    {
        _logger.LogDebug("Getting total count for collector: {CollectorId}", collectorId);

        string query = @"
            SELECT COUNT(*)
            FROM CollectorData
            WHERE CollectorId = @CollectorId";

        if (Guid.TryParse(collectorId, out Guid collectorGuid) is false)
        {
            _logger.LogError("Invalid GUID format for CollectorId: {CollectorId}", collectorId);
            throw new ArgumentException($"CollectorId must be a valid GUID. Received: {collectorId}", nameof(collectorId));
        }

        SqlConnection connection = new(_configuration.ConnectionString);
        try
        {
            await connection.OpenAsync().ConfigureAwait(false);

            SqlCommand command = new(query, connection);
            try
            {
                command.Parameters.Add("@CollectorId", System.Data.SqlDbType.UniqueIdentifier).Value = collectorGuid;

                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                int count = result is null ? 0 : (int)result;

                _logger.LogInformation("Total count for collector {CollectorId}: {Count}", collectorId, count);

                return count;
            }
            finally
            {
                await command.DisposeAsync().ConfigureAwait(false);
            }
        }
        finally
        {
            await connection.DisposeAsync().ConfigureAwait(false);
        }
    }
}
