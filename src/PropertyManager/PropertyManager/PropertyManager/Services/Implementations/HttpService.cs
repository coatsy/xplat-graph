using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PropertyManager.Extensions;

namespace PropertyManager.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public HttpService()
        {
            _httpClient = new HttpClient();
        }

        public Uri Resource { get; set; }

        public HttpRequestHeaders GetRequestHeaders()
        {
            return _httpClient.DefaultRequestHeaders;
        }

        public async Task<T> GetAsync<T>(string resource)
        {
            // Get the response.
            var response = await _httpClient.GetStringAsync(
                new Uri(Resource.AbsoluteUri + resource));

            // Parse the response.
            var result = JsonConvert.DeserializeObject<T>(response);
            return result;
        }

        public async Task<T> PostAsync<T>(string resource, object data)
        {
            var str = JsonConvert.SerializeObject(data, _jsonSerializerSettings);
            using (var stream = GenerateStreamFromString(str))
            {
                return await PostAsync<T>(resource, stream, Constants.JsonContentType);
            }
        }

        public async Task<T> PostAsync<T>(string resource, Stream stream, string contentType)
        {
            // Create content.
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            // Get the response.
            var response = await _httpClient.PostAsync(
                new Uri(Resource.AbsoluteUri + resource), streamContent);

            // Check response.
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            // Parse the response.
            var result = JsonConvert.DeserializeObject<T>(
                await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<T> PutAsync<T>(string resource, object data)
        {
            var str = JsonConvert.SerializeObject(data, _jsonSerializerSettings);
            using (var stream = GenerateStreamFromString(str))
            {
                return await PutAsync<T>(resource, stream, Constants.JsonContentType);
            }
        }

        public async Task<T> PutAsync<T>(string resource, Stream stream, string contentType)
        {
            // Create content.
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            // Get the response.
            var response = await _httpClient.PutAsync(
                new Uri(Resource.AbsoluteUri + resource), streamContent);

            // Check response.
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            // Parse the response.
            var result = JsonConvert.DeserializeObject<T>(
                await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<T> PatchAsync<T>(string resource, object data)
        {
            var str = JsonConvert.SerializeObject(data, _jsonSerializerSettings);
            using (var stream = GenerateStreamFromString(str))
            {
                return await PatchAsync<T>(resource, stream, Constants.JsonContentType);
            }
        }

        public async Task<T> PatchAsync<T>(string resource, Stream stream, string contentType)
        {
            // Create content.
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            // Get the response.
            var response = await _httpClient.PatchAsync(
                new Uri(Resource.AbsoluteUri + resource), streamContent);

            // Check response.
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            // Parse the response.
            var result = JsonConvert.DeserializeObject<T>(
                await response.Content.ReadAsStringAsync());
            return result;
        }

        private static Stream GenerateStreamFromString(string str)
        {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            streamWriter.Write(str);
            streamWriter.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
