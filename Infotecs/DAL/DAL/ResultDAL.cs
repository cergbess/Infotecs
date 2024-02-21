using Infotecs.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infotecs.DAL
{
    public class ResultDAL
    {
        private readonly AppDbContext _dbContext;

        public ResultDAL(DbContextOptions<AppDbContext> dbContext)
        {
            _dbContext = new AppDbContext(dbContext);
        }

        public async Task SaveResultsToDatabase(Result result)
        {
            string sql = "INSERT INTO results (filename, mindatetime, avgtimeinseconds, avgindicatorvalue, medianindicatorvalue, maxindicatorvalue, minindicatorvalue, totalrows, alltime) " +
                            "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})";
            await _dbContext.Database.ExecuteSqlRawAsync(sql, result.filename, result.mindatetime, result.avgtimeinseconds,
                result.avgindicatorvalue, result.medianindicatorvalue, result.maxindicatorvalue,
                result.minindicatorvalue, result.totalrows, result.alltime);
        }

        public List<Result> GetResults(
        string? fileName = null,
        DateTime? minDateTime = null,
        DateTime? maxDateTime = null,
        double? minAvgIndicatorValue = null,
        double? maxAvgIndicatorValue = null,
        double? minAvgTimeInSeconds = null,
        double? maxAvgTimeInSeconds = null)
        {
            var query = _dbContext.results.AsQueryable();

            query = ApplyFilter(query, r => r.filename == fileName, !string.IsNullOrEmpty(fileName));
            query = ApplyFilter(query, r => r.mindatetime >= minDateTime, minDateTime != null);
            query = ApplyFilter(query, r => r.mindatetime <= maxDateTime, maxDateTime != null);
            query = ApplyFilter(query, r => r.avgindicatorvalue >= minAvgIndicatorValue, minAvgIndicatorValue != null);
            query = ApplyFilter(query, r => r.avgindicatorvalue <= maxAvgIndicatorValue, maxAvgIndicatorValue != null);
            query = ApplyFilter(query, r => r.avgtimeinseconds >= minAvgTimeInSeconds, minAvgTimeInSeconds != null);
            query = ApplyFilter(query, r => r.avgtimeinseconds <= maxAvgTimeInSeconds, maxAvgTimeInSeconds != null);

            return query.ToList();
        }

        private IQueryable<Result> ApplyFilter(IQueryable<Result> query, Expression<Func<Result, bool>> predicate, bool condition)
        {
            return condition ? query.Where(predicate) : query;
        }
        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteExistingRecords(string fileName)
        {
            var existingRecords = await _dbContext.results
                .Where(v => v.filename == fileName)
                .ToListAsync();
            _dbContext.results.RemoveRange(existingRecords);

            await _dbContext.SaveChangesAsync();
        }
    }
}
