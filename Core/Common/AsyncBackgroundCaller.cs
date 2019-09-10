using System;
using System.Threading.Tasks;

namespace Common
{
    public interface IAsyncBackgroundCaller
    {
        Task<string> Call(string baseAddress, string relativeAddress);
    }
    public class AsyncBackgroundCaller : IAsyncBackgroundCaller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AsyncBackgroundCaller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Call(string baseAddress, string relativeAddress)
        {
            var httpClient = _httpClientFactory.Get(baseAddress);
            var responseMessage = await httpClient.GetAsync(relativeAddress);
            return (await responseMessage.Content.ReadAsStringAsync()).Length>0 ? baseAddress+" ok" : baseAddress+" fail";
        }
    }
}