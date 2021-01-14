using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportsApp.Persistence
{
    public interface ISQLiteOfflineStore<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(long id);
        Task UpdateAsync(T entity);
        Task InsertAsync(T entity);
        Task DeleteAsync(T photo);
    }
}
