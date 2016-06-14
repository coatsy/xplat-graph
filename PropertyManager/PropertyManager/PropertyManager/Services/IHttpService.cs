using System;
using System.Threading.Tasks;

namespace PropertyManager.Services
{
    public interface IHttpService
    {
        Uri Endpoint { get; set; }

        string AccessToken { get; set; }

        Task<T> GetAsync<T>(string resource);

        //Task<T[]> GetCollectionAsync<T>(string requestUri);
    }
}
