using Microsoft.AspNetCore.Mvc;
using Services;
using Telegram.Bot.Types;

[ApiController]
[Route("api/telegram")]
public class TelegramWebhookController : ControllerBase
{
    private readonly ITelegramBotService botService;

    //Внедрение зависимости, всё по книжечке
    public TelegramWebhookController(ITelegramBotService _botService)
    {
        botService = _botService;
    }


    //Суть метода - принимать вебхуки от Телеграмма (а как я понял там идут POST-методы)
    //и вызывать сервис по обработке (пока назвал его TelegramHandler)
    [HttpPost]
    public async Task<IActionResult> HandleWebhookAsync([FromBody] Update update)
    {
        try
        {
            await botService.HandleMessageAsync(update);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
}