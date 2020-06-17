using Inspections.Core.Domain.ReportsAggregate;

namespace Inspections.Core
{
    public interface IReportsBuilder
    {
        Report Build();
        ReportsBuilder AddChecklists(int[] checklistsIds);
        ReportsBuilder WithDefaultNotes(bool addDefaultNOtes);
        ReportsBuilder AddSignatures(int[] signaturesIds);
    }
}
