﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Inspections.API.Models.Configuration;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Inspections.API.ApplicationServices
{
    public class PhotoRecordManager
    {
        private readonly StorageDriver _storage;
        private readonly IOptions<ClientSettings> _options;
        private readonly IAmazonS3 _amazonS3;
        private readonly string BasePath;


        public PhotoRecordManager(StorageDriver storage, IOptions<ClientSettings> options, IAmazonS3 amazonS3)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _amazonS3 = amazonS3 ?? throw new ArgumentNullException(nameof(amazonS3));
            BasePath = _options.Value.ReportsImagesFolder.Replace("\\", "/");
        }

        public Task CreateAlbum(string name)
        {
            _storage.CreatFolderIfNotExists(GenerateAlbumPath(name));
            return Task.CompletedTask;
        }

        internal string GenerateAlbumPath(string albumName)
        {
            return Path.Combine(BasePath, albumName);
        }

        public async Task<PhotoItemResult> AddPhoto(Stream file, string album, string name, string contentType)
        {
            using var tnCopy = new MemoryStream();
            await file.CopyToAsync(tnCopy);

            var photo = await _storage.SaveAsync(file, GenerateAlbumPath(album), Path.GetFileName(name), true, contentType).ConfigureAwait(false);
            var thumbnail = await AddPhotoThumbnail(tnCopy, GenerateAlbumPath(album), Path.GetFileName(name), contentType).ConfigureAwait(false);
            if (photo is not null && thumbnail is not null)
                return new PhotoItemResult
                {
                    PhotoPath = photo.Path ,
                    ThumbnailPath = thumbnail.Path,
                    PhotoStorageId = photo.CloudId,
                    ThumbnailStorageId = thumbnail.CloudId,
                    ThumbnailUrl = GenerateSafeUrl(thumbnail.Path),
                    PhotoUrl = GenerateSafeUrl(thumbnail.Path)
                };

            throw new InvalidDataException("Error while adding photo to storage");
        }

        internal string GenerateSafeUrl(string photoPath)
        {
            return _amazonS3.GeneratePreSignedURL(_options.Value.S3BucketName, photoPath, DateTime.Now.AddHours(2), null);
        }

        internal async Task<StorageItem> AddPhotoThumbnail(Stream file, string album, string name, string contentType)
        {
            file.Position = 0;
            using Image img = await Image.LoadAsync(file).ConfigureAwait(false);
            img.Mutate(i => i.Resize(img.Width / 3, img.Height / 3));
            using var ms = new MemoryStream();
            img.SaveAsJpeg(ms);
            ms.Position = 0;
            return await _storage.SaveAsync(ms, album, Path.GetFileNameWithoutExtension(name) + "small" + Path.GetExtension(name), true, contentType).ConfigureAwait(false);
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
    /// Photo Cointais the path or url path of the saved Photo depending of the used storage driver
    /// Thumbnail Cointais the path or url path of the photo thumbnail depending generated of the used storage driver
    /// </summary>
    public class PhotoItemResult
    {
        public string? PhotoStorageId { get; set; } = default!;
        public string? ThumbnailStorageId { get; set; } = default!;
        public string PhotoPath { get; set; } = default!;
        public string ThumbnailPath { get; set; } = default!;
        public string? PhotoUrl { get; set; } = default!;
        public string? ThumbnailUrl { get; set; } = default!;
    }
}