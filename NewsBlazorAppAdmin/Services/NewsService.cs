using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedLib.AdminDTO;
using SharedLib.DTO;
using System.Net.Http.Json;

namespace NewsBlazorAppAdmin.Services
{
    public class NewsService
    {
        public NewsService(ApiServices apiServices)
        {
            ApiServices = apiServices;
        }

        public ApiServices ApiServices { get; }

        public async Task<ResponseDTO<int>> AddAsync(NewAddDTO newAddDTO)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<int>>("/api/News", newAddDTO);
        }
        public async Task<ResponseDTO<int>> EditAsync(NewAddDTO newAddDTO)
        {
            return await ApiServices.PutJsonAsync<ResponseDTO<int>>("/api/News", newAddDTO);
        }

        public async Task<ResponseDTO<object>> DeleteAsync(int id)
        {
            return await ApiServices.DeleteJsonAsync<ResponseDTO<object>>($"/api/News?newsId={id}");
        }

        public async Task<ResponseListNewsAdminDTO> Query(NewsQueryDTO newsQueryDTO) {

            return await ApiServices.PostJsonAsync<ResponseListNewsAdminDTO>("/api/News/AdminQuery", newsQueryDTO);
        }

        public async Task<ResponseDTO<NewDetailsDTO>> GetAsync(int Id)
        {
            return await ApiServices.GetJsonAsync<ResponseDTO<NewDetailsDTO>>($"/api/News/{Id}");
        }

        public async Task<ResponseDTO<string>> UploadImage(IBrowserFile file)
        {
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
            //5mb max file
            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize:512000*10));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            multipartFormDataContent.Add(fileContent , "file", file.Name);
            return await ApiServices.PostAsync<ResponseDTO<string>>("/api/News/UploadImage", multipartFormDataContent);
        }
    }
}
