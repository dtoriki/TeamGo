using System;

namespace TeamGo.Shared.Abstracts.DataProviding
{
    /// <summary>
    /// Представляет сущность данных.
    /// </summary>
    public interface IDataEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public Guid Id { get; }
    }
}
