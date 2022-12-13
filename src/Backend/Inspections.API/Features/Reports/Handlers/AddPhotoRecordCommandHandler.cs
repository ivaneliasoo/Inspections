using System.Data.Common;
using System.IO;
using System.Globalization;
using Ardalis.GuardClauses;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Reports.Commands;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces.Repositories;
using MediatR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Inspections.API.Features.Reports.Handlers;

public class AddPhotoRecordCommandHandler : IRequestHandler<AddPhotoRecordCommand, bool>
{
    private readonly PhotoRecordManager _photoManager;
    private readonly IReportsRepository _reportsRepository;

    public AddPhotoRecordCommandHandler(PhotoRecordManager photoManager
        , IReportsRepository reportsRepository)
    {
        _photoManager = photoManager ?? throw new ArgumentNullException(nameof(photoManager));
        _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
    }

    public async Task<bool> Handle(AddPhotoRecordCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var savedFilesPaths = new string[request!.FormFiles.Count];
        try
        {
            var report = await _reportsRepository.GetByIdAsync(request!.ReportId).ConfigureAwait(false);

            bool isLabelInHeader = request.Label?.Length > 0;

            int createdFilesCount = 0;
            foreach (var file in request!.FormFiles)
            {
                var nameAndLabel = !isLabelInHeader ? file.FileName.Split('|', StringSplitOptions.RemoveEmptyEntries) : null;
                var name = nameAndLabel is null ? file.FileName : nameAndLabel[0];
                var label = nameAndLabel is null ? request.Label!.ToUpperInvariant() : nameAndLabel.Length > 1 ? nameAndLabel[1] : "";

                // var filePath = await _photoManager.AddPhoto(file.OpenReadStream(), request!.ReportId.ToString(CultureInfo.InvariantCulture), name, file.ContentType).ConfigureAwait(false);
                var photoData = await PhotoAsByteArray(file.OpenReadStream(), request!.ReportId.ToString(CultureInfo.InvariantCulture), name, file.ContentType);

                Console.WriteLine("*** BaseUrl {0}", request.BaseUrl);
                var photo = new PhotoRecord(request.ReportId, request.BaseUrl, label, photoData.Photo, photoData.Thumbnail);
                //photo.PhotoUrl = $"{request.BaseUrl}/api/{photo.Id}/photo";
                //photo.ThumbnailUrl = $"{request.BaseUrl}/api/{photo.Id}/thumbnail";

                // var photo = new PhotoRecord(request.ReportId, filePath.PhotoPath, filePath.ThumbnailPath, label);
                // photo.SetMetadata(filePath.PhotoStorageId!, filePath.ThumbnailStorageId!, filePath.PhotoUrl!, filePath.ThumbnailUrl!);
                // photo.SetMetadata(filePath.PhotoStorageId!, filePath.ThumbnailStorageId!, filePath.PhotoUrl!, filePath.ThumbnailUrl!);

                report.AddPhoto(photo);

                // savedFilesPaths[createdFilesCount] = filePath.PhotoPath;
                createdFilesCount++;
            }

            await _reportsRepository.UpdateAsync(report).ConfigureAwait(false);

            return true;
        }
        catch (DbException e)
        {
            // Console.WriteLine("*** DbException {0}", e);
            foreach (var filePath in savedFilesPaths)
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                    await _photoManager.RemovePhoto(filePath).ConfigureAwait(false);
            }
            return false;
        }
    }

    public async Task<PhotoItemResult> PhotoAsByteArray(Stream file, string album, string name, string contentType)
    {
        await using var tnCopy = new MemoryStream();
        await file.CopyToAsync(tnCopy);

        file.Position = 0;
        using Image img = await Image.LoadAsync(file).ConfigureAwait(false);
        var (width, height) = (img.Width / 4, img.Height / 4);
        img.Mutate(i => i.Resize(width, height));
        await using var ms = new MemoryStream();
        await img.SaveAsJpegAsync(ms);
        ms.Position = 0;
        var photo = ms.ToArray();

        file.Position = 0;
        using Image img2 = await Image.LoadAsync(file).ConfigureAwait(false);
        (width, height) = (img.Width / 5, img.Height / 5);
        img.Mutate(i => i.Resize(width, height));
        await using var ms2 = new MemoryStream();
        await img.SaveAsJpegAsync(ms2);
        ms.Position = 0;
        var thumbnail = ms2.ToArray();

        // Console.WriteLine("*** AddPhoto. Before return");
        var result = new PhotoItemResult
        {
            PhotoPath = "",
            ThumbnailPath = "",
            PhotoStorageId = "",
            ThumbnailStorageId = "",
            ThumbnailUrl = "",
            PhotoUrl = "",
            Photo = photo,
            Thumbnail = thumbnail
        };

        return result;
    }

    // public class PhotoItem
    // {
    //     public byte[]? Photo { get; init; }
    //     public byte[]? ThumbNail { get; init; }
    // }
}
