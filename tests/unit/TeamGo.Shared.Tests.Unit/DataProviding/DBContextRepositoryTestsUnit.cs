using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamGo.Shared.DataProviding;
using TeamGo.Shared.Tests.Unit.TestClasses;
using Xunit;

namespace TeamGo.Shared.Tests.Unit.DataProviding
{
    public class DBContextRepositoryTestsUnit
    {
        private readonly DbContextOptions<TestDbContext> _dynamicOptions;

        public DBContextRepositoryTestsUnit()
        {
            string rnd = "db_" + Guid
                .NewGuid()
                .ToString();

            _dynamicOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(rnd)
                .Options;
        }

        [Fact]
        public async void Preseted_Entity_With_Autogenerated_Guid_Correctly_Added_Into_Database()
        {
            TestEntity testEntity = new TestEntity()
            {
                IntData = 10,
                StringData = "10"
            };

            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            using(DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
            {
                Guid actual = await repository.CreateEntityAsync(testEntity);
                Assert.NotEqual(default, actual);
            }
        }

        [Fact]
        public async void Preseted_Entity_With_Pre_Generated_Guid_Correctly_Added_Into_Database()
        {
            Guid presetedGuid = Guid.NewGuid();
            TestEntity testEntity = new TestEntity()
            {
                Id = presetedGuid,
                IntData = 10,
                StringData = "10"
            };

            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
            {
                Guid actual = await repository.CreateEntityAsync(testEntity);
                Assert.Equal(presetedGuid, actual);
            }
        }

        [Fact]
        public async void Entity_With_Autogenerated_Guid_Correctly_Added_With_Action_Into_Database()
        {
            Action<TestEntity> entityOptions = x =>
            {
                x.IntData = 12;
                x.StringData = "13";
            };

            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
            {
                Guid actual = await repository.CreateEntityAsync(entityOptions);
                Assert.NotEqual(default, actual);
            }
        }

        [Fact]
        public async void Try_Get_Entity_From_Database_by_Id()
        {
            int iterations = 10;
            List<Guid> generatedGuids = new List<Guid>();
            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            Random rnd = new Random();
            using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
            {
                for (int i = 0; i < iterations; i++)
                {
                    generatedGuids.Add(await repository.CreateEntityAsync(new TestEntity()
                    {
                        IntData = rnd.Next(100),
                        StringData = rnd.Next(100).ToString()
                    }));
                }

                Guid firstGuid = generatedGuids.FirstOrDefault();
                Guid lastGuid = generatedGuids.LastOrDefault();

                TestEntity entity1 = await repository.ReadEntityAsync<TestEntity>(firstGuid);
                TestEntity entity2 = await repository.ReadEntityAsync<TestEntity>(lastGuid);

                bool actual = entity1 != null
                    && entity2 != null
                    && entity1.Id.Equals(firstGuid)
                    && entity2.Id.Equals(lastGuid);

                Assert.True(actual);
            }
        }

        [Fact]
        public async void Try_Get_Null_Entity_From_Database()
        {
            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
            {
                TestEntity actual = await repository.ReadEntityAsync<TestEntity>(Guid.NewGuid());
                Assert.Null(actual);
            }
        }

        [Fact]
        public async void Try_Get_Multiply_Entities_By_Predicate()
        {
            Random rnd = new Random();
            int iterationsForInt = rnd.Next(1, 11);
            int iterationsForString = rnd.Next(1, 11);

            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
            {
                int intFieldData = rnd.Next(100);
                for (int i = 0; i < iterationsForInt; i++)
                {
                    await repository.CreateEntityAsync(new TestEntity()
                    {
                        IntData = intFieldData,
                        StringData = rnd.Next(100).ToString()
                    });
                }

                string stringFieldData = rnd.Next(100).ToString();
                for (int i = 0; i < iterationsForString; i++)
                {
                    await repository.CreateEntityAsync(new TestEntity() {
                        IntData = rnd.Next(100),
                        StringData = stringFieldData
                    });
                }

                IEnumerable<TestEntity> given1 = await repository.ReadEntitiesAsync<TestEntity>(x => x.IntData == intFieldData);
                IEnumerable<TestEntity> given2 = await repository.ReadEntitiesAsync<TestEntity>(x => x.StringData == stringFieldData);

                bool actual = given1.Count() == iterationsForInt
                    && given2.Count() == iterationsForString;

                Assert.True(actual);
            }
        }

        [Fact]
        public async void Try_Update_Entity()
        {
            Random rnd = new Random();
            int intData = rnd.Next(10);
            TestEntity testEntity = new TestEntity()
            {
                IntData = intData,
                StringData = "10"
            };

            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
            {
                Guid id = await repository.CreateEntityAsync(testEntity);

                int newIntData = rnd.Next(20, 50);
                await repository.UpdateEntity<TestEntity>(id, x => x.IntData = newIntData);

                int actual = (await repository.ReadEntityAsync<TestEntity>(id)).IntData;

                Assert.Equal(newIntData, actual);
            }
        }

        [Fact]
        public async void Try_Remove_Entity()
        {
            TestEntity testEntity = new TestEntity()
            {
                IntData = 10,
                StringData = "10"
            };

            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
            {
                Guid id = await repository.CreateEntityAsync(testEntity);

                bool wasCreated = (await repository.ReadEntityAsync<TestEntity>(id)) != null;

                await repository.DeleteEntityAsync<TestEntity>(id);

                bool wasDeleted = (await repository.ReadEntityAsync<TestEntity>(id)) == null;

                bool actual = wasCreated && wasDeleted;

                Assert.True(actual);
            }
        }

        [Fact]
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
        public void Ctor_Throws_Argument_Null_Exception() => _ = Assert.Throws<ArgumentNullException>(() => new DBContextRepository<TestDbContext>(null));
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.

        [Fact]
        public async void CreateEntityAsync_With_Preseted_Entity_Throws_Object_Disposed_Exception_WhenRepository_Disposed()
        {
            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            _ = await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
            {
                DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor);
                repository.Dispose();

                _ = await repository.CreateEntityAsync(new TestEntity());
            });
        }

        [Fact]
        public async void CreateEntityAsync_With_Preseted_Entity_Throws_Argument_Null_Exception()
        {
            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            _ = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
                {

#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                    _ = await repository.CreateEntityAsync<TestEntity>(entity: null);
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                }

            });
        }

        [Fact]
        public async void CreateEntityAsync_With_Optional_Entity_Throws_Argument_Null_Exception()
        {
            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            _ = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
                {

#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                    _ = await repository.CreateEntityAsync<TestEntity>(entityOptions: null);
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                }

            });
        }

        [Fact]
        public async void ReadEntitiesAsync_Throws_Argument_Null_Exception()
        {
            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            _ = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
                {

#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                    _ = await repository.ReadEntitiesAsync<TestEntity>(predicate: null);
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                }

            });
        }

        [Fact]
        public async void UpdateEntity_Throws_Argument_Null_Exception()
        {
            Func<TestDbContext> contextConstrucor = () => new TestDbContext(_dynamicOptions);
            _ = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                using (DBContextRepository<TestDbContext> repository = new DBContextRepository<TestDbContext>(contextConstrucor))
                {

#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                    await repository.UpdateEntity<TestEntity>(Guid.NewGuid(), null);
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                }

            });
        }
    }
}