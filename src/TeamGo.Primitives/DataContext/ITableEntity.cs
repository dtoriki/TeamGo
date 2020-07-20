namespace TeamGo.Primitives.DataContext
{
    /// <summary>
    /// Представляет сущность данных.
    /// </summary>
    /// <typeparam name="TId">Тип уникального идентификатора</typeparam>
    public interface ITableEntity<TId> 
    {
        public object Id { get; }
    }
}
