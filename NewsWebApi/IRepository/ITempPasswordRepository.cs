using NewsWebApi.Models;

namespace NewsWebApi.IRepository
{
    public interface ITempPasswordRepository
    {
        public Task AddPassword(string email, string passoword);
        public Task<TempPasswordModel> GetPassword(string email);

    }
}
