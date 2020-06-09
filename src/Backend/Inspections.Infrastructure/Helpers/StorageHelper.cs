using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Inspections.Core.Interfaces;
using Microsoft.Extensions.Options;
using Ardalis.GuardClauses;

namespace Inspections.Infrastructure.Helpers
{
    public class StorageHelper : IStorageHelper
    {
        private readonly string TempFolderRoute;
        public StorageHelper(string tempFolderRoute)
        {
            Guard.Against.NullOrWhiteSpace(tempFolderRoute, nameof(tempFolderRoute));
            TempFolderRoute = tempFolderRoute;
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string SaveOnTempFolder(byte[] fileContent, string fileName, bool useUniqueString)
        {
            return Save(fileContent, TempFolderRoute, fileName, useUniqueString);
        }

        public string SaveOnTempFolder(Stream fileContent, string fileName, bool useUniqueString)
        {
            return Save(fileContent, TempFolderRoute, fileName, useUniqueString);
        }

        public async Task<string> SaveOnTempFolderAsync(Stream fileContent, string fileName, bool useUniqueString)
        {
            return await SaveAsync(fileContent, TempFolderRoute, fileName, useUniqueString).ConfigureAwait(false);
        }

        public string Save(byte[] fileContent, string path, string fileName, bool useUniqueString)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                stream.Write(fileContent, 0, fileContent.Length);
            }
            return filePath;
        }

        public async Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            CreatFolderIfNotExists(path);

            using (var stream = File.Create(filePath))
            {
                await fileStream.CopyToAsync(stream).ConfigureAwait(false);
            }
            return filePath;
        }

        public string Save(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);

            CreatFolderIfNotExists(path);

            using (var stream = File.Create(filePath))
            {
                fileStream.CopyTo(stream);
            }
            return filePath;
        }

        public string Save(string path, string fileName, bool useUniqueString, out string savedFileName)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            if (!File.Exists(filePath))
                File.Create($"{filePath}");
            savedFileName = filePath.Replace(path, "");
            return filePath;
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public void DeleteFilesInFolder(string path)
        {
            if (Directory.Exists(path))
            {
                var infoDir = new DirectoryInfo(path);
                foreach (var file in infoDir.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        public bool DeleteFileInFolder(string path, string name)
        {
            try
            {
                var files = Directory.GetFiles(path, name);

                foreach (string file in files)
                {
                    File.Delete(file);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void DeleteFolder(string path)
        {
            if (Directory.Exists(path))
            {
                var infoDir = new DirectoryInfo(path);
                foreach (var file in infoDir.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        public void DeleteFolder(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }

        public string[] GetFilesInFolder(string folder)
        {
            try
            {
                return Directory.GetFiles(folder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CreatFolderIfNotExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
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

        public async Task<MemoryStream> GetMemoryStreamFromFile(string fullPath)
        {
            MemoryStream msResult;
            if (File.Exists(fullPath))
            {
                using (var file = File.OpenRead(fullPath))
                {
                    msResult = new MemoryStream();
                    await file.CopyToAsync(msResult).ConfigureAwait(false);
                    file.Close();
                }
            }
            else
                throw new FileNotFoundException("File not exists", fullPath);

            return msResult;

        }

        public void CopyFile(string source, string target, bool ovewrite = true)
        {
            if (File.Exists(source))
                File.Copy(source, target, ovewrite);
        }

        public void Move(string sourceFileName, string pathDest)
        {
            if (File.Exists(sourceFileName))
            {
                try
                {
                    var fileName = Path.GetFileName(sourceFileName);

                    File.Move(sourceFileName, $"{pathDest}{fileName}");
                }
                catch (Exception e)
                {
                    throw new Exception("Error moving file: " + e.Message);
                }
            }
            else
            {
                throw new FileNotFoundException("File not exists", sourceFileName);
            }
        }
    }

}