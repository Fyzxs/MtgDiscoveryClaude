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
        List<CollectorDataRecord> records = new();

        string query = @"
            SELECT CollectorId, SetId, CardId, Unmodified, Signed, Proof, Altered
            FROM CollectorCards
            WHERE CollectorId = @CollectorId";

        await using SqlConnection connection = new SqlConnection(_configuration.ConnectionString);
        await connection.OpenAsync().ConfigureAwait(false);

        await using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CollectorId", collectorId);

        await using SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            CollectorDataRecord record = new()
            {
                CollectorId = reader.GetString(0),
                SetId = reader.GetString(1),
                CardId = reader.GetString(2),
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

    public async Task<int> GetTotalCountAsync(string collectorId)
    {
        string query = @"
            SELECT COUNT(*)
            FROM CollectorCards
            WHERE CollectorId = @CollectorId";

        await using SqlConnection connection = new SqlConnection(_configuration.ConnectionString);
        await connection.OpenAsync().ConfigureAwait(false);

        await using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CollectorId", collectorId);

        object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
        int count = (int)result;

        _logger.LogInformation("Total count for collector {CollectorId}: {Count}", collectorId, count);

        return count;
    }
}
