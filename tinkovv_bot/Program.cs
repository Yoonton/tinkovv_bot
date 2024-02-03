using Telegram.Bot;
using Telegram.Bot.Types;
namespace tinkovv_bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new TelegramBotClient("6878733229:AAGOk_4FkCvRmDSY1anMdyFSJfaK9FLkOrU");
            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if(message.Text != null)
            {
                if(message.Text.ToLower().Contains("начать"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "аааааа че я делаю");
                    return;
                }

            }
            throw new NotImplementedException();
        }
    }
}
