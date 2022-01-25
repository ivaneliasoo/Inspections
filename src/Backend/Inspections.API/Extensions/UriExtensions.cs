using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Inspections.API.Extensions
{
    public static class UriExtensions
    {
        public static async Task<string> ToBase64String(this Uri contentUrl)
        {
            var fileBytes = Array.Empty<byte>();
            using var httpClient = new HttpClient();
            fileBytes = await httpClient.GetByteArrayAsync(contentUrl);
            return "data:image/png;base64," + Convert.ToBase64String(fileBytes);
        }
    }
}
