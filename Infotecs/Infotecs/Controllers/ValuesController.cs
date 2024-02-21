using Domain.Domain;
using Infotecs.DAL;
using Infotecs.DAL.Models;
using Infotecs.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly ResultDAL _resultDAL;
        private readonly ValuesDAL _valuesDAL;
        private readonly FileReader _fileReader;
        private readonly ResultDomain _result;
        public ValuesController(DbContextOptions<AppDbContext> db)
        {
            _resultDAL = new ResultDAL(db);
            _valuesDAL = new ValuesDAL(db);
            _fileReader = new FileReader();
            _result = new ResultDomain();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");

                var valuesList = new List<Value>();
                var errorLines = new List<string>();

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    await _fileReader.ProcessFileContent(reader, file, valuesList, errorLines);
                }

                var rowCount = valuesList.Count;
                if (rowCount < 1 || rowCount > 10000)
                {
                    return BadRequest(rowCount);
                }
                var isFileNameExists = await _valuesDAL.IsFileNameExists(file.FileName);
                if (isFileNameExists)
                {
                    await _resultDAL.DeleteExistingRecords(file.FileName);
                }

                var results = _result.CalculateResults(valuesList);

                await _resultDAL.SaveResultsToDatabase(results);
                await _valuesDAL.SaveValuesToDatabase(valuesList);

                await _resultDAL.Save();

                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetValuesByFileName(string fileName)
        {
           var values = await _valuesDAL.GetValuesByFileName(fileName);
            if (values.Any())
            {
                return Ok(values);
            }
            else
            {
                return NotFound($"No values found for the file name: {fileName}");
            }
        }
    }
}
