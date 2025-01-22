using Microsoft.EntityFrameworkCore;
using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;
using SharedLib.DTO;

namespace NewsWebApi.Repository
{

    public class Top15NewsRepository:ITop15NewsRepository
    {
        public Top15NewsRepository(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public AppDbContext AppDbContext { get; }

 

        public IQueryable<NewModel> GetAllNews()
        {
            return AppDbContext.Set<Top15NewsModel>().OrderByDescending(x => x.rank).Include(x=>x.NewModel).ThenInclude(x=>x.SectionModel).Select(x=>x.NewModel);
        }

        public async Task<IEnumerable<Top15NewsModel>> GetAllAsync() {
            return await AppDbContext.Set<Top15NewsModel>().OrderBy(x=>x.rank).ToListAsync();
        }

        public void Delete(Top15NewsModel top15NewsModel)
        {
            AppDbContext.Set<Top15NewsModel>().Remove(top15NewsModel);
        }

        public async Task AddAsync(Top15NewsModel top15NewsModel)
        {
           await AppDbContext.Set<Top15NewsModel>().AddAsync(top15NewsModel);
        }

        public async Task<Top15NewsModel> GetAsync(int id)
        {
            return await AppDbContext.Set<Top15NewsModel>().Where(x=>x.NewModelId == id).FirstOrDefaultAsync();
        }
    }
}
