using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamGo.Shared.Abstracts.DataProviding;

namespace TeamGo.Shared.DataProviding
{
    public sealed class DBContextRepository<TContext> : IDataRepositoryAsync<TContext>
        where TContext : DbContext, IDataProvider
    {
        private bool _isDisposed;

        public Func<TContext> ProviderConstructor { get; private set; }

        public DBContextRepository(Func<TContext> providerConstructor)
        {
            _isDisposed = false;
            ProviderConstructor = providerConstructor ?? throw new ArgumentNullException();
        }

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

        public async Task<Guid> CreateEntityAsync<TEntity>(Action<TEntity> entityOptions)
            where TEntity : class, IDataEntity, new()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DBContextRepository<TContext>));
            }
            TEntity entity = Activator.CreateInstance<TEntity>();
            entityOptions.Invoke(entity);

            return await CreateEntityAsync(entity);
        }

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

        public async Task UpdateEntity<TEntity>(Guid id, Action<TEntity> updateAction)
            where TEntity : class, IDataEntity
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DBContextRepository<TContext>));
            }
            using (TContext context = ProviderConstructor.Invoke())
            {
                await UpdateEntity(id, updateAction, context);
            }
        }

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

        public void Dispose()
        {
            if (_isDisposed)
            {
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                ProviderConstructor = null;
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
            return await Task.Run(() => context.Set<TEntity>().Where(predicate));
        }

        private async Task<TEntity> ReadEntityAsync<TEntity>(Guid id, TContext context)
            where TEntity : class, IDataEntity
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        private async Task UpdateEntity<TEntity>(Guid id, Action<TEntity> updateAction, TContext context)
            where TEntity : class, IDataEntity
        {
            TEntity entity = await ReadEntityAsync<TEntity>(id);
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
