using Microsoft.AspNetCore.Mvc;
namespace ChatBotInpadserver.Controllers
{
    [ApiController]
    [Route("api/web")]
    public class WebClientController : ControllerBase
    {
        // private readonly DbContext db;

        [HttpGet("test")]
        public IActionResult TestMethod()
        {
            return Ok("Работает");
        }
        /*
        [HttpPost("questions/add")]
        //public async IActionResult AddQuestion([FromBody] string text)
       // {
        //    try
        //    {
         //       var question = new Question(text);
        //        await db.Questions.Add(question);
         //       return Ok(question);
        //    }
         //   catch (Exception ex)
        //    {
         //       return BadRequest(ex.Message);
          //  }
    //    }
   // }
        */
    }
}
