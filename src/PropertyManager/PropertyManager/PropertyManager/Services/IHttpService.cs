﻿using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PropertyManager.Services
{
    public interface IHttpService
    {
        Uri Resource { get; set; }

        HttpRequestHeaders GetRequestHeaders();

        Task<T> GetAsync<T>(string resource);

        Task<T> PostAsync<T>(string resource, object data);

        Task<T> PostAsync<T>(string resource, Stream stream, string contentType);

        Task<T> PutAsync<T>(string resource, object data);

        Task<T> PutAsync<T>(string resource, Stream stream, string contentType);

        Task<T> PatchAsync<T>(string resource, object data);

        Task<T> PatchAsync<T>(string resource, Stream stream, string contentType);

        Task<T> SendAsync<T>(string resource, HttpMethod httpMethod, object data);

        Task<T> SendAsync<T>(string resource, HttpMethod httpMethod, Stream stream, string contentType);
    }
}
