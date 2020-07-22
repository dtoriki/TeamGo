using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamGo.Shared.Abstracts.DataProviding
{
    /// <summary>
    /// Представляет собой репозиторий поставщика данных.
    /// </summary>
    /// <typeparam name="TDataProvider">Тип поставщика данных <see cref="IDataProvider"/></typeparam>
    public interface IDataRepositoryAsync<TDataProvider> : IDisposable
        where TDataProvider : IDataProvider
    {
        /// <summary>
        /// Конструктор поставщика данных
        /// </summary>
        Func<TDataProvider> ProviderConstructor { get; }

        /// <summary>
        /// Добавляет сущность в структуру данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="IDataEntity"/></typeparam>
        /// <param name="entity">Сущность для добавления</param>
        /// <returns>Присвоенный уникальный идентификатор</returns>
        Task<Guid> CreateEntityAsync<TEntity>(TEntity entity)
            where TEntity : class, IDataEntity;
        /// <summary>
        /// Добавляет сущность в структуру данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="IDataEntity{TId}"/></typeparam>
        /// <param name="entityOptions">Настройки добавляемой сущности</param>
        /// <returns>Присвоенный уникальный идентификатор</returns>
        Task<Guid> CreateEntityAsync<TEntity>(Action<TEntity> entityOptions)
            where TEntity : class, IDataEntity, new();
        /// <summary>
        /// Читает сущность из структуры данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="IDataEntity{TId}"/></typeparam>
        /// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        /// <returns>Сущность</returns>
        Task<TEntity> ReadEntityAsync<TEntity>(Guid id)
            where TEntity : class, IDataEntity;
        /// <summary>
        /// Читает множество сущностей из структуры данных.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="IDataEntity"/></typeparam>
        /// <param name="predicate">Условие выбора сущностей</param>
        /// <returns>Сущности</returns>
        Task<IEnumerable<TEntity>> ReadEntitiesAsync<TEntity>(Func<TEntity, bool> predicate)
            where TEntity : class, IDataEntity;
        /// <summary>
        /// Читает множество сущностей из структуры данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="IDataEntity"/></typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        /// <param name="updateAction">Действия обновления</param>
        Task UpdateEntity<TEntity>(Guid id, Action<TEntity> updateAction)
            where TEntity : class, IDataEntity;
        /// <summary>
        /// Читает множество сущностей из структуры данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="IDataEntity"/></typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        /// <param name="newEntity">Сущность на замену</param>
        Task UpdateEntity<TEntity>(Guid id, TEntity newEntity)
            where TEntity : class, IDataEntity;
        /// <summary>
        /// Удаляет сущность из структуры данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="IDataEntity"/></typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        Task DeleteEntity<TEntity>(Guid id)
             where TEntity : class, IDataEntity;
    }
}
