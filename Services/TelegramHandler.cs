using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ChatBotInpadserver.Data.Models;
using ChatBot_Inpad_server.Data;

namespace Services;

public interface ITelegramBotService
{
    Task HandleMessageAsync(Update update);
}

public class TelegramBotService : ITelegramBotService
{
    private readonly TelegramBotClient _bot;
    private readonly ILogger<TelegramBotService> _logger;
    private readonly Dictionary<long, UserSession> _userSessions = new();
    private readonly List<AdviceItem> _adviceList = new();

    public TelegramBotService(ILogger<TelegramBotService> logger)
    {
        _logger = logger;
        _bot = new TelegramBotClient(Secrets.TgBotToken);
        InitializeAdviceData();
    }

    private void InitializeAdviceData()
    {
        //o включить акселератор;
        //o проверить диск C;
        //o закрыть вкладки браузера;
        //o проверить рабочие наборы;
        //o удалить неиспользуемые группы, виды и DWG;
        //o проверить вес файла хранилища;
        //o отключить аналитические данные;
        //o использовать корпоративные семейства.

        // Заполняем базу знаний советами
        _adviceList.AddRange(new[]
        {
            new AdviceItem { Id = 1, Title = "Безопасное подключение", Content = "🔒 Всегда проверяйте сертификаты сервера перед подключением к VPN.", Category = "security" },
            new AdviceItem { Id = 2, Title = "Оптимальный сервер", Content = "🚀 Выбирайте сервер ближайший к вашему местоположению для лучшей скорости.", Category = "performance" },
            new AdviceItem { Id = 3, Title = "Wi-Fi защита", Content = "📡 Всегда используйте VPN при подключении к публичным Wi-Fi сетям.", Category = "security" },
            new AdviceItem { Id = 4, Title = "Скорость соединения", Content = "📊 Если скорость низкая, попробуйте переключиться на другой сервер или протокол.", Category = "performance" },
            new AdviceItem { Id = 5, Title = "⚡ Общие советы", Content = "📱 Мой совет это - включить акселератор.", Category = "general" },
            new AdviceItem { Id = 6, Title = "⚡ Общие советы", Content = "📱 Можете проверить диск C.", Category = "general" },
            new AdviceItem { Id = 7, Title = "⚡ Общие советы", Content = "📱 Попробуйте закрыть вкладки браузера.", Category = "general" },
            new AdviceItem { Id = 8, Title = "⚡ Общие советы", Content = "📱 Незабудьте проверить рабочие наборы.", Category = "general" },
            new AdviceItem { Id = 9, Title = "⚡ Общие советы", Content = "📱 Советую  удалить неиспользуемые группы, виды и DWG.", Category = "general" },
            new AdviceItem { Id = 10, Title = "⚡ Общие советы", Content = "📱 Так же советую проверить вес файла хранилища.", Category = "general" },
            new AdviceItem { Id = 10, Title = "⚡ Общие советы", Content = "📱 Отключите аналитические данныеа.", Category = "general" },
            new AdviceItem { Id = 10, Title = "⚡ Общие советы", Content = "📱 используйте корпоративные семейства.", Category = "general" }
        });
    }

    public async Task HandleMessageAsync(Update update)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    await HandleMessage(update.Message!);
                    break;

