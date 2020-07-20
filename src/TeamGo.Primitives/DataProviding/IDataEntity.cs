namespace TeamGo.Primitives.DataProviding
{
    /// <summary>
    /// Представляет сущность данных.
    /// </summary>
    /// <typeparam name="TId">Тип уникального идентификатора</typeparam>
    public interface IDataEntity<TId> 
    {
        public TId Id { get; }
    }
}
