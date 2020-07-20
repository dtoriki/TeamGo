using System;

namespace TeamGo.Primitives.DataProviding
{
    /// <summary>
    /// Представляет сущность данных.
    /// </summary>
    public interface IDataEntity
    {
        public Guid Id { get; }
    }
}
