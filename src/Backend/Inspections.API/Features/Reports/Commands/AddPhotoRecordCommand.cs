using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Inspections.API.Features.Reports.Commands
{
    public class AddPhotoRecordCommand : IRequest<bool>
    {
        public int ReportId { get; }
        public IFormFileCollection FormFiles { get; }
        public string Label { get; }

        public AddPhotoRecordCommand(int reportId, IFormFileCollection formFiles, string label)
        {
            ReportId = reportId;
            FormFiles = formFiles;
            Label = label;
        }
    }
}
