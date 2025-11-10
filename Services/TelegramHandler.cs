using System.Diagnostics.Eventing.Reader;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System;

namespace Services;

    
public interface ITelegramBotService
{
    Task HandleMessageAsync(Update update);
}
public class TelegramBotService : ITelegramBotService
{
    //static CancellationToken cts = new CancellationToken();
    private static readonly TelegramBotClient bot = new TelegramBotClient("OUR_TELEGRAM_BOT_TOKEN");
    public Task HandleMessageAsync(Update update)
    { 
        if (update.Message is { } message)
        {
            var chatId = message.Chat.Id;
            var user = message.Chat.Username;
            var messageText = message.Text;
            if (messageText == "/start")
            {
                bot.SendMessage(chatId,"Бот запущен!");
                return Task.CompletedTask;
            }
            throw new Exception("Невозможный запрос!");
        }
        else
        {
            return Task.FromException(new Exception("Я не могу обработать данный запрос:("));
        }
    }
}

public class PollingTelegramService : BackgroundService, ITelegramBotService
{
    
    private static readonly TelegramBotClient bot = new TelegramBotClient("BOT-TOKEN");
    private static int lastUpdateId = 0;
    private static readonly InputFileId stikerUrfu = new("CAACAgIAAxkBAAETylVpEcQ84-hTzKQhtN2-iD8inCFvIAACLoUAAmg6-Et9M1CxiyA3qTYE");
    public Task HandleMessageAsync(Update update)
    {
        if (update.Message is { } message)
        { 
            var chatId = message.Chat.Id;
            var user = message.Chat.Username;
            var messageText = message.Text;
            Console.WriteLine($"Пользователь {user} отправил сообщение {messageText}!");
            if (messageText == "/start")
            {
                    Console.WriteLine($"Ответ отправлен пользователю {user}!");
                    bot.SendMessage(chatId, $"Бот запущен! \nПриветствую,{user}!\n" +
                        $"Этот бот ещё не доделан, но процесс идёт!\n" +
                        $"Спасибо за тест!");
                    Thread.Sleep(500);
                    bot.SendSticker(chatId, stikerUrfu);
                    Console.WriteLine($"Ответ отправлен пользователю {user}!");
                
                
                return Task.CompletedTask;
            }
            Console.WriteLine($"Пользователь {chatId} отправил пока невозможный запрос!");
            throw new Exception($"Невозможный запрос!");
        }
        else
        {
            return Task.FromException(new Exception("Я не могу обработать данный запрос:("));
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var updates = await bot.GetUpdates(
                    offset: lastUpdateId + 1,
                    limit: 10,
                    timeout: 60,
                    cancellationToken: stoppingToken);

                foreach (var update in updates)
                {
                    if (update.Id > lastUpdateId)
                    {
                        await HandleMessageAsync(update);
                        lastUpdateId = update.Id;
                    }
                    await Task.Delay(1000, stoppingToken);

                }
            }
            catch (OperationCanceledException)
            {
                // выход при остановке
                break;
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            await Task.Delay(1000,stoppingToken);
        }
    }
}