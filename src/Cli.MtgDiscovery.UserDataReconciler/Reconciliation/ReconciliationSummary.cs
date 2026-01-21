namespace Cli.MtgDiscovery.UserDataReconciler.Reconciliation;

internal sealed class ReconciliationSummary
{
    public int SetsProcessed { get; set; }
    public int SetsWithDiscrepancies { get; set; }
    public int SetsUpdated { get; set; }
    public int CardsAddedToGroups { get; set; }
    public int CardsRemovedFromGroups { get; set; }
    public int Errors { get; set; }

    public void Merge(ReconciliationSummary other)
    {
        SetsProcessed += other.SetsProcessed;
        SetsWithDiscrepancies += other.SetsWithDiscrepancies;
        SetsUpdated += other.SetsUpdated;
        CardsAddedToGroups += other.CardsAddedToGroups;
        CardsRemovedFromGroups += other.CardsRemovedFromGroups;
        Errors += other.Errors;
    }
}
