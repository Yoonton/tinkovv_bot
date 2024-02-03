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
                if(message.Text.ToLower().Contains("добваить расходы"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "//добваднеие расхода");
                    return;
                }
                else if(message.Text.ToLower().Contains("добавить доходы"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "//добавление дохода");
                    return;
                }
            }
        }
    }
}
