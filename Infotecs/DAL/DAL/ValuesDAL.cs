using Infotecs.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.DAL
{
    public class ValuesDAL
    {
        private readonly AppDbContext _dbContext;

        public ValuesDAL(DbContextOptions<AppDbContext> dbContext)
        {
            _dbContext = new AppDbContext(dbContext);
        }
        public async Task SaveValuesToDatabase(List<Value> valuesList)
        {
            foreach (var value in valuesList)
            {
                string sql = "INSERT INTO values (datetime, timeinseconds, indicatorvalue, filename) " +
                            "VALUES ({0}, {1}, {2}, {3})";
                await _dbContext.Database.ExecuteSqlRawAsync(sql, value.datetime, value.timeinseconds, value.indicatorvalue, value.filename);
            }
        }

        public async Task<bool> IsFileNameExists(string fileName)
        {
            return await _dbContext.values.AnyAsync(v => v.filename == fileName);
        }

        public async Task<List<Value>> GetValuesByFileName(string fileName)
        {
            return await _dbContext.values.AsNoTracking().Where(v => v.filename == fileName).ToListAsync();
        }
    }
}
