using Microsoft.EntityFrameworkCore;
using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;

namespace NewsWebApi.Repository
{
    public class SectionsRepository:ISectionsRepository
    {
        public SectionsRepository(AppDbContext appDbContext )
        {
            AppDbContext = appDbContext;
        }

        public AppDbContext AppDbContext { get; }

        public async Task AddAsync(SectionModel sectionModel)
        {
            await AppDbContext.Set<SectionModel>().AddAsync(sectionModel);
        }

        public async Task<IEnumerable<SectionModel>> GetAllAsync() {
           return await AppDbContext.Set<SectionModel>().OrderBy(x=>x.Rank).ToListAsync();
        }

        public async Task<IEnumerable<SectionModel>> GetAllDecAsync()
        {
            return await AppDbContext.Set<SectionModel>().OrderByDescending(x => x.Rank).ToListAsync();
        }

        public async Task<SectionModel> GetAsync(int id) {
            return await AppDbContext.Set<SectionModel>().FirstOrDefaultAsync(x => x.Id == id);
        }


    }
}
