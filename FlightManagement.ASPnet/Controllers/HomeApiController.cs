using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.ASPnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeApiController : ControllerBase
    {
        private readonly ILogger<HomeApiController> _logger;

        public HomeApiController(ILogger<HomeApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet("index")]
        public IActionResult GetIndex()
        {
            // Вернуть данные для главной страницы
            var data = new
            {
                Message = "Welcome to the Flight Management System",
                Description = "This is the home page of the Flight Management API."
            };

            return Ok(data);
        }

        [HttpGet("home")]
        public IActionResult GetHome()
        {
            // Вернуть данные для домашней страницы
            var data = new
            {
                Message = "Home Page",
                Description = "Here you can find information about flights."
            };

            return Ok(data);
        }

        [HttpGet("tables")]
        public IActionResult GetTables()
        {
            // Вернуть данные для таблиц
            var tablesData = new List<string>
            {
                "Flight Table",
                "Airport Table",
                "Schedule Table"
            };

            return Ok(tablesData);
        }

        [HttpGet("privacy")]
        public IActionResult GetPrivacy()
        {
            // Вернуть данные о конфиденциальности
            var privacyData = new
            {
                Message = "Privacy Policy",
                Description = "Your privacy is important to us. Please read our privacy policy."
            };

            return Ok(privacyData);
        }

        [HttpGet("error")]
        public IActionResult GetError()
        {
            // Пример возвращения сообщения об ошибке
            var errorData = new
            {
                RequestId = HttpContext.TraceIdentifier,
                Message = "An error occurred while processing your request."
            };

            return StatusCode(500, errorData); // 500 Internal Server Error
        }
    }
}
