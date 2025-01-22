using Microsoft.EntityFrameworkCore;
using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;

namespace NewsWebApi.Repository
{
    public class TempPasswordRepository : ITempPasswordRepository
    {
        public TempPasswordRepository(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public AppDbContext AppDbContext { get; }

        public async Task AddPassword(string email, string passoword)
        {
            var tempPassword = await AppDbContext.Set<TempPasswordModel>().FirstOrDefaultAsync(x => x.Email == email);
            if (tempPassword == null)
            {
                tempPassword = new TempPasswordModel
                {
                    Email = email,
                    Date = DateTime.Now,
                    Password = passoword
                };
                await AppDbContext.Set<TempPasswordModel>().AddAsync(tempPassword);
            }
            else
            {
                tempPassword.Date = DateTime.Now;
                tempPassword.Password = passoword;
            }
            await AppDbContext.SaveChangesAsync();
        }

        public async Task<TempPasswordModel> GetPassword(string email) {
            return await AppDbContext.Set<TempPasswordModel>().FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
