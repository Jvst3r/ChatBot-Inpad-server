// Services/TelegramBotService.cs
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ChatBotInpadServer.Data.Models;
using ChatBotInpadServer.Services;
using System.Text.Json;
using ChatBotInpadServer.Data;

namespace Services
{
    public interface ITelegramBotService
    {
        Task HandleMessageAsync(Update update);
    }

    public class TelegramBotService : ITelegramBotService
    {
        private readonly TelegramBotClient bot;
        private readonly KnowledgeService knowledgeService;
        private readonly UserService userService;
        private readonly string token;
  

        public TelegramBotService(
            KnowledgeService knowledgeService,
            UserService userService,
            IConfiguration configuration)
        {
            this.knowledgeService = knowledgeService;
            this.userService = userService;

            token = Secrets.TgBotToken   //configuration["Telegram:BotToken"]
                ?? throw new ArgumentException("Telegram Bot Token не найден в конфигурации");

            bot = new TelegramBotClient(token);
            Console.WriteLine("Telegram Bot Service инициализирован");
        }


        
        /// <summary>
        /// Определяет тип сообщения от телеграма и вызывает нужный метод для ответа
        /// </summary>
        /// <param name="update">Тип сообщения от Телеграма</param>
        /// <returns></returns>
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
                Console.WriteLine($"Ошибка обработки сообщения: {ex.Message}");
            }
        }

        /// <summary>
        /// Обрабатывает текстовое сообщение, определяет тип текстового сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <returns></returns>
        private async Task HandleMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var username = message.Chat.Username ?? $"User_{chatId}";
            var messageText = message.Text ?? string.Empty;

            Console.WriteLine($"Сообщение от @{username} ({chatId}): {messageText}");

            // Сохраняем пользователя в БД
            await userService.GetOrCreateUserAsync("Telegram", chatId.ToString(), username);

            // Обработка команд
            if (messageText.StartsWith("/"))
            {
                await HandleCommand(chatId, messageText, username);
                return;
            }

            // Обработка текстовых команд (кнопок)
            await HandleTextCommand(chatId, messageText, username);
        }

        /// <summary>
        /// Обрабатывает текстовые команды, начинающиеся на "/"
        /// </summary>
        /// <param name="chatId">ID чата</param>
        /// <param name="command">команда</param>
        /// <param name="username">Телеграм @</param>
        /// <returns></returns>
        private async Task HandleCommand(long chatId, string command, string username)
        {
            command = command.ToLower().Trim();

            switch (command)
            {
                case "/start":
                    await ShowStartMessage(chatId, username);
                    break;
                case "/search":
                    await StartSearchMode(chatId, username);
                    break;
                case "/help":
                case "/помощь":
                    await ShowHelp(chatId, username);
                    break;
                case "/menu":
                    await ShowMainMenu(chatId, username);
                    break;
                case "/stats":
                    await ShowStatistics(chatId, username);
                    break;
                case "/categories":
                    await ShowCategories(chatId, username);
                    break;
                default:
                    await SendMessage(chatId, "❌ Неизвестная команда. Используйте /help для списка команд.");
                    break;
            }
        }

        /// <summary>
        /// Показывает приветственное ссобщение с меню и описанием команд
        /// </summary>
        /// <param name="chatId">ID чата телеграм</param>
        /// <param name="username">Телеграм @</param>
        /// <returns></returns>
        private async Task ShowStartMessage(long chatId, string username)
        {
            var welcomeText =
                "🤖 *Добро пожаловать в BIM Chat Bot!*\n\n" +
                "Я помогу вам с вопросами по Revit и BIM-моделированию.\n\n" +
                "*Основные команды:*\n" +
                "• /search - поиск совета по вопросу\n" +
                "• /categories - просмотр категорий\n" +
                "• /menu - главное меню\n" +
                "• /stats - статистика\n" +
                "• /help - справка\n\n" +
                "Просто напишите ваш вопрос, и я постараюсь помочь!";

            await SendMessage(chatId, welcomeText, GetMainMenuKeyboard());
        }

        /// <summary>
        /// Обрабатывает команды из кнопок
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="text"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task HandleTextCommand(long chatId, string text, string username)
        {
            switch (text)
            {
                case "🔙 Главное меню":
                    await ShowMainMenu(chatId, username);
                    break;
                case "📚 База знаний":
                    await ShowCategories(chatId, username);
                    break;
                case "🔍 Поиск совета":
                    await StartSearchMode(chatId, username);
                    break;
                case "📊 Статистика":
                    await ShowStatistics(chatId, username);
                    break;
                case "❓ Помощь":
                    await ShowHelp(chatId, username);
                    break;
                default:
                    // Если текст начинается с эмодзи категории
                    if (text.StartsWith("📁 "))
                    {
                        var category = text.Replace("📁 ", "");
                        await ShowKnowledgeByCategory(chatId, category, username);
                    }
                    else
                    {
                        // Обычный поиск
                        await SearchAndRespond(chatId, text, username);
                    }
                    break;
            }
        }

        private async Task ShowMainMenu(long chatId, string username)
        {
            var text = "🎯 *Главное меню*\n\nВыберите действие:";
            await SendMessage(chatId, text, GetMainMenuKeyboard());
        }

        private async Task StartSearchMode(long chatId, string username)
        {
            await SendMessage(chatId, "🔍 *Поиск совета*\n\nНапишите ваш вопрос:");
        }

        private async Task SearchAndRespond(long chatId, string query, string username)
        {
            Console.WriteLine($"🔎 Поиск по запросу: {query}");

            
            var knowledgeItem = await knowledgeService.SearchByQueryAsync(query);

            if (knowledgeItem != null)
            {
                // Увеличиваем счетчик использования
                await knowledgeService.IncrementUseCountAsync(knowledgeItem.Id);

                // Сохраняем сообщение в историю
                await userService.SaveMessageAsync(
                    text: query,
                    platform: "Telegram",
                    platformId: chatId.ToString(),
                    isFromUser: true);

                await userService.SaveMessageAsync(
                    text: knowledgeItem.AnswerText,
                    platform: "Telegram",
                    platformId: chatId.ToString(),
                    isFromUser: false,
                    knowledgeItemId: knowledgeItem.Id);

                var response =
                    $"💡 *{knowledgeItem.Title}*\n\n" +
                    $"📁 Категория: {knowledgeItem.Category}\n\n" +
                    $"{knowledgeItem.AnswerText}\n\n" +
                    $"🏷️ Теги: {knowledgeItem.Tags}";

                await SendMessage(chatId, response, GetMainMenuKeyboard());
            }
            else
            {
                var response =
                    "❌ *В базе данных не найден подходящий совет*\n\n" +
                    "Ваш вопрос отправлен администратору на уточнение.\n" +
                    "Мы добавим ответ в ближайшее время!";

                // Сохраняем неизвестный вопрос
                await userService.SaveMessageAsync(
                    text: query,
                    platform: "Telegram",
                    platformId: chatId.ToString(),
                    isFromUser: true);

                await userService.SaveMessageAsync(
                    text: "Ответ не найден, отправлен администратору",
                    platform: "Telegram",
                    platformId: chatId.ToString(),
                    isFromUser: false);
                //НАДО ДОРАБОТАТЬ!!!
                Console.WriteLine($" Не найден ответ на вопрос: '{query}'. Отправлено администратору");

                await SendMessage(chatId, response, GetMainMenuKeyboard());
            }
        }

        private async Task ShowCategories(long chatId, string username)
        {
            try
            {
                var categories = await knowledgeService.GetAllCategoriesAsync();

                if (!categories.Any())
                {
                    await SendMessage(chatId, "📚 База знаний пуста.", GetMainMenuKeyboard());
                    return;
                }

                var keyboardRows = new List<KeyboardButton[]>();
                var currentRow = new List<KeyboardButton>();

                foreach (var category in categories)
                {
                    currentRow.Add(new KeyboardButton($"📁 {category}"));

                    if (currentRow.Count == 2)
                    {
                        keyboardRows.Add(currentRow.ToArray());
                        currentRow.Clear();
                    }
                }

                if (currentRow.Any())
                {
                    keyboardRows.Add(currentRow.ToArray());
                }

                // Кнопки навигации
                keyboardRows.Add(new[] { new KeyboardButton("🔙 Главное меню") });

                var keyboard = new ReplyKeyboardMarkup(keyboardRows)
                {
                    ResizeKeyboard = true
                };

                await SendMessage(chatId, "📚 *База знаний*\n\nВыберите категорию:", keyboard);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении категорий: {ex.Message}");
                await SendMessage(chatId, "❌ Ошибка при загрузке категорий.", GetMainMenuKeyboard());
            }
        }

        private async Task ShowKnowledgeByCategory(long chatId, string category, string username, int currentIndex = 0)
        {
            try
            {
                // Советы по категории
                var items = await knowledgeService.GetItemsByCategoryAsync(category, 1, 1000); // Большое число чтобы получить все

                if (!items.Any())
                {
                    await SendMessage(chatId, $"Категория '{category}' пока пуста.", GetMainMenuKeyboard());
                    return;
                }

                
                if (currentIndex >= items.Count) currentIndex = 0;
                if (currentIndex < 0) currentIndex = items.Count - 1;

                var item = items[currentIndex];

                // Увеличиваем счетчик использования
                await knowledgeService.IncrementUseCountAsync(item.Id);

                // Создаем inline-кнопки для навигации
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            "◀️ Предыдущий",
                            $"cat_prev:{category}:{currentIndex}"),
                        InlineKeyboardButton.WithCallbackData(
                            $"{currentIndex + 1}/{items.Count}",
                            $"cat_counter:{category}:{currentIndex}"),
                        InlineKeyboardButton.WithCallbackData(
                            "Следующий ▶️",
                            $"cat_next:{category}:{currentIndex}")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            "🔙 К категориям",
                            "back_to_categories")
                    }
                });

                var messageText =
                    $"💡 *{item.Title}*\n\n" +
                    $"{item.AnswerText}\n\n" +
                    $"📁 Категория: {item.Category}\n" +
                    $"🏷️ Теги: {item.Tags}\n" +
                    $"📊 Использовано: {item.UseCount} раз";

                await SendMessage(chatId, messageText, inlineKeyboard);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке советов: {ex.Message}");
                await SendMessage(chatId, "❌ Ошибка при загрузке советов.", GetMainMenuKeyboard());
            }
        }

        private async Task HandleCallbackQuery(CallbackQuery callbackQuery)
        {
            var chatId = callbackQuery.Message!.Chat.Id;
            var messageId = callbackQuery.Message.MessageId;
            var username = callbackQuery.From.Username ?? $"User_{chatId}";
            var data = callbackQuery.Data;

            Console.WriteLine($"Callback от {username}: {data}");

            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    await AnswerCallbackQuery(callbackQuery.Id);
                    return;
                }

                // Обработка навигации по категориям
                if (data.StartsWith("cat_") || data.StartsWith("category:"))
                {
                    await HandleCategoryNavigation(callbackQuery, chatId, messageId, username, data);
                    return;
                }

                // Обработка возврата к категориям
                if (data == "back_to_categories")
                {
                    await HandleBackToCategories(callbackQuery, chatId, messageId, username);
                    return;
                }

                // Обработка возврата в главное меню
                if (data == "back_to_main_menu")
                {
                    await HandleBackToMainMenu(callbackQuery, chatId, messageId, username);
                    return;
                }

                await AnswerCallbackQuery(callbackQuery.Id, "Неизвестная команда");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка обработки callback: {ex.Message}");
                await AnswerCallbackQuery(callbackQuery.Id, "Произошла ошибка");
            }
        }

        private async Task HandleCategoryNavigation(CallbackQuery callbackQuery, long chatId, int messageId, string username, string data)
        {
            try
            {
                if (data.StartsWith("category:"))
                {
                    // Выбор категории из списка
                    var cat = data.Substring("category:".Length);
                    await ShowKnowledgeByCategory(chatId, cat, username, 0);
                    await AnswerCallbackQuery(callbackQuery.Id);
                    return;
                }

                // Парсим данные навигации
                var parts = data.Split(':');
                if (parts.Length < 3)
                {
                    await AnswerCallbackQuery(callbackQuery.Id, "Ошибка в данных навигации");
                    return;
                }

                var action = parts[0];
                var category = parts[1];
                var currentIndex = int.Parse(parts[2]);

                // Получаем все элементы категории
                var items = await knowledgeService.GetItemsByCategoryAsync(category, 1, 1000);

                if (!items.Any())
                {
                    await AnswerCallbackQuery(callbackQuery.Id, "Категория пуста");
                    return;
                }

                var newIndex = currentIndex;

                switch (action)
                {
                    case "cat_next":
                        newIndex = (currentIndex + 1) % items.Count;
                        break;
                    case "cat_prev":
                        newIndex = (currentIndex - 1 + items.Count) % items.Count;
                        break;
                    case "cat_counter":
                        await AnswerCallbackQuery(callbackQuery.Id, $"Совет {currentIndex + 1} из {items.Count}");
                        return;
                }

                // Обновляем сообщение с новым индексом
                await UpdateCategoryMessage(chatId, messageId, category, newIndex, items);
                await AnswerCallbackQuery(callbackQuery.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка обработки навигации по категории: {ex.Message}");
                await AnswerCallbackQuery(callbackQuery.Id, "Ошибка навигации");
            }
        }

        private async Task UpdateCategoryMessage(long chatId, int messageId, string category, int index, List<KnowledgeItem> items)
        {
            try
            {
                var item = items[index];

                // Увеличиваем счетчик использования
                await knowledgeService.IncrementUseCountAsync(item.Id);

                // Создаем inline-кнопки для навигации
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            "◀️ Предыдущий",
                            $"cat_prev:{category}:{index}"),
                        InlineKeyboardButton.WithCallbackData(
                            $"{index + 1}/{items.Count}",
                            $"cat_counter:{category}:{index}"),
                        InlineKeyboardButton.WithCallbackData(
                            "Следующий ▶️",
                            $"cat_next:{category}:{index}")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            "🔙 К категориям",
                            "back_to_categories")
                    }
                });

                var messageText =
                    $"💡 *{item.Title}*\n\n" +
                    $"{item.AnswerText}\n\n" +
                    $"📁 Категория: {item.Category}\n" +
                    $"🏷️ Теги: {item.Tags}\n" +
                    $"📊 Использовано: {item.UseCount} раз";

                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: messageText,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: inlineKeyboard,
                    cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка обновления сообщения: {ex.Message}");
                // Если не удалось отредактировать, отправляем новое
                await ShowKnowledgeByCategory(chatId, category, "", index);
            }
        }

        private async Task HandleBackToCategories(CallbackQuery callbackQuery, long chatId, int messageId, string username)
        {
            try
            {
                // Удаляем сообщение с советами
                try
                {
                    await bot.DeleteMessage(chatId, messageId, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Не удалось удалить сообщение: {ex.Message}");
                }

                // Показываем категории
                await ShowKnowledgeBaseCategories(chatId, username);
                await AnswerCallbackQuery(callbackQuery.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка возврата к категориям: {ex.Message}");
                await AnswerCallbackQuery(callbackQuery.Id, "Ошибка возврата к категориям");
            }
        }

        private async Task HandleBackToMainMenu(CallbackQuery callbackQuery, long chatId, int messageId, string username)
        {
            try
            {
                // Удаляем сообщение с категориями
                try
                {
                    await bot.DeleteMessage(chatId, messageId, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Не удалось удалить сообщение: {ex.Message}");
                }

                // Показываем главное меню
                await ShowMainMenu(chatId, username);
                await AnswerCallbackQuery(callbackQuery.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка возврата в главное меню: {ex.Message}");
                await AnswerCallbackQuery(callbackQuery.Id, "Ошибка возврата в меню");
            }
        }

        private async Task ShowKnowledgeBaseCategories(long chatId, string username)
        {
            try
            {
                var categories = await knowledgeService.GetAllCategoriesAsync();

                if (!categories.Any())
                {
                    await SendMessage(chatId, "📚 База знаний пуста.", GetMainMenuKeyboard());
                    return;
                }

                var inlineKeyboardRows = new List<InlineKeyboardButton[]>();
                var currentRow = new List<InlineKeyboardButton>();

                foreach (var category in categories)
                {
                    currentRow.Add(InlineKeyboardButton.WithCallbackData(
                        text: $"📁 {category}",
                        callbackData: $"category:{category}"));

                    if (currentRow.Count == 2)
                    {
                        inlineKeyboardRows.Add(currentRow.ToArray());
                        currentRow.Clear();
                    }
                }

                if (currentRow.Any())
                {
                    inlineKeyboardRows.Add(currentRow.ToArray());
                }

                // Кнопка возврата в главное меню
                inlineKeyboardRows.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "🔙 Главное меню",
                        callbackData: "back_to_main_menu")
                });

                var inlineKeyboard = new InlineKeyboardMarkup(inlineKeyboardRows);

                await SendMessage(chatId, "📚 *База знаний*\n\nВыберите категорию:", inlineKeyboard);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при получении категорий: {ex.Message}");
                await SendMessage(chatId, "❌ Ошибка при загрузке категорий.", GetMainMenuKeyboard());
            }
        }

        private async Task AnswerCallbackQuery(string callbackQueryId, string? text = null)
        {
            try
            {
                await bot.AnswerCallbackQuery(
                    callbackQueryId: callbackQueryId,
                    text: text,
                    cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка ответа на callback: {ex.Message}");
            }
        }

        private async Task ShowHelp(long chatId, string username)
        {
            var helpText =
                "📖 *Справка по использованию бота*\n\n" +
                "*Команды:*\n" +
                "• /start - Начало работы\n" +
                "• /search - Поиск совета\n" +
                "• /categories - Все категории\n" +
                "• /menu - Главное меню\n" +
                "• /stats - Статистика\n" +
                "• /help - Справка\n\n" +
                "*Как пользоваться:*\n" +
                "1. Напишите вопрос\n" +
                "2. Или выберите категорию\n" +
                "3. Бот найдет подходящий ответ";

            await SendMessage(chatId, helpText, GetMainMenuKeyboard());
        }

        private async Task ShowStatistics(long chatId, string username)
        {
            try
            {
                var knowledgeStats = await knowledgeService.GetAllKnowledgeItemsAsync();
                var userStats = await userService.GetUserStatsAsync();

                var totalKnowledge = knowledgeStats.Items.Count;
                var totalUses = knowledgeStats.Items.Sum(i => i.UseCount);

                var statsText =
                    $"📊 *Статистика*\n\n" +
                    $"📚 Всего советов: {totalKnowledge}\n" +
                    $"📈 Использовано советов: {totalUses}\n" +
                    $"👥 Пользователей: {userStats.TotalUsers}\n" +
                    $"💬 Сообщений: {userStats.TotalMessages}\n\n" +
                    $"*Популярные советы:*\n";

                var popularItems = knowledgeStats.Items
                    .OrderByDescending(i => i.UseCount)
                    .Take(3);

                foreach (var item in popularItems)
                {
                    statsText += $"\n• {item.Title} - {item.UseCount} раз";
                }

                await SendMessage(chatId, statsText, GetMainMenuKeyboard());
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка получения статистики: {ex.Message}");
                await SendMessage(chatId, "❌ Ошибка при загрузке статистики.", GetMainMenuKeyboard());
            }
        }

        private async Task SendMessage(
            long chatId,
            string text,
            ReplyMarkup? replyMarkup = null,
            ParseMode parseMode = ParseMode.Markdown)
        {
            try
            {
                await bot.SendMessage(
                    chatId: chatId,
                    text: text,
                    parseMode: parseMode,
                    replyMarkup: replyMarkup,
                    cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка отправки сообщения в {chatId}: {ex.Message}");
            }
        }

        private ReplyKeyboardMarkup GetMainMenuKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("📚 База знаний"), new KeyboardButton("🔍 Поиск совета") },
                new[] { new KeyboardButton("📊 Статистика"), new KeyboardButton("❓ Помощь") }
            })
            {
                ResizeKeyboard = true
            };
        }
    }

    // Вспомогательные классы для хранения сессий (состояния), помогают показывать советы по порядку
    public class UserSession
    {
        public long ChatId { get; set; }
        public string? CurrentCategory { get; set; }
        public List<KnowledgeItem> CurrentItems { get; set; } = new();
        public int CurrentIndex { get; set; }
        public bool HasShownCurrentItem { get; set; } = false;
    }
}