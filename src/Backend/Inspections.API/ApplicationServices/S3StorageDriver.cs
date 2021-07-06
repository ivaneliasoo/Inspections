using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Inspections.API.Models.Configuration;
using Microsoft.Extensions.Options;

namespace Inspections.API.ApplicationServices
{
    public class S3StorageDriver : StorageDriver
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly IOptions<ClientSettings> options;

        public S3StorageDriver(IAmazonS3 amazonS3, IOptions<ClientSettings> options)
        {
            _amazonS3 = amazonS3 ?? throw new ArgumentNullException(nameof(amazonS3));
            this.options = options ?? throw new ArgumentNullException(nameof(options));

        }

        public async Task CreatFolderIfNotExists(string folderPath)
        {
            string key = $"{folderPath.Replace("\\", "/")}/";
            GetObjectResponse? obj = default!;
            try
            {
                obj = await _amazonS3.GetObjectAsync(options.Value.S3BucketName, key);
            }
            catch (Exception)
            {
                if (obj is null)
                {
                    var uploadObject = new PutObjectRequest
                    {
                        BucketName = options.Value.S3BucketName,
                        Key = key
                    };
                    var response = await _amazonS3.PutObjectAsync(uploadObject);
                }
            }
        }

        public async Task DeleteFile(string filePath)
        {
            var delRequest = new DeleteObjectRequest
            {
                BucketName = options.Value.S3BucketName,
                Key = filePath
            };
            await _amazonS3.DeleteObjectAsync(delRequest);
        }

        public async Task DeleteFolder(string path)
        {
            var delRequest = new DeleteObjectRequest
            {
                BucketName = options.Value.S3BucketName,
                Key = path
            };
            await _amazonS3.DeleteObjectAsync(delRequest);
        }

        public async Task DeleteFolder(string path, bool recursive = true)
        {
            var delRequest = new DeleteObjectRequest
            {
                BucketName = options.Value.S3BucketName,
                Key = path
            };
            await _amazonS3.DeleteObjectAsync(delRequest);
        }

        public string GenerateFilePath(string path, string fileName, bool useUniqueString = false)
        {
            return $"{path.Replace("\\", "/")}/{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now.Ticks}_{Path.GetExtension(fileName)}";
        }

        public async Task<string[]> GetFilesInFolder(string folder)
        {
            List<string> fileList = new List<string>();
            var result = await _amazonS3.ListObjectsAsync(options.Value.S3BucketName);
            foreach (var obj in result.S3Objects)
            {
                var url = _amazonS3.GeneratePreSignedURL(options.Value.S3BucketName, obj.Key, DateTime.Now.AddHours(8), null);
                fileList.Add(url);
            }

            return fileList.ToArray();
        }

        public string Save(Stream fileStream, string path, string fileName, bool useUniqueString, string contentType = "")
        {
            throw new NotImplementedException("Synchronous Support for saving file isn't implemented. since awsClient only support async");
        }

        public async Task<StorageItem> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString, string contentType = "")
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            var uploadObject = new PutObjectRequest
            {
                BucketName = options.Value.S3BucketName,
                Key = filePath,
                InputStream = fileStream,
                ContentType = contentType
            };
            var response = await _amazonS3.PutObjectAsync(uploadObject);
            return new StorageItem { CloudId = response.ETag, Path = filePath };
        }
    }
}
