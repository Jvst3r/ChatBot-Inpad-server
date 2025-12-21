using Microsoft.AspNetCore.Mvc;
using Services;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;
using ChatBot_Inpad_server.Data;

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
//Бот работает в данный момент от этого класса TelegramBotBackgroundService
//TelegramWebhoolController пока оставил мб еще пригодится
public class TelegramBotBackgroundService : BackgroundService
{
    private readonly TelegramBotClient _botClient;
    private readonly ITelegramBotService _botService;
    private readonly ILogger<TelegramBotBackgroundService> _logger;

    public TelegramBotBackgroundService(
        ITelegramBotService botService,
        ILogger<TelegramBotBackgroundService> logger)
    {
        _botService = botService;
        _logger = logger;
        _botClient = new TelegramBotClient(Secrets.TgBotToken);
    }
    //тут все просто, обработка ошибок и вывод инфы о работе бота
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions();

        _botClient.StartReceiving(
            async (client, update, ct) => await _botService.HandleMessageAsync(update),
            (client, exception, ct) =>
            {
                _logger.LogError(exception, "Telegram error");
                return Task.CompletedTask;
            },
            receiverOptions,
            stoppingToken
        );

        var me = await _botClient.GetMe();
        _logger.LogInformation($"Бот @{me.Username} запущен!");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}