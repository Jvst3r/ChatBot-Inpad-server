using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace Services;

    
public interface ITelegramBotService
{
    Task HandleMessageAsync(Update update);
}
public class TelegramBotService : ITelegramBotService
{
    private readonly TelegramBotClient telegramBotClient;
        public Task HandleMessageAsync(Update update)
        {
            if (update.Message is { } message)
        {
            var chatId = message.Chat.Id;
            var user = message.Chat.Username;


            return Task.CompletedTask;//заглушечка
        }
        else
        {
            return Task.CompletedTask; //заглушечка
        }
        }
}