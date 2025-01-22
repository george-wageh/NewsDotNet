namespace NewsWebApi.Data
{
    public class AppUnitWork
    {
        public AppUnitWork(AppDbContext appDbContext) {
            AppDbContext = appDbContext;
        }

        public AppDbContext AppDbContext { get; }

        public async Task SaveChangesAsync() { 
            await AppDbContext.SaveChangesAsync();
        }

    }
}
