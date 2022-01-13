using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Infrastructure.Data;
using MediatR;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Inspections.API.Features.Reports.Handlers
{
    public class ExportReportCommandHandler : IRequestHandler<ExportReportCommand, byte[]>
    {
        private readonly InspectionsContext _context;

        public ExportReportCommandHandler(InspectionsContext context)
        {
            this._context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task<byte[]> Handle(ExportReportCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var config = _context.Set<ReportConfiguration>().FirstOrDefault(c => c.Id == request.ExportData.ReportConfigurationId);
            if (config is null) throw new InvalidOperationException("Report configurationinfo is required");
            return await GenerateReport(request.ExportData.PageUrl, config, request.ExportData.PhotosPerPage);
        }

        private async Task<byte[]> GenerateReport(string pageUrl, ReportConfiguration config, int photosPerPage)
        {
            Guard.Against.Null(pageUrl, nameof(pageUrl));
            Guard.Against.Null(config, nameof(config));

            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            // TODO-IVAN: --no-sandbox is an insecure workaround. I'll take a look into this next time
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, Args = new string[] { "--no-sandbox" } });
            await using var page = await browser.NewPageAsync();

            await page.GoToAsync($"{pageUrl}");
            await page.WaitForFunctionAsync("() => window.isPrintable === true");

            var pdfOptions = new PdfOptions
            {
                DisplayHeaderFooter = true,
                MarginOptions = new MarginOptions
                {
                    Bottom = config.MarginBottom,
                    Top = config.MarginTop,
                    Left = config.MarginLeft,
                    Right = config.MarginRight
                },
                HeaderTemplate = "",
                FooterTemplate = config.Footer,
            };

            return await page.PdfDataAsync(pdfOptions);
        }
    }
}
