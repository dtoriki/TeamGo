using System;
using System.Threading.Tasks;

namespace TeamGo.Shared.Abstracts.DataProviding
{
    /// <summary>
    /// Представляет поставщика данных.
    /// </summary>
    public interface IDataProvider : IDisposable
    {
        /// <summary>
        /// Ассинхронно сохраняет данные
        /// </summary>
        Task SaveAsync();
        /// <summary>
        /// Сохраняет данные
        /// </summary>
        void Save();
    }
}
