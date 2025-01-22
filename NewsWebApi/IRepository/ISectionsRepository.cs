using NewsWebApi.Models;

namespace NewsWebApi.IRepository
{
    public interface ISectionsRepository
    {
        public Task<IEnumerable<SectionModel>> GetAllAsync();
        public Task<IEnumerable<SectionModel>> GetAllDecAsync();
        public Task<SectionModel> GetAsync(int id);
        public Task AddAsync(SectionModel sectionModel);


    }
}
