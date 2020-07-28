using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamGo.Shared.Abstracts.DataProviding;

namespace TeamGo.Shared.DataProviding
{

    /// <summary>
    /// Предоставляет возможность управления данными для <see cref="DbContext" />.
    /// Является реализацией <see cref="IDisposable"/>. 
    /// </summary>
    /// <remarks>
    /// <see cref="DbContext"/> Должен реализовать <see cref="IDataProvider"/>
    /// </remarks>
    /// <typeparam name="TContext">
    /// Тип базы данных.
    /// Должен быть наследником <see cref="DbContext"/>,
    /// и реализовывать интерфейс <see cref="IDataProvider"/>
    /// </typeparam>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="ObjectDisposedException" />
    public sealed class DBContextRepository<TContext> : IDataRepositoryAsync<TContext>, ISoftDeletionAsync
        where TContext : DbContext, IDataProvider
    {
        private bool _isDisposed;
        private Func<TContext> _providerConstructor;

        /// <inheritdoc />
        /// <remarks>
        /// Результатом функции является наследник <see cref="DbContext"/>,
        /// реализующий интерфейс <see cref="IDataProvider"/>
        /// </remarks>
        public Func<TContext> ProviderConstructor
        {
            get
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException(nameof(DBContextRepository<TContext>));
                }
                return _providerConstructor;
            }
        }

        /// <summary>
        /// Создаёт экземпляр <see cref="DBContextRepository{TContext}"/>
        /// </summary>
        /// <param name="providerConstructor">Функция создания экземпляра <typeparamref name="TContext"/></param>     
        /// <exception cref="ArgumentNullException" />
        public DBContextRepository(Func<TContext> providerConstructor)
        {
            _isDisposed = false;
            _providerConstructor = providerConstructor ?? throw new ArgumentNullException();
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ObjectDisposedException" />
        public async Task<Guid> CreateEntityAsync<TEntity>(TEntity entity)
            where TEntity : class, IDataEntity
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DBContextRepository<TContext>));
            }
            using (TContext context = ProviderConstructor.Invoke())
            {
                return await CreateEntityAsync(entity, context);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// /// <exception cref="ObjectDisposedException" />
        public async Task<Guid> CreateEntityAsync<TEntity>(Action<TEntity> entityOptions)
            where TEntity : class, IDataEntity, new()
        {
            if (entityOptions == null)
            {
                throw new ArgumentNullException();
            }
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DBContextRepository<TContext>));
            }
            TEntity entity = Activator.CreateInstance<TEntity>();
            entityOptions.Invoke(entity);

            return await CreateEntityAsync(entity);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ObjectDisposedException" />
        public async Task<IEnumerable<TEntity>> ReadEntitiesAsync<TEntity>(Func<TEntity, bool> predicate)
            where TEntity : class, IDataEntity
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DBContextRepository<TContext>));
            }
            using (TContext context = ProviderConstructor.Invoke())
            {
                return await ReadEntitiesAsync(predicate, context);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ObjectDisposedException" />
        public async Task<TEntity> ReadEntityAsync<TEntity>(Guid id)
            where TEntity : class, IDataEntity
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DBContextRepository<TContext>));
            }
            using (TContext context = ProviderConstructor.Invoke())
            {
                return await ReadEntityAsync<TEntity>(id, context);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ObjectDisposedException" />
        public async Task UpdateEntity<TEntity>(Guid id, Action<TEntity> updateAction)
            where TEntity : class, IDataEntity
        {
            if (updateAction == null)
            {
                throw new ArgumentNullException();
            }
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DBContextRepository<TContext>));
            }
            using (TContext context = ProviderConstructor.Invoke())
            {
                await UpdateEntity(id, updateAction, context);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ObjectDisposedException" />
        public async Task DeleteEntityAsync<TEntity>(Guid id)
            where TEntity : class, IDataEntity
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DBContextRepository<TContext>));
            }
            using (TContext context = ProviderConstructor.Invoke())
            {
                await DeleteEntityAsync<TEntity>(id, context);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ObjectDisposedException" />
        public async Task SoftDeleteAsync<TEntity>(Guid id)
            where TEntity : class, IDataEntity, ISoftDeleteable
        {
            await UpdateEntity<TEntity>(id, x =>
            {
                x.IsSoftDeleted = true;
                x.DeleteTime = DateTime.UtcNow;
            });
        }
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ObjectDisposedException" />
        public async Task RepareAsync<TEntity>(Guid id)
            where TEntity : class, IDataEntity, ISoftDeleteable
        {
            await UpdateEntity<TEntity>(id, x =>
            {
                x.IsSoftDeleted = false;
                x.DeleteTime = null;
            });
        }
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ObjectDisposedException" />
        public async Task<IEnumerable<TEntity>> ReadSoftEntitiesAsync<TEntity>(Func<TEntity, bool> predicate, bool isDeleted = false)
            where TEntity : class, IDataEntity, ISoftDeleteable
        {
            return (await ReadEntitiesAsync(predicate))
                .Where(x => x.IsSoftDeleted == isDeleted)
                .ToList();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_isDisposed)
            {
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                _providerConstructor = null;
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                _isDisposed = true;
            }
        }

        private async Task<Guid> CreateEntityAsync<TEntity>(TEntity entity, TContext context)
            where TEntity : class, IDataEntity
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveAsync();

            return entity.Id;
        }

        private async Task<IEnumerable<TEntity>> ReadEntitiesAsync<TEntity>(Func<TEntity, bool> predicate, TContext context)
           where TEntity : class, IDataEntity
        {
            return await Task.Run(() => context.Set<TEntity>().Where(predicate).ToList());
        }

        private async Task<TEntity> ReadEntityAsync<TEntity>(Guid id, TContext context)
            where TEntity : class, IDataEntity
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        private async Task UpdateEntity<TEntity>(Guid id, Action<TEntity> updateAction, TContext context)
            where TEntity : class, IDataEntity
        {
            TEntity entity = await ReadEntityAsync<TEntity>(id, context);
            updateAction.Invoke(entity);
            await context.SaveAsync();
        }

        private async Task DeleteEntityAsync<TEntity>(Guid id, TContext context)
             where TEntity : class, IDataEntity
        {
            TEntity entity = await ReadEntityAsync<TEntity>(id, context);
            context.Set<TEntity>().Remove(entity);
            await context.SaveAsync();
        }

    }
}
