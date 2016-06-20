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

        Task<T> PutAsync<T>(string name, Stream stream, string contentType);
    }
}
