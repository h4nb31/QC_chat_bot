using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace QualityControl.Properties
{
    public class BotService : IHostedService
    {
        private readonly string _botToken;
        //private readonly IReviewService _reviewService;
        private readonly IServiceScopeFactory _scopeFactory;
        private TelegramBotClient? _botClient;
        private CancellationTokenSource? _cts;

        public BotService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _botToken = "6530522678:AAECd4ghXCFfh7-o8hJT1baYzT-IgCDz-h0";
        }
        
        public Task StartAsync(CancellationToken cToken)
        {
            _botClient = new TelegramBotClient(_botToken);
            _cts = new CancellationTokenSource();
            //SetWebhookAsync().GetAwaiter().GetResult();

            var recieverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,  //текстовые сообщения
                    UpdateType.CallbackQuery //inline кнопки
                },
                ThrowPendingUpdates = true
            };

            //Обработка получений обновлений бота с использованием scope фабрики
            _botClient.StartReceiving(
                async (botClient, update, canellationToken) =>
                   {
                       using (var scope = _scopeFactory.CreateScope())
                       {
                           var handlers = scope.ServiceProvider.GetRequiredService<BotHandlers>();
                           await handlers.HandlerUpdateAsync(botClient, update, canellationToken);
                       }
                   },
                    BotHandlers.HandelPollingErrorAsync,
                    recieverOptions,
                    cancellationToken: cToken
                );


            return Task.CompletedTask;
        }

        //private async Task SetWebhookAsync()  //Реализация получения апдейтов через вебхук
        //{
        //    ArgumentNullException.ThrowIfNull(_botClient);
        //    var webhookUrl = "https://146.158.119.207/webhook";
        //    await _botClient.SetWebhookAsync(webhookUrl);
        //}

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(_cts);
            _cts?.Cancel();
            //_botClient?.DeleteWebhookAsync().GetAwaiter().GetResult();  //Удаление вебхука при выключении бота

            return Task.CompletedTask;
        }
    }
}
