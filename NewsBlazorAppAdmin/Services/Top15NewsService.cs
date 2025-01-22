using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Services
{
    public class Top15NewsService
    {
        public Top15NewsService(ApiServices apiServices) {
            ApiServices = apiServices;
        }

        public ApiServices ApiServices { get; }

        public async Task<ResponseDTO<IEnumerable<NewDTO>>> GetAll() {
            return await ApiServices.GetJsonAsync<ResponseDTO<IEnumerable<NewDTO>>>("/api/Top15News");
        }

        public async Task<ResponseDTO<object>> SetUp(int newsId)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<object>>("/api/Top15News/SetUp", newsId);
        }

        public async Task<ResponseDTO<object>> SetDown(int newsId)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<object>>("/api/Top15News/SetDown", newsId);
        }
        public async Task<ResponseDTO<object>> Push(int newsId)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<object>>("/api/Top15News/Push", newsId);
        }
        public async Task<ResponseDTO<object>> Delete(int newsId)
        {
            return await ApiServices.DeleteJsonAsync<ResponseDTO<object>>($"/api/Top15News?newsId={newsId}");
        }
    }
}
