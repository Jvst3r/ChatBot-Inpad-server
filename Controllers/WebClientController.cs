using ChatBot_Inpad_server.Services;
using ChatBotInpadserver.Data.DataBase;
using ChatBotInpadServer.Services;
using Microsoft.AspNetCore.Mvc;
namespace ChatBotInpadserver.Controllers
{
    [ApiController]
    [Route("api/web")]
    public class WebClientController : ControllerBase
    {
        private readonly AdminService adminService;
        private readonly KnowledgeService knowledgeService;

        public WebClientController(AdminService _adminService, KnowledgeService _knowledgeService)
        {
            adminService = _adminService;
            knowledgeService = _knowledgeService;
        }

        [HttpGet("test")]
        public IActionResult TestMethod()
        {
            return Ok("Работает");
        }

        [HttpPost("login")]        
        public async Task<IActionResult> LoginAdmin(string Email, string Password)
        {
            var dto = await adminService.LoginAsync(Email, Password);

            if (dto.Success) 
                return Ok(dto);

            return Unauthorized(dto);
        }

    }
}
