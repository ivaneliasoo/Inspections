using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Inspections.API.Extensions
{
    public static class UriExtensions
    {
        public static async Task<string> ToBase64String(this Uri contentUrl)
        {
            var fileBytes = Array.Empty<byte>();
            try
            {
                using var httpClient = new HttpClient();
                fileBytes = await httpClient.GetByteArrayAsync(contentUrl);
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            return "data:image/png;base64," + Convert.ToBase64String(fileBytes);
        }
    }
}
