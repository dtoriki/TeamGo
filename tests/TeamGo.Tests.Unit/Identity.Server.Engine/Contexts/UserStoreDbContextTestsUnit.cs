using Microsoft.EntityFrameworkCore;
using TeamGo.Identity.Server.Engine.Contexts;
using TeamGo.Identity.Server.Engine.DbEntites;
using TeamGo.Tests.Engine;
using Xunit;

namespace TeamGo.Tests.Unit.Identity.Server.Engine.Contexts
{
    public class UserStoreDbContextTestsUnit
    {
        private readonly DbContextOptions<UserStoreDbContext> _dynamicOptions;
        public UserStoreDbContextTestsUnit()
        {
            _dynamicOptions = InMemoryDbOptions.GetDBInMemoryOptions<UserStoreDbContext>();
        }
        [Fact(Timeout = 1000)]
        public void UserStoreDbContext_As_IDataProvider_Correctly_Saving_Data()
        {
            using (UserStoreDbContext context = new UserStoreDbContext(_dynamicOptions))
            {
                UserEntity userEntity = new UserEntity();

                context.Users.Add(userEntity);
                context.Save();

                UserEntity given = context.Find<UserEntity>(userEntity.Id);
                Assert.NotNull(given);
            }

        }

        [Fact(Timeout = 1000)]
        public async void UserStoreDbContext_As_IDataProvider_Correctly_Saving_Data_Async()
        {
            using (UserStoreDbContext context = new UserStoreDbContext(_dynamicOptions))
            {
                UserEntity userEntity = new UserEntity();

                context.Users.Add(userEntity);
                await context.SaveAsync();

                UserEntity given = context.Find<UserEntity>(userEntity.Id);
                Assert.NotNull(given);
            }

        }
    }
}
