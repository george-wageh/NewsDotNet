using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;
using SharedLib.DTO;

namespace NewsWebApi.Services
{
    public class SectionsService
    {
        public SectionsService(ISectionsRepository sectionsRepository, AppUnitWork appUnitWork)
        {
            SectionsRepository = sectionsRepository;
            AppUnitWork = appUnitWork;
        }

        public ISectionsRepository SectionsRepository { get; }
        public AppUnitWork AppUnitWork { get; }

        public async Task<ResponseDTO<int>> AddAsync(SectionDTO sectionDTO)
        {
            var sections = await SectionsRepository.GetAllAsync();
            var maxRank = sections.LastOrDefault();
            var newRank = maxRank == null ? 1 : (maxRank.Rank + 1);
            var section = new SectionModel
            {
                Name = sectionDTO.Name,
                Rank = newRank
            };
            await SectionsRepository.AddAsync(section);
            await AppUnitWork.SaveChangesAsync();
            var id = section.Id;
            return new ResponseDTO<int> { Success = true, Data = id };
        }
        public async Task<ResponseDTO<object>> SetUpAsync(int sectionId)
        {
            var sections = await SectionsRepository.GetAllAsync();
            var curruntTop = sections.Where(x => x.Id == sectionId).FirstOrDefault();
            if (curruntTop == null)
            {
                return new ResponseDTO<object>
                {
                    Success = false,
                    Message = "Section not found"
                };
            }

            var nextNewsRankTop = sections.Where(x => x.Rank > curruntTop.Rank).FirstOrDefault();
            if (nextNewsRankTop == null)
            {
                return new ResponseDTO<object>
                {
                    Success = false,
                    Message = "Section not found"
                };
            }
            else
            {
                var tempRank = curruntTop.Rank;
                curruntTop.Rank = nextNewsRankTop.Rank;
                nextNewsRankTop.Rank = tempRank;
                await AppUnitWork.SaveChangesAsync();
                return new ResponseDTO<object>
                {
                    Success = true,
                };
            }
        }
        public async Task<ResponseDTO<object>> SetDownAsync(int sectionId)
        {
            var sections = await SectionsRepository.GetAllAsync();
            var curruntTop = sections.Where(x => x.Id == sectionId).FirstOrDefault();
            if (curruntTop == null)
            {
                return new ResponseDTO<object>
                {
                    Success = false,
                    Message = "Section not found"
                };
            }

            var nextNewsRankTop = sections.Where(x => x.Rank < curruntTop.Rank).LastOrDefault();
            if (nextNewsRankTop == null)
            {
                return new ResponseDTO<object>
                {
                    Success = false,
                    Message = "Section not found"
                };
            }
            else
            {
                var tempRank = curruntTop.Rank;
                curruntTop.Rank = nextNewsRankTop.Rank;
                nextNewsRankTop.Rank = tempRank;
                await AppUnitWork.SaveChangesAsync();
                return new ResponseDTO<object>
                {
                    Success = true,
                };
            }
        }

        public async Task<ResponseDTO<object>> EditAsync(SectionDTO sectionDTO)
        {

            var section = await SectionsRepository.GetAsync(sectionDTO.Id);
            if (section != null)
            {
                section.Name = sectionDTO.Name;
                await AppUnitWork.SaveChangesAsync();
                return new ResponseDTO<object> { Success = true };

            }
            else
            {
                return new ResponseDTO<object> { Success = false, Message = "Section not found" };

            }
        }
        public async Task<ResponseDTO<IEnumerable<SectionDTO>>> GetAllAsync()
        {
            var sections = await SectionsRepository.GetAllDecAsync();
            var sectionsDTO = sections.Select(x => new SectionDTO
            {
                Id = x.Id,
                Name = x.Name
            });
            return new ResponseDTO<IEnumerable<SectionDTO>>
            {
                Success = true,
                Data = sectionsDTO
            };
        }


    }
}
