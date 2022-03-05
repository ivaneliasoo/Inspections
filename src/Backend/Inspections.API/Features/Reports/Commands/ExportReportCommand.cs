using Inspections.API.Features.Reports.Models;
using JetBrains.Annotations;
using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class ExportReportCommand : IRequest<byte[]>
    {
        public ExportReportCommand(int id, bool printPhotos, ExportDto exportData)
        {
            Id = id;
            PrintPhotos = printPhotos;
            ExportData = exportData;
        }

        public int Id { [UsedImplicitly] get; }
        public bool PrintPhotos { [UsedImplicitly] get; }
        public ExportDto ExportData { get; }
    }
}
