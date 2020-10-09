using Inspections.API.Models.Configuration;
using Inspections.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.ApplicationServices
{
    public class FileUploadService
    {
        private readonly IOptions<ClientSettings> _storageOptions;
        private readonly IStorageHelper _storageHelper;

        public FileUploadService(IOptions<ClientSettings> storageOptions, IStorageHelper storageHelper)
        {
            _storageOptions = storageOptions ?? throw new ArgumentNullException(nameof(storageOptions));
            _storageHelper = storageHelper ?? throw new ArgumentNullException(nameof(storageHelper));
        }
        internal async Task UploadAttachments(IFormFileCollection files, string uniqueFolderName = "")
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), _storageOptions.Value.ReportsImagesFolder, uniqueFolderName);
            foreach (var file in files)
            {
                await _storageHelper.SaveAsync(file.OpenReadStream(), folder, file.FileName, true).ConfigureAwait(false);
            }
        }
        internal async Task<IEnumerable<string>> UploadAttachments(IFormFileCollection files, bool useUniqueString = true)
        {
            var uploadedFilesPaths = new List<string>();

            string folder = Path.Combine(Directory.GetCurrentDirectory(), _storageOptions.Value.ReportsImagesFolder, "");
            foreach (var file in files)
            {
                var path = await _storageHelper.SaveAsync(file.OpenReadStream(), folder, file.FileName, useUniqueString).ConfigureAwait(false);
                uploadedFilesPaths.Add(path);
            }
            return uploadedFilesPaths.AsEnumerable();
        }

        internal async Task<string> UploadAttachments(IFormFile file, string requestFolder = "", string fileName = "")
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), _storageOptions.Value.ReportsImagesFolder, requestFolder);
            var result = await _storageHelper.SaveAsync(file.OpenReadStream(), folder, fileName.Length>0 ? fileName : file.FileName, true).ConfigureAwait(false);
            await SaveResizedImage(file, folder, result);
            return result;
        }

        internal async Task<byte[][]> GetAttachments(string requestFolder)
        {
            var attachamentsRoutes = await Task.Run(() => _storageHelper.GetFilesInFolder(requestFolder)).ConfigureAwait(false);
            List<byte[]> result = new List<byte[]>();
            foreach (var attachement in attachamentsRoutes)
            {
                result.Add(File.ReadAllBytes(attachement));
            }
            return result.ToArray();
        }

        internal async Task<string[]> GetAttachmentsForPreview(string requestFolder)
        {
            return await Task.Run(() => _storageHelper.GetFilesInFolder(requestFolder)).ConfigureAwait(false);
        }

        internal async Task DeleteAttachments(string requestFolder)
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), _storageOptions.Value.ReportsImagesFolder, requestFolder);
            await Task.Run(() => _storageHelper.DeleteFolder(folder, true)).ConfigureAwait(false);
        }
        internal async Task DeleteAsync(string fileName)
        {
            await Task.Run(() => _storageHelper.DeleteFile(fileName)).ConfigureAwait(false);
        }


        internal async Task SaveResizedImage(IFormFile file, string filePath, string fileName)
        {
            using Image img = await Image.LoadAsync(file.OpenReadStream());
            img.Mutate(i => i.Resize(img.Width / 3, img.Height / 3));
            img.Save(Path.Combine(filePath, Path.GetFileNameWithoutExtension(fileName)+"small"+Path.GetExtension(fileName)));
        }
    }
}
