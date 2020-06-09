using System.IO;
using System.Threading.Tasks;

namespace Inspections.Core.Interfaces
{
    public interface IStorageHelper
    {
        void CopyFile(string source, string target, bool ovewrite = true);
        void CreatFolderIfNotExists(string folderPath);
        void DeleteFile(string filePath);
        bool DeleteFileInFolder(string path, string name);
        void DeleteFilesInFolder(string path);
        void DeleteFolder(string path);
        void DeleteFolder(string path, bool recursive = true);
        bool FileExists(string filePath);
        string GenerateFilePath(string path, string fileName, bool useUniqueString = false);
        string[] GetFilesInFolder(string folder);
        Task<MemoryStream> GetMemoryStreamFromFile(string fullPath);
        void Move(string sourceFileName, string pathDest);
        string Save(byte[] fileContent, string path, string fileName, bool useUniqueString);
        string Save(Stream fileStream, string path, string fileName, bool useUniqueString);
        string Save(string path, string fileName, bool useUniqueString, out string savedFileName);
        Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString);
        string SaveOnTempFolder(byte[] fileContent, string fileName, bool useUniqueString);
        string SaveOnTempFolder(Stream fileContent, string fileName, bool useUniqueString);
        Task<string> SaveOnTempFolderAsync(Stream fileContent, string fileName, bool useUniqueString);
    }
}