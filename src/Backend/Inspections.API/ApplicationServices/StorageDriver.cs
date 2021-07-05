using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.ApplicationServices
{
    public interface StorageDriver
    {
        Task CreatFolderIfNotExists(string folderPath);
        Task DeleteFile(string filePath);
        Task DeleteFolder(string path);
        Task DeleteFolder(string path, bool recursive = true);
        string GenerateFilePath(string path, string fileName, bool useUniqueString = false);
        Task<string[]> GetFilesInFolder(string folder);
        string Save(Stream fileStream, string path, string fileName, bool useUniqueString, string contentType = "");
        Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString, string contentType = "");
    }
}
