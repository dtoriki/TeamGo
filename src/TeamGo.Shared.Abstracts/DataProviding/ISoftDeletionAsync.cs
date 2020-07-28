using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamGo.Shared.Abstracts.DataProviding
{
    /// <summary>
    /// Предоставляеть интерфейс для мягкого удаления сущности
    /// </summary>
    public interface ISoftDeletionAsync
    {
        /// <summary>
        /// Мягкое удалене сущности.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности. Сущность должна реализовывать <see cref="IDataEntity"/> и <see cref="ISoftDeleteable"/></typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        Task SoftDeleteAsync<TEntity>(Guid id)
            where TEntity : class, IDataEntity, ISoftDeleteable;
        /// <summary>
        /// Восстановление удалёной сущности.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности. Сущность должна реализовывать <see cref="IDataEntity"/> и <see cref="ISoftDeleteable"/></typeparam>
        /// <param name="id">Уникальный идентификатор сущности</param>
        Task RepareAsync<TEntity>(Guid id)
            where TEntity : class, IDataEntity, ISoftDeleteable;
        /// <summary>
        /// Читает множество сущностей из структуры данных. Поддерживает опцию чтения удалёных сущностей.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности. Сущность должна реализовывать <see cref="IDataEntity"/> и <see cref="ISoftDeleteable"/></typeparam>
        /// <param name="predicate">Условие выбора сущностей</param>
        /// <param name="isDeleted">Если true читает только удалёные сущности, если false - не удалёные</param>
        Task<IEnumerable<TEntity>> ReadSoftEntitiesAsync<TEntity>(Func<TEntity, bool> predicate, bool isDeleted = false)
            where TEntity : class, IDataEntity, ISoftDeleteable;
    }
}
