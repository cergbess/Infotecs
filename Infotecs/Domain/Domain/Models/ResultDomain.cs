using Infotecs.DAL.Models;
namespace Infotecs.Domain.Models
{
    public class ResultDomain
    {
        public string filename { get; set; }

        public DateTime mindatetime { get; set; }
        public double avgtimeinseconds { get; set; }
        public double avgindicatorvalue { get; set; }
        public double medianindicatorvalue { get; set; }
        public double maxindicatorvalue { get; set; }
        public double minindicatorvalue { get; set; }
        public int totalrows { get; set; }
        public TimeSpan alltime { get; set; }

        public Result CalculateResults(List<Value> valuesList)
        {
            var result = new Result()
            {
                filename = valuesList[0].filename,
                mindatetime = valuesList.Min(v => v.datetime),
                avgtimeinseconds = valuesList.Average(v => v.timeinseconds),
                avgindicatorvalue = valuesList.Average(v => v.indicatorvalue),
                medianindicatorvalue = CalculateMedian(valuesList.Select(v => v.indicatorvalue)),
                maxindicatorvalue = valuesList.Max(v => v.indicatorvalue),
                minindicatorvalue = valuesList.Min(v => v.indicatorvalue),
                totalrows = valuesList.Count(),
                alltime = valuesList.Max(v => v.datetime) - valuesList.Min(v => v.datetime)
            };

            return result;
        }

        private double CalculateMedian(IEnumerable<double> values)
        {
            var sortedValues = values.OrderBy(v => v).ToList();
            var count = sortedValues.Count;

            if (count % 2 == 0)
            {
                var middle1 = sortedValues[count / 2 - 1];
                var middle2 = sortedValues[count / 2];
                return (middle1 + middle2) / 2;
            }
            else
            {
                return sortedValues[count / 2];
            }
        }
    }  
}
