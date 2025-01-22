using SharedLib.DTO;
using System.Net.Http.Json;

namespace NewsBlazorAppAdmin.Services
{
    public class ApiServices
    {
        public ApiServices(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
            httpClient = HttpClientFactory.CreateClient("Client");
        }

        public IHttpClientFactory HttpClientFactory { get; }
        public HttpClient httpClient { get; }

        public async Task<O> PostJsonAsync<O>(string urlPath, Object i) where O : class  {


            try
            {
                var response = await httpClient.PostAsJsonAsync(urlPath, i);
                var responseDto = await response.Content.ReadFromJsonAsync<O>();
                return responseDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<O> PostAsync<O>(string urlPath, HttpContent i) where O : class
        {
            try
            {
                var response = await httpClient.PostAsync(urlPath, i);
                var responseDto = await response.Content.ReadFromJsonAsync<O>();
                return responseDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<O> PutJsonAsync<O>(string urlPath, Object i) where O : class
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync(urlPath, i);
                var responseDto = await response.Content.ReadFromJsonAsync<O>();
                return responseDto;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public async Task<O> GetJsonAsync<O>(string urlPath) where O : class
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<O>(urlPath);
                return response;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public async Task<O> DeleteJsonAsync<O>(string urlPath) where O : class
        {
            try
            {
                var response = await httpClient.DeleteFromJsonAsync<O>(urlPath);
                return response;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

    }
}
