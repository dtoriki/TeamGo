using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamGo.Primitives.DataContext
{
    /// <summary>
    /// Представляет собой репозиторий поставщика данных.
    /// </summary>
    /// <typeparam name="TDataProvider">Тип поставщика данных <see cref="IDataProvider"/></typeparam>
    public interface IDataRepositoryAsync<TDataProvider>
        where TDataProvider : IDataProvider
    {
        /// <summary>
        /// Конструктор поставщика данных
        /// </summary>
        Func<TDataProvider> ProviderConstructor { get; }

        /// <summary>
        /// Добавляет сущность в структуру данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="ITableEntity{TId}"/></typeparam>
        /// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
        /// <param name="entity">Сущность для добавления</param>
        /// <returns>Присвоенный уникальный идентификатор</returns>
        Task<TId> CreateEntityAsync<TEntity, TId>(TEntity entity)
            where TEntity : class, ITableEntity<TId>;
        /// <summary>
        /// Добавляет сущность в структуру данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="ITableEntity{TId}"/></typeparam>
        /// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
        /// <param name="entityOptions">Настройки добавляемой сущности</param>
        /// <returns>Присвоенный уникальный идентификатор</returns>
        Task<TId> CreateEntityAsync<TEntity, TId>(Action<TEntity> entityOptions)
            where TEntity : class, ITableEntity<TId>, new();
        /// <summary>
        /// Читает сущность из структуры данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="ITableEntity{TId}"/></typeparam>
        /// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        /// <returns>Сущность</returns>
        Task<TEntity> ReadEntityAsync<TEntity, TId>(TId id)
            where TEntity : class, ITableEntity<TId>;
        /// <summary>
        /// Читает множество сущностей из структуры данных.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="ITableEntity{TId}"/></typeparam>
        /// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
        /// <param name="predicate">Условие выбора сущностей</param>
        /// <returns>Сущности</returns>
        Task<IEnumerable<TEntity>> ReadEntitiesAsync<TEntity, TId>(Func<TEntity, bool> predicate)
            where TEntity : class, ITableEntity<TId>;
        /// <summary>
        /// Читает множество сущностей из структуры данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="ITableEntity{TId}"/></typeparam>
        /// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        /// <param name="updateAction">Действия обновления</param>
        Task UpdateEntity<TEntity, TId>(TId id, Action<TEntity> updateAction)
            where TEntity : class, ITableEntity<TId>;
        /// <summary>
        /// Читает множество сущностей из структуры данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности <see cref="ITableEntity{TId}"/></typeparam>
        /// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        /// <param name="newEntity">Сущность на замену</param>
        Task UpdateEntity<TEntity, TId>(TId id, TEntity newEntity)
            where TEntity : class, ITableEntity<TId>;
        /// <summary>
        /// Удаляет сущность из структуры данных
        /// </summary>
        /// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        Task DeleteEntity<TId>(TId id);
    }
}
