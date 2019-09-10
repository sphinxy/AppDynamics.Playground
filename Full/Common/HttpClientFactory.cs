using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Common
{
    public interface IHttpClientFactory
    {
        HttpClient Get(string address);
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IDictionary<string, HttpClient> _httpClients;

        public HttpClientFactory(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClients = new ConcurrentDictionary<string, HttpClient>();
        }

        public HttpClient Get(string address)
        {
            if (!_httpClients.ContainsKey(address))
            {
                var uri = new Uri(address);
                _httpClients.Add(address,
                    new HttpClient(GetHandler(uri)) {BaseAddress = uri});
            }

            return _httpClients[address];
        }

        public DelegatingHandler GetHandler(Uri uri)
        {
            return new DummyDelegatingHandler(new HttpClientHandler());
        }
    }

    public class DummyDelegatingHandler : DelegatingHandler
    {
        public DummyDelegatingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
            
        }
    }
}