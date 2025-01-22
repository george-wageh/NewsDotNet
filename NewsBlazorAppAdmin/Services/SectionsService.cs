using SharedLib.DTO;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace NewsBlazorAppAdmin.Services
{
    public class SectionsService
    {
        public SectionsService(ApiServices apiServices)
        {
            ApiServices = apiServices;
        }

        public ApiServices ApiServices { get; }

        public async Task<ResponseDTO<int>> AddAsync(SectionDTO sectionDTO)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<int>>("/api/Sections", sectionDTO);
        }
        public async Task<ResponseDTO<object>> EditAsync(SectionDTO sectionDTO)
        {
            return await ApiServices.PutJsonAsync<ResponseDTO<object>>("/api/Sections", sectionDTO);
        }
        public async Task<ResponseDTO<IEnumerable<SectionDTO>>> GetAllAsync()
        {
            return await ApiServices.GetJsonAsync<ResponseDTO<IEnumerable<SectionDTO>>>("/api/Sections");
           
        }
        public async Task<ResponseDTO<object>> SetUp(int sectionId)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<object>>("/api/Sections/SetUp", sectionId);
        }

        public async Task<ResponseDTO<object>> SetDown(int sectionId)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<object>>("/api/Sections/SetDown", sectionId);
        }

    }
}
