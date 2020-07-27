using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamGo.Identity.Server.Engine.DbEntites;
using TeamGo.Shared.Abstracts.DataProviding;

namespace TeamGo.Identity.Server.Engine.Contexts
{
    internal class UserStoreDbContext : DbContext, IDataProvider
    {
#pragma warning disable CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles  { get; set; }
        public UserStoreDbContext(DbContextOptions<UserStoreDbContext> options) : base (options)
#pragma warning restore CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.
        {

        }

        public void Save()
        {
            SaveChanges();
        }

        public async Task SaveAsync()
        {
            await SaveChangesAsync();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder
                .Entity<RoleEntity>()
                .HasOne(x => x.User)
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                .WithMany(x => x.Roles)
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
