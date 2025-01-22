using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;
using SharedLib.AdminDTO;
using SharedLib.DTO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NewsWebApi.Services
{
    public class NewsService
    {
        public NewsService(INewsRepository newsRepository,IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ISectionsRepository sectionsRepository,ITop15NewsRepository top15NewsRepository,AppUnitWork appUnitWork)
        {
            NewsRepository = newsRepository;
            WebHostEnvironment = webHostEnvironment;
            HttpContextAccessor = httpContextAccessor;
            SectionsRepository = sectionsRepository;
            Top15NewsRepository = top15NewsRepository;
            AppUnitWork = appUnitWork;

        }

        public INewsRepository NewsRepository { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public ISectionsRepository SectionsRepository { get; }
        public ITop15NewsRepository Top15NewsRepository { get; }
        public AppUnitWork AppUnitWork { get; }
        private string GetFullPathImage(string fileName) {
           return Path.Combine("https://" + HttpContextAccessor.HttpContext.Request.Host.Value, "sfroot\\images\\news", fileName).Replace("\\", "/");
        }
        public async Task<ResponseDTO<NewDetailsDTO>> GetAsync(int id)
        {
            var newModel = await NewsRepository.GetAsync(id);
            if (newModel == null)
            {
                return new ResponseDTO<NewDetailsDTO>
                {
                    Success = false,
                    Message = "news not found"
                };
            }
            else
            {
                return new ResponseDTO<NewDetailsDTO>
                {
                    Success = true,
                    Data = new NewDetailsDTO
                    {
                        Author = newModel.Author,
                        Date = newModel.Date,
                        Description = newModel.Description,
                        Id = newModel.Id,
                        Image = GetFullPathImage(newModel.ImageUrl),
                        Section = newModel.SectionModel.Name,
                        SectionId = newModel.SectionId,
                        Title = newModel.Title

                    }
                };
            }
        }


        public async Task<ResponseDTO<IEnumerable<NewDTO>>> GetLatestNewsSectionAsync(int sectionId)
        {
            var newsModels = await NewsRepository.GetInSection(sectionId).OrderByDescending(x => x.Id).Take(5).ToListAsync();
            return new ResponseDTO<IEnumerable<NewDTO>>
            {
                Success = true,
                Data = newsModels.Select(x => new NewDTO
                {
                    Id = x.Id,
                    Image = x.ImageUrl,
                    Section = x.SectionModel.Name,
                    Title = x.Title,
                }).ToList()
            };
        }


        public async Task<ResponseDTO<IEnumerable<NewDTO>>> GetMostViewsNewsSectionAsync()
        {
            var newsModels = await NewsRepository.GetTop5Views();
            return new ResponseDTO<IEnumerable<NewDTO>>
            {
                Success = true,
                Data = newsModels.Select(x => new NewDTO
                {
                    Id = x.Id,
                    Image = x.ImageUrl,
                    Section = x.SectionModel.Name,
                    Title = x.Title,
                }).ToList()
            };
        }


        public async Task<ResponseDTO<int>> AddAsync(NewAddDTO NewAddDTO)
        {
            var newModel = new NewModel
            {
                Author = NewAddDTO.Author,
                Date = DateTime.Now,
                Description = NewAddDTO.Description,
                ImageUrl = NewAddDTO.Image,
                SectionId = NewAddDTO.SectionId,
                Title = NewAddDTO.Title,
                ViewsCount = 0
            };
            await NewsRepository.AddAsync(newModel);
            await AppUnitWork.SaveChangesAsync();
            return new ResponseDTO<int>
            {
                Success = true,
                Data = newModel.Id
            };
        }

        public async Task<ResponseDTO<string>> UploadImage(IFormFile file) {
            if (file == null || file.Length == 0)
                return new ResponseDTO<string>
                {
                    Success = false,
                    Message = "File is invalid"
                };
            string uploadsFolder = Path.Combine(WebHostEnvironment.WebRootPath, "images\\news");
            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            // Create a unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Get the full file path
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file to disk
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return new ResponseDTO<string>
            {
                Success = true,
                Data = GetFullPathImage(uniqueFileName),
                Message = ""
            };

        }
        public async Task<ResponseDTO<int>> EditAsync(NewAddDTO NewAddDTO)
        {
            var newModel = await NewsRepository.GetAsync(NewAddDTO.Id);
            if (newModel ==null) {
                return new ResponseDTO<int>
                {
                    Success = false,
                    Message = "news not found"
                };
            }
            {
                newModel.Author = NewAddDTO.Author;
                newModel.SectionId = NewAddDTO.SectionId;
                newModel.Description = NewAddDTO.Description;
                newModel.ImageUrl = NewAddDTO.Image;
                newModel.Title = NewAddDTO.Title;
            }
            await AppUnitWork.SaveChangesAsync();
            return new ResponseDTO<int>
            {
                Success = true,
                Data = newModel.Id
            };
        }

        public async Task<ResponseDTO<object>> DeleteAsync(int id)
        {
            var newsModel = await this.NewsRepository.GetAsync(id);
            if (newsModel != null)
            {
                NewsRepository.Delete(newsModel);
                await AppUnitWork.SaveChangesAsync();
                return new ResponseDTO<object> { Success = true };
            }
            else
            {
                return new ResponseDTO<object>()
                {
                    Success = false,
                    Message = "news not found"
                };
            }
        }


        private async Task<(IEnumerable<NewModel> , int count ,SectionModel section)> GetAllNews(NewsQueryDTO newsQueryDTO) {
            var pageItemsCount = 5;

            var query = NewsRepository.EmptyQuery();

            var section = await SectionsRepository.GetAsync(newsQueryDTO.SectionId);

            query = newsQueryDTO.SectionId != 0 && section != null ?
             query.Union(NewsRepository.GetInSection(newsQueryDTO.SectionId)) :
             query.Union(NewsRepository.GetAll());

            newsQueryDTO.QString = newsQueryDTO.QString == null ? "" : newsQueryDTO.QString;
            query = query.Intersect(NewsRepository.Search(newsQueryDTO.QString));

            var count = await query.CountAsync();

            var news = await query.OrderByDescending(x => x.Id).Skip(pageItemsCount * (newsQueryDTO.PageNum <= 0 ? 0 : newsQueryDTO.PageNum - 1)).
                Take(pageItemsCount).Include(x => x.SectionModel).ToListAsync();
            return (news , count , section);

        }


        public async Task<ResponseListNewsAdminDTO> GetAllAdmin(NewsQueryDTO newsQueryDTO)
        {
            var pageItemsCount = 5;
            var (news , count , section) = await this.GetAllNews(newsQueryDTO);
            var query2 = Top15NewsRepository.GetAllNews();
            var top15NewsIds = query2.Select(x => x.Id).ToHashSet();

            var newsDTOs = news.Select(x => new NewAdminDTO
            {
                Id = x.Id,
                Image = GetFullPathImage(x.ImageUrl),
                Section = x.SectionModel.Name,
                Title = x.Title,
                IsInTopNews = top15NewsIds.Contains(x.Id),
            }).ToList();


            return new ResponseListNewsAdminDTO
            {
                Data = newsDTOs,
                PagesCount = (int)Math.Ceiling((decimal)count / pageItemsCount),
                SectionName = section == null ? "كل الفئات" : section.Name,
                Success = true
            };

        }


        public async Task<ResponseListNewsDTO> GetAllUser(NewsQueryDTO newsQueryDTO)
        {

            var pageItemsCount = 5;
            var (news, count, section) = await this.GetAllNews(newsQueryDTO);

            var newsDTOs = news.Select(x => new NewDTO
            {
                Id = x.Id,
                Image = GetFullPathImage(x.ImageUrl),
                Section = x.SectionModel.Name,
                Title = x.Title,
            }).ToList();

            return new ResponseListNewsDTO
            {
                Data = newsDTOs,
                PagesCount = (int)Math.Ceiling((decimal)count / pageItemsCount),
                SectionName = section == null ? "كل الفئات" : section.Name,
                Success = true
            };

        }
        public async Task<ResponseDTO<IEnumerable<NewDTO>>> GetAllInSection(int sectionId)
        {
            var query = NewsRepository.GetInSection(sectionId);
            var news = await query.OrderByDescending(x => x.Id).Skip(0).
                 Take(5).Include(x => x.SectionModel).ToListAsync();

            var newsDTOs = news.Select(x => new NewDTO
            {
                Id = x.Id,
                Image = GetFullPathImage(x.ImageUrl),
                Section = x.SectionModel.Name,
                Title = x.Title,
            }).ToList();

            return new ResponseDTO<IEnumerable<NewDTO>>
            {
                Data = newsDTOs,
                Success = true
            };

        }
    }
}
