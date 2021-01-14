using ReportsApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportsApp.Persistence
{
    public class PhotosOStore : ISQLiteOfflineStore<PhotoRecord>
    {
        private readonly SQLiteAsyncConnection _db;

        public PhotosOStore(ISQLiteDb db)
        {
            _db = db.GetConnection();
            _db.CreateTableAsync<PhotoRecord>();
        }
        public async Task DeleteAsync(PhotoRecord photo)
        {
            await _db.DeleteAsync(photo);
        }

        public async Task<IEnumerable<PhotoRecord>> GetAllAsync()
        {
            return await _db.Table<PhotoRecord>().ToListAsync();
        }

        public async Task<PhotoRecord> GetAsync(long id)
        {
            return await _db.GetAsync<PhotoRecord>(id);
        }

        public async Task InsertAsync(PhotoRecord entity)
        {
            await _db.InsertAsync(entity);
        }

        public async Task UpdateAsync(PhotoRecord entity)
        {
            await _db.UpdateAsync(entity);
        }
    }
}
