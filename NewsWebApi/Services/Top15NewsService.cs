using Microsoft.EntityFrameworkCore;
using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;
using SharedLib.DTO;

namespace NewsWebApi.Services
{
    public class Top15NewsService
    {
        public ITop15NewsRepository Top15NewsRepository { get; }
        public AppUnitWork AppUnitWork { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public Top15NewsService(ITop15NewsRepository top15NewsRepository , AppUnitWork appUnitWork, IHttpContextAccessor httpContextAccessor)
        {
            Top15NewsRepository = top15NewsRepository;
            AppUnitWork = appUnitWork;
            HttpContextAccessor = httpContextAccessor;
        }
        private string GetFullPathImage(string fileName)
        {
            return Path.Combine("https://" + HttpContextAccessor.HttpContext.Request.Host.Value, "sfroot\\images\\news", fileName).Replace("\\", "/");
        }
        public async Task<ResponseDTO<IEnumerable<NewDTO>>> GetAllAsync()
        {
            return await GetAllAsync(15 , 0);
        }
        public async Task<ResponseDTO<object>> AddAsync(int newsId) {
            var isINNews = await AppUnitWork.AppDbContext.Set<NewModel>().AnyAsync(x => x.Id == newsId);
            if(isINNews == false)
                return new ResponseDTO<object>()
                {
                    Success = false,
                    Message = "لم يتم العثور علي المقاله"
                };

            var topNews = await Top15NewsRepository.GetAllAsync();
            if (topNews.Where(x => x.NewModelId == newsId).FirstOrDefault()!=null) {
                return new ResponseDTO<object>()
                {
                    Success = false,
                    Message = "موجود بالفعل"
                };
            }
            var maxRank = topNews.LastOrDefault();
            var newRank = maxRank == null?1: (maxRank.rank + 1);
            if (topNews.Count() == 15) {
                var oldRank = topNews.FirstOrDefault();
                Top15NewsRepository.Delete(oldRank);
            }
            var newTopNews = new Top15NewsModel {
                NewModelId = newsId,
                rank = newRank,
            };
            await Top15NewsRepository.AddAsync(newTopNews);
            await AppUnitWork.SaveChangesAsync();
            return new ResponseDTO<object>()
            {
                Success = true,
            };
        }
        public async Task<ResponseDTO<object>> DeleteAsync(int newsId) { 
            var topNew = await Top15NewsRepository.GetAsync(newsId);
            if (topNew != null) {
                Top15NewsRepository.Delete(topNew);
                await AppUnitWork.SaveChangesAsync();
            }
            return new ResponseDTO<object>
            {
                Success = true
            };
        }
        public async Task<ResponseDTO<object>> SetUpAsync(int newsId) { 
            var topNews = await Top15NewsRepository.GetAllAsync();
            var curruntTop = topNews.Where(x => x.NewModelId == newsId).FirstOrDefault();
            if (curruntTop == null || topNews.Count() <= 1) {
                return new ResponseDTO<object>
                {
                    Success = false,
                    Message = "News not found"
                };
            }

            var nextNewsRankTop = topNews.Where(x => x.rank > curruntTop.rank).FirstOrDefault();
            if (nextNewsRankTop == null)
            {
                return new ResponseDTO<object>
                {
                    Success = false,
                    Message = "News not found"
                };
            }
            else {
                var tempRank = curruntTop.rank;
                curruntTop.rank = nextNewsRankTop.rank;
                nextNewsRankTop.rank = tempRank;
                await AppUnitWork.SaveChangesAsync();
                return new ResponseDTO<object>
                {
                    Success = true,
                };
            }
        }
        public async Task<ResponseDTO<object>> SetDownAsync(int newsId)
        {
            var topNews = await Top15NewsRepository.GetAllAsync();
            var curruntTop = topNews.Where(x => x.NewModelId == newsId).FirstOrDefault();
            if (curruntTop == null || topNews.Count() <= 1)
            {
                return new ResponseDTO<object>
                {
                    Success = false,
                    Message = "News not found"
                };
            }

            var nextNewsRankTop = topNews.Where(x => x.rank < curruntTop.rank).LastOrDefault();
            if (nextNewsRankTop == null)
            {
                return new ResponseDTO<object>
                {
                    Success = false,
                    Message = "News not found"
                };
            }
            else
            {
                var tempRank = curruntTop.rank;
                curruntTop.rank = nextNewsRankTop.rank;
                nextNewsRankTop.rank = tempRank;
                await AppUnitWork.SaveChangesAsync();
                return new ResponseDTO<object>
                {
                    Success = true,
                };
            }
        }
        public async Task<ResponseDTO<IEnumerable<NewDTO>>> GetAllAsync(int take , int skip) {
            var newsModels = await Top15NewsRepository.GetAllNews().Take(take).Skip(skip).ToListAsync();
            var newsModelsDto = newsModels.Select(x => new NewDTO
            {
                Id = x.Id,
                Image = GetFullPathImage(x.ImageUrl),
                Section = x.SectionModel.Name,
                Title = x.Title,
            });
            return new ResponseDTO<IEnumerable<NewDTO>>
            {
                Data = newsModelsDto,
                Success = true
            };
        }


        public async Task<ResponseDTO<IEnumerable<NewDTO>>> GetTop4Async()
        {
            return await GetAllAsync(4, 0);
       
        }

        public async Task<ResponseDTO<IEnumerable<NewDTO>>> GetLast11Async()
        {
            return await GetAllAsync(11, 4);

        }


    }
}
