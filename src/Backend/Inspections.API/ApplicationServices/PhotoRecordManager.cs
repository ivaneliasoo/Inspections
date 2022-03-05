using Amazon.S3;
using Inspections.API.Models.Configuration;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Inspections.API.ApplicationServices
{
    public class PhotoRecordManager
    {
        private readonly StorageDriver _storage;
        private readonly IOptions<ClientSettings> _options;
        private readonly IAmazonS3 _amazonS3;
        private readonly ILogger _logger;
        private readonly string _basePath;

        public PhotoRecordManager(StorageDriver storage, IOptions<ClientSettings> options, IAmazonS3 amazonS3,
            ILogger<PhotoRecordManager> logger)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _amazonS3 = amazonS3 ?? throw new ArgumentNullException(nameof(amazonS3));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _basePath = _options.Value.ReportsImagesFolder.Replace("\\", "/");
        }

        public Task CreateAlbum(string name)
        {
            _storage.CreatFolderIfNotExists(GenerateAlbumPath(name));
            return Task.CompletedTask;
        }

        private string GenerateAlbumPath(string albumName)
        {
            return Path.Combine(_basePath, albumName);
        }

        public async Task<PhotoItemResult> AddPhoto(Stream file, string album, string name, string contentType)
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
            var photo = await _storage
                .SaveAsync(ms, GenerateAlbumPath(album), Path.GetFileName(name), true, contentType)
                .ConfigureAwait(false);
            var thumbnail =
                await AddPhotoThumbnail(tnCopy, GenerateAlbumPath(album), Path.GetFileName(name), contentType)
                    .ConfigureAwait(false);

            if (photo is null || thumbnail is null)
            {
                throw new InvalidDataException("Error while adding photo to storage");
            }

            return new PhotoItemResult
            {
                PhotoPath = photo.Path,
                ThumbnailPath = thumbnail.Path,
                PhotoStorageId = photo.CloudId,
                ThumbnailStorageId = thumbnail.CloudId,
                ThumbnailUrl = GenerateSafeUrl(thumbnail.Path),
                PhotoUrl = GenerateSafeUrl(thumbnail.Path)
            };
        }

        internal string GenerateSafeUrl(string photoPath)
        {
            return _amazonS3.GeneratePreSignedURL(_options.Value.S3BucketName, photoPath, DateTime.Now.AddHours(2),
                null);
        }

        internal async Task<string> GenerateAsBase64(string photoPath)
        {
            try
            {
                await using var fileStream =
                    await _amazonS3.GetObjectStreamAsync(_options.Value.S3BucketName, photoPath, null);
                await using var ms = new MemoryStream();
                await fileStream.CopyToAsync(ms);
                var content = ms.ToArray();
                return "data:image/png;base64," + Convert.ToBase64String(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating base64 for photo");
                return "";
            }
        }

        private async Task<StorageItem> AddPhotoThumbnail(Stream file, string album, string name, string contentType)
        {
            file.Position = 0;
            using Image img = await Image.LoadAsync(file).ConfigureAwait(false);
            var (width, height) = (img.Width / 5, img.Height / 5);
            img.Mutate(i => i.Resize(width, height));
            await using var ms = new MemoryStream();
            await img.SaveAsJpegAsync(ms);
            ms.Position = 0;
            return await _storage.SaveAsync(ms, album,
                    Path.GetFileNameWithoutExtension(name) + "small" + Path.GetExtension(name), true, contentType)
                .ConfigureAwait(false);
        }

        public async Task RemovePhoto(string name)
        {
            await _storage.DeleteFile(name);
        }

        public async Task RemoveAlbum(string name)
        {
            await _storage.DeleteFolder(name);
        }
    }

    /// <summary>
    /// returned type when photo is added
    /// Photo Contains the path or url path of the saved Photo depending of the used storage driver
    /// Thumbnail Contains the path or url path of the photo thumbnail depending generated of the used storage driver
    /// </summary>
    public class PhotoItemResult
    {
        public string? PhotoStorageId { get; init; }
        public string? ThumbnailStorageId { get; init; }
        public string PhotoPath { get; init; } = default!;
        public string ThumbnailPath { get; init; } = default!;
        public string? PhotoUrl { get; init; }
        public string? ThumbnailUrl { get; init; }
    }
}
