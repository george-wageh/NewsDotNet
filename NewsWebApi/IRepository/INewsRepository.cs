using NewsWebApi.Models;

namespace NewsWebApi.IRepository
{
    public interface INewsRepository
    {
        public Task<NewModel> GetAsync(int id);

        public IQueryable<NewModel> EmptyQuery();

        public IQueryable<NewModel> Search(string qstring);

        public IQueryable<NewModel> GetInSection(int sectionId);

        public IQueryable<NewModel> GetAll();

        public Task<IEnumerable<NewModel>> GetTop5Views();

        public Task AddAsync(NewModel newModel);

        public void Delete(NewModel newModel);
    }
}
