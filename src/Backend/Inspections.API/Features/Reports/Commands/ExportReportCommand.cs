using Inspections.API.Features.Reports.Models;
using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class ExportReportCommand : IRequest<byte[]>
    {
        public ExportReportCommand(int id, bool printPhotos, ExportDTO exportData)
        {
            Id = id;
            PrintPhotos = printPhotos;
            ExportData = exportData;
        }

        public int Id { get; }
        public bool PrintPhotos { get; }
        public ExportDTO ExportData { get; }
    }
}
