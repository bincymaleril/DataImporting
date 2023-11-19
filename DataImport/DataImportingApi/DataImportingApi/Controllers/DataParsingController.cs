using DataImportingApi.Models;
using DataImportingApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataImportingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataParsingController:ControllerBase
    {
        private readonly DataParsingService dataParsingService;

        public DataParsingController(DataParsingService textParsingService)
        {
            dataParsingService = textParsingService;
        }

        [HttpPost]
        public IActionResult ParseText([FromBody] TextDataModel textDataModel)
        {
            try
            {
               var result = dataParsingService.ExtractAndCalculate(textDataModel.Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing the text: {ex.Message}");
            }
        }
    }
}
