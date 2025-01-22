using Microsoft.EntityFrameworkCore;
using NewsWebApi.Models;

namespace NewsWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Top15NewsModel>().HasOne(x => x.NewModel)
                .WithOne().HasForeignKey<Top15NewsModel>(x => x.NewModelId);

            modelBuilder.Entity<NewModel>().HasOne(x => x.SectionModel)
                .WithMany(x => x.News).HasForeignKey(x => x.SectionId);

            modelBuilder.Entity<AdminModel>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<AdminModel>().HasMany(x => x.UserRoles).WithOne(x => x.Admin);
            modelBuilder.Entity<RoleModel>().HasMany(x => x.UserRoles).WithOne(x => x.Role);

            modelBuilder.Entity<UserRoleModel>().HasKey(x => new { x.AdminId, x.RoleId });

            modelBuilder.Entity<UserRoleModel>()
                .HasOne(x => x.Admin)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.AdminId);

            modelBuilder.Entity<UserRoleModel>()
                .HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<RoleModel>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<TempPasswordModel>().HasIndex(x => x.Email).IsUnique();


        }
    }
}
