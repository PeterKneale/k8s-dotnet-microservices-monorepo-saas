using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Media.FunctionalTests
{
    public static class HttpHelper
    {
        public static async Task<HttpStatusCode> UploadFile(Uri url)
        {
            var embeddedFileName = "Media.FunctionalTests.Content.google.png";
            await using var stream = typeof(HttpHelper).Assembly.GetManifestResourceStream(embeddedFileName);

            using var client = new HttpClient();
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            var response = await client.PutAsync(url, content);
            return response.StatusCode;
        }
        
        public static async Task<HttpStatusCode> DownloadFile(Uri url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            return response.StatusCode;
        }
    }
}