                case UpdateType.CallbackQuery:
                    await HandleCallbackQuery(update.CallbackQuery!);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Ошибка обработки сообщения");
        }
    }

    private async Task HandleMessage(Message message)
    {
        var chatId = message.Chat.Id;
        var username = message.Chat.Username ?? "Unknown";
        var messageText = message.Text ?? string.Empty;

        _logger.LogInformation("📨 Сообщение от @{Username}: {MessageText}", username, messageText);

        if (messageText == "/start")
        {
            await ShowMainMenu(chatId, username);
        }
        if (messageText == "🔙 Главное меню")
        {
            await ShowMainMenu(chatId, username);
        }
        else if (messageText.StartsWith("/"))
        {
            await HandleCommand(chatId, messageText, username);
        }
        else
        {
            await HandleTextMessage(chatId, messageText, username);
        }
    }

    private async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        var chatId = callbackQuery.Message!.Chat.Id;
        var username = callbackQuery.From.Username ?? "Unknown";
        var data = callbackQuery.Data;

        _logger.LogInformation("🔄 Callback от @{Username}: {CallbackData}", username, data);

        if (data.StartsWith("next_advice:"))
        {
            var parts = data.Split(':');
            if (parts.Length >= 3)
            {
                var category = parts[1];
                var currentIndex = int.Parse(parts[2]); // Текущий индекс (1-based)

                await ShowAdvice(chatId, category, currentIndex, username);
            }
        }

        await _bot.AnswerCallbackQuery(callbackQuery.Id);
    }

    private async Task ShowMainMenu(long chatId, string username)
    {
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("📖 Инструкция"),  new KeyboardButton("📚 База знаний") }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };

        await _bot.SendMessage(
            chatId: chatId,
            text: "🔒 Вот функции которые мне доступны\n\nВыберите действие:",
            parseMode: ParseMode.Markdown,
            replyMarkup: replyKeyboard,
            cancellationToken: CancellationToken.None);
    }
    //Обработка кнопочек
    private async Task HandleTextMessage(long chatId, string messageText, string username)
    {
        switch (messageText)
        {
            case "📚 База знаний":
                await ShowKnowledgeBaseMenu(chatId, username);
                break;
            case "🚀 Производительность":
                await ShowFirstAdvice(chatId, "performance", username);
                break;

            case "⚡ Общие советы":
                await ShowFirstAdvice(chatId, "general", username);
                break;
        }
    }
    //Кнопочки с их ключами, чтобы обращаться к их советам
    private async Task ShowKnowledgeBaseMenu(long chatId, string username)
    {
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("⚡ Общие советы"), new KeyboardButton("🚀 Производительность") },
            new[] { new KeyboardButton("🔙 Главное меню") }
        })
        {
            ResizeKeyboard = true
        };
        // Это сообщение выведется первым
        await _bot.SendMessage(
            chatId: chatId,
            text: "📚 **База знаний**\n\nВыберите категорию советов:",
            parseMode: ParseMode.Markdown,
            replyMarkup: replyKeyboard,
            cancellationToken: CancellationToken.None);
    }

    private async Task ShowFirstAdvice(long chatId, string category, string username)
    {
        // Инициализируем сессию пользователя
        if (!_userSessions.ContainsKey(chatId))
        {
            _userSessions[chatId] = new UserSession { ChatId = chatId, CurrentCategory = category };
        }
        else
        {
            _userSessions[chatId].CurrentCategory = category;
            _userSessions[chatId].CurrentAdviceIndex = 0;
        }

        await ShowAdvice(chatId, category, 0, username);
    }

    private async Task ShowAdvice(long chatId, string category, int adviceIndex, string username)
    {
        var categoryAdvices = _adviceList.Where(a => a.Category == category).ToList();

        if (!categoryAdvices.Any())
        {
            await _bot.SendMessage(
                chatId: chatId,
                text: "❌ В этой категории пока нет советов.",
                cancellationToken: CancellationToken.None);
            return;
        }

        if (adviceIndex >= categoryAdvices.Count)
        {
            adviceIndex = 0; // Возвращаемся к первому совету
        }

        var advice = categoryAdvices[adviceIndex];
        var currentNumber = adviceIndex + 1;
        var totalCount = categoryAdvices.Count;

        // Создаем инлайн-кнопку "Следующий совет"
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    "▶️ Следующий совет",
                    $"next_advice:{category}:{currentNumber}")
            }
        });
        // Не работала кнопка Глав Меню, пока сделал так
        if (currentNumber == 8)
        {
            var finishinlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "▶️ Главное меню",
                        $"next_advice:{ShowMainMenu(chatId, username)}")
                }
            });
        }
        // Описание внизу, ну ты видел
        var messageText = $"💡 **{advice.Title}**\n\n{advice.Content}\n\n" +
                         $"📖 Совет {currentNumber} из {totalCount} в категории";

        await _bot.SendMessage(
            chatId: chatId,
            text: messageText,
            parseMode: ParseMode.Markdown,
            replyMarkup: inlineKeyboard,
            cancellationToken: CancellationToken.None);

        // Обновляем индекс в сессии
        if (_userSessions.ContainsKey(chatId))
        {
            _userSessions[chatId].CurrentAdviceIndex = currentNumber; // Следующий индекс
        }

        _logger.LogInformation("📚 Показан совет {AdviceId} пользователю @{Username}", advice.Id, username);
    }

    private async Task HandleCommand(long chatId, string command, string username)
    {
        // TODO: Реализовать обработку команд
        await _bot.SendMessage(
            chatId: chatId,
            text: $"Команда {command} пока не реализована",
            cancellationToken: CancellationToken.None);
    }
}