using DevLab.JmesPath;
using Microsoft.AspNetCore.Mvc;

namespace Homework2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public ShopController(IWebHostEnvironment env) {
            _env = env;
        }
        private string ReadJsonFile()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "Data", "shop.json");
            if(!System.IO.File.Exists(filePath))
            {
                throw new Exception("NO find the file shop.json");
            }
            return System.IO.File.ReadAllText(filePath);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var json = ReadJsonFile();
            return Content(json, "application/json");
        }
        [HttpGet("query")]
        public IActionResult Query([FromQuery] string expression)
        {
            if(string.IsNullOrWhiteSpace(expression))
            {
                return BadRequest(new { mess = "Expression is not white space" });
            }
            try
            {
                var json = ReadJsonFile();
                var jmes = new JmesPath();
                var result = jmes.Transform(json, expression);
                return Content(result, "application/json");
            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    mess = "Expression is not valid " , error = ex.Message
                });
            }
        }
    }
}
