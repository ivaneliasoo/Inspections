using System.IO;
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
        Task<StorageItem> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString, string contentType = "");
    }
}
