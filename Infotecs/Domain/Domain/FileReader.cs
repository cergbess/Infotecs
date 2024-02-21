using Infotecs.DAL.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Domain.Domain
{
    public class FileReader
    {
        public async Task ProcessFileContent(StreamReader reader, IFormFile file, List<Value> valuesList, List<string> errorLines)
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                ProcessLine(line, file, valuesList, errorLines);
            }
        }

        private void ProcessLine(string line, IFormFile file, List<Value> valuesList, List<string> errorLines)
        {
            var values = line.Split(';');
            if (values.Length == 3)
            {
                var fileName = file.FileName;
                DateTime dateTime = ParseDateTime(values[0]);
                var timeInSeconds = int.Parse(values[1]);
                var indicatorValue = double.Parse(values[2].Substring(0, values[2].Length - 1));

                if (!IsValidData(dateTime, timeInSeconds, indicatorValue))
                {
                    errorLines.Add(line);
                    return;
                }
                var value = new Value
                {
                    datetime = dateTime,
                    indicatorvalue = indicatorValue,
                    timeinseconds = timeInSeconds,
                    filename = fileName
                };
                valuesList.Add(value);
            }
            else
            {
                errorLines.Add(line);
            }
        }

        private DateTime ParseDateTime(string dateTimeString)
        {
            string format = "yyyy-MM-dd_HH-mm-ss";
            return DateTime.ParseExact(dateTimeString.Substring(1), format, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        private bool IsValidData(DateTime dateTime, int timeInSeconds, double indicatorValue)
        {
            return !(dateTime < DateTime.Parse("2000-01-01") || dateTime > DateTime.Now ||
                timeInSeconds < 0 || indicatorValue < 0);
        }
    }
}
