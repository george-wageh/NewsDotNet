using Microsoft.EntityFrameworkCore;
using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;
using System.Collections;
using System.Linq;

namespace NewsWebApi.Repository
{
    public class NewsRepository:INewsRepository
    {
        public NewsRepository(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public AppDbContext AppDbContext { get; }

        public async Task<NewModel> GetAsync(int id) {
            return await AppDbContext.Set<NewModel>().Where(x => x.Id == id).Include(x => x.SectionModel).FirstOrDefaultAsync();
        }

        public IQueryable<NewModel> Search(string qstring )
        {
            //return AppDbContext.Set<NewModel>().Where(x => x.Title == qstring);
            return AppDbContext.Set<NewModel>().Where(x => (x.Title.Contains(qstring) || x.Description.Contains(qstring)));
        }

        public IQueryable<NewModel> GetInSection(int sectionId) {
            return AppDbContext.Set<NewModel>().Where(x => x.SectionId == sectionId);
        }

        public async Task<IEnumerable<NewModel>> GetTop5Views() { 
            return await AppDbContext.Set<NewModel>().OrderByDescending(x=>x.ViewsCount)
                .Take(5).Include(x=>x.SectionModel).ToListAsync();
        }

        public async Task AddAsync(NewModel newModel) {
            await AppDbContext.Set<NewModel>().AddAsync(newModel);
        }


        public void Delete(NewModel newModel) {
            AppDbContext.Set<NewModel>().Remove(newModel);
        }

        public IQueryable<NewModel> EmptyQuery()
        {
           return AppDbContext.Set<NewModel>().Where(x => false);
        }

        public IQueryable<NewModel> GetAll()
        {
            return AppDbContext.Set<NewModel>().Where(x => true);

        }
    }
}
