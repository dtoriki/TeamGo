using System;
using System.Threading.Tasks;

namespace TeamGo.Shared.Abstracts.DataProviding
{
    /// <summary>
    /// Представляет поставщика данных.
    /// </summary>
    public interface IDataProvider : IDisposable
    {
        Task SaveAsync();
        void Save();
    }
}
