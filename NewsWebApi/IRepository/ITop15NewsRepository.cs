using NewsWebApi.Models;

namespace NewsWebApi.IRepository
{
    public interface ITop15NewsRepository
    {
        public IQueryable<NewModel> GetAllNews();
        public Task<IEnumerable<Top15NewsModel>> GetAllAsync();
        public Task<Top15NewsModel> GetAsync(int id);
        public void Delete(Top15NewsModel top15NewsModel);
        public Task AddAsync(Top15NewsModel top15NewsModel);
    }
}
