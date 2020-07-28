using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamGo.Shared.Abstracts.DataProviding;

namespace Test.Engine
{
    internal class TestDbContext : DbContext, IDataProvider
    {
        public DbSet<TestEntity> TestTable { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.
        public TestDbContext(DbContextOptions<TestDbContext> opt) : base (opt)
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
    }
}
