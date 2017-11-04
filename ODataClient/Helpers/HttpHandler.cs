using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ODataClient.Helpers
{
    public interface IHttpHandler
    {
        Task<T> GetAsync<T>(string url);
    }

    public class HttpHandler : IHttpHandler
    {
        private readonly HttpClient _client;

        public HttpHandler()
        {
            _client = new HttpClient();
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
