// Services/TelegramBotBackgroundService.cs
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Services
{
    public class TelegramBotBackgroundService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ITelegramBotClient botClient;

        public TelegramBotBackgroundService(
            IServiceProvider serviceProvider,
            ITelegramBotClient botClient)
        {
            this.serviceProvider = serviceProvider;
            this.botClient = botClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Telegram Bot Background Service запущен");

            using var cts = new CancellationTokenSource();

            // Настройки получения обновлений
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                DropPendingUpdates = true, // Вместо ThrowPendingUpdates в новых версиях
            };

            // Запускаем обработчик обновлений
            botClient.StartReceiving(
              updateHandler: HandleUpdateAsync,
              errorHandler: HandlePollingErrorAsync
          );

            // Получаем информацию о боте
            try
            {
                var me = await botClient.GetMe(cancellationToken: stoppingToken);
                Console.WriteLine($"Бот @{me.Username} запущен и готов к работе!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении информации о боте: {ex.Message}");
            }

            // Ждем отмены
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient,
            Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            // СОЗДАЕМ НОВЫЙ SCOPE ДЛЯ КАЖДОГО СООБЩЕНИЯ
            using (var scope = serviceProvider.CreateScope())
            {
                try
                {
                    // Получаем сервисы из SCOPE (они будут Scoped)
                    var botService = scope.ServiceProvider.GetRequiredService<ITelegramBotService>();
                    await botService.HandleMessageAsync(update);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Ошибка обработки обновления: {ex.Message}");
                }
            }
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient,
            Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($" Ошибка Telegram API: {exception.Message}");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(" Telegram Bot Background Service остановлен");
            await base.StopAsync(cancellationToken);
        }
    }
}