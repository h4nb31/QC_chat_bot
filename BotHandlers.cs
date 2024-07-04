using QualityControl.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace QualityControl.Properties
{
    public class BotHandlers
    {
        private readonly IReviewService _reviewService;
        public BotHandlers(IReviewService reviewService)
        {

            _reviewService = reviewService;
        }
        public async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cToken)
        {



            if (update.Message is not { } Message)
                return;
            if (Message.Text is not { } messageText)
                return;

            var chatId = Message.Chat.Id;
            var TopicId = Message.MessageThreadId;
            var UserInfo = Message.From;
            ArgumentNullException.ThrowIfNull(UserInfo);

            var review = new ReviewListModel
            {
                Name = $"{UserInfo.FirstName} - {UserInfo.Username}",
                ReviewType = "TelegramTest",
                Object = $"{Message.Chat.FirstName}",
                ReviewText = messageText
            };

            try
            {
                await _reviewService.AddReviewAsync(review);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Youre Text was added to base: [{messageText}]",
                cancellationToken: cToken
                );

            //if(update.Type == UpdateType.Message && update.Message?.Text != null)
            //{
            //    using var scope = _scopedFactroy.CreateScope();
            //    var reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();

            //    var message = update.Message;
            //    var user = message.From;
            //    var chatId = update.Message.Chat.Id;
            //    var messageText = message.Text;

            //    var review = new ReviewListModel
            //    {
            //        Name = $"{user?.FirstName}",
            //        ReviewType = "TelegramTest",
            //        Object = $"{message.Chat.FirstName}",
            //        ReviewText = messageText
            //    };

            //    try
            //    {
            //        await _reviewService.AddReviewAsync(review);
            //    }
            //    catch (Exception ex)
            //    {
            //        await Console.Out.WriteLineAsync(ex.ToString());
            //    }

            //    Message sentMessage = await botClient.SendTextMessageAsync(
            //    chatId: chatId,
            //    text: $"Youre Text was added to base: [{messageText}]",
            //    cancellationToken: cToken
            //    );
            //}

        }

        public static Task HandelPollingErrorAsync( ITelegramBotClient botClient, Exception exception, CancellationToken cToken )
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n{apiRequestException.ErrorCode}\n[{apiRequestException.Message}]",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
