using System.Diagnostics.Eventing.Reader;
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
    //static CancellationToken cts = new CancellationToken();
    private static TelegramBotClient bot = new TelegramBotClient("OUR_TELEGRAM_BOT_TOKEN");
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