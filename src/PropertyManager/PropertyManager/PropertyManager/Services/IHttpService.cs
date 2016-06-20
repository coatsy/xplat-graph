using System;
using System.IO;
using System.Threading.Tasks;

namespace PropertyManager.Services
{
    public interface IHttpService
    {
        Uri Endpoint { get; set; }

        string AccessToken { get; set; }

        Task<T> GetAsync<T>(string resource);

        Task<T> PostAsync<T>(string resource, object data);

        Task<T> PostAsync<T>(string resource, Stream stream, string contentType);

        Task<T> PutAsync<T>(string resource, object data);

        Task<T> PutAsync<T>(string resource, Stream stream, string contentType);

        Task<T> PatchAsync<T>(string resource, object data);

        Task<T> PatchAsync<T>(string resource, Stream stream, string contentType);
    }
}
