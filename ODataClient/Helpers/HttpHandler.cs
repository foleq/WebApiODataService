using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace ODataClient.Helpers
{
    public interface IHttpHandler
    {
        Task<T> GetAsync<T>(string url);
        Task<T> PostJsonAsync<T>(string url, string jsonData);
    }

    public class HttpHandler : IHttpHandler
    {
        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public async Task<T> PostJsonAsync<T>(string url, string jsonData)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                var json = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
