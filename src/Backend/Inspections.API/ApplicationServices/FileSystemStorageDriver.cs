using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.ApplicationServices
{
    public class FileSystemStorageDriver : StorageDriver
    {
        public async Task<StorageItem> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString, string contenType = "")
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            await CreatFolderIfNotExists(path);

            using (var stream = File.Create(filePath))
            {
                await fileStream.CopyToAsync(stream).ConfigureAwait(false);
            }
            return new StorageItem { Path = filePath };
        }

        public string Save(Stream fileStream, string path, string fileName, bool useUniqueString, string contentType)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);

            CreatFolderIfNotExists(path);

            using (var stream = File.Create(filePath))
            {
                fileStream.CopyTo(stream);
            }
            return filePath;
        }

        public Task DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }

        public Task DeleteFolder(string path)
        {
            if (Directory.Exists(path))
            {
                var infoDir = new DirectoryInfo(path);
                foreach (var file in infoDir.GetFiles())
                {
                    file.Delete();
                }
            }
            return Task.CompletedTask;
        }

        public Task DeleteFolder(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
            return Task.CompletedTask;
        }

        public Task<string[]> GetFilesInFolder(string folder)
        {
            try
            {
                return Task.FromResult(Directory.GetFiles(folder));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task CreatFolderIfNotExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return Task.CompletedTask;
        }

        public string GenerateFilePath(string path, string fileName, bool useUniqueString = false)
        {
            //replace invalid chars in path 
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

            if (useUniqueString)
                return Path.Combine(path, fileName.Insert(fileName.LastIndexOf('.'), DateTime.Now.Ticks.ToString()));
            else
                return Path.Combine(path, fileName);
        }
    }
}
