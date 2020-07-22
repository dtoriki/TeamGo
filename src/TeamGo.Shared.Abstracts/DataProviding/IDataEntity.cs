using System;

namespace TeamGo.Shared.Abstracts.DataProviding
{
    /// <summary>
    /// Представляет сущность данных.
    /// </summary>
    public interface IDataEntity
    {
        public Guid Id { get; }
    }
}
