using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace tinkovv_bot
{
    internal class Program
    {
        public static Dictionary<long, string> currentState = new Dictionary<long, string>();
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
            var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Добавить доход", "button1"),
                    InlineKeyboardButton.WithCallbackData("Добавить расход", "button2"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Вывсти доход", "button3"),
                    InlineKeyboardButton.WithCallbackData("Вывести расход", "button4"),
                },
            });
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            var message = update.Message;
                            var user = message.From;
                            Console.WriteLine($"{user.FirstName}    |    {message.Text}");
                            var chat = message.Chat;
                            switch (message.Type)
                            {
                                case MessageType.Text:
                                    {
                                        if (message.Text == "/start" || currentState[user.Id] == "start")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id,"Сервис учета личных расходов");
                                            await botClient.SendTextMessageAsync(chat.Id,"Выберите пункт меню",replyMarkup: inlineKeyboard);
                                            currentState[user.Id] = "start";
                                            Console.WriteLine(chat.Id);
                                        }
                                        else
                                        {
                                            if (currentState[user.Id] == "start")
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            }
                                        }
                                        return;
                                    }
                                default:
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Используйте только текст!");
                                        return;
                                    }
                            }
                        }

                    case UpdateType.CallbackQuery:
                        {
                            var callbackQuery = update.CallbackQuery;
                            var user = callbackQuery.From;
                            Console.WriteLine($"{user.FirstName}    |    {callbackQuery.Data}");
                            var chat = callbackQuery.Message.Chat;
                            switch (callbackQuery.Data)
                            {
                                case "button1":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        if (currentState.ContainsKey(user.Id))
                                        {
                                            currentState[user.Id] = "Income";
                                        }
                                        else
                                        {
                                            currentState.Add(user.Id, "Income");
                                        }
                                        break;
                                    }

                                case "button2":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        if (currentState.ContainsKey(user.Id))
                                        {
                                            currentState[user.Id] = "Expenses";
                                        }
                                        else
                                        {
                                            currentState.Add(user.Id, "Expenses");
                                        }
                                        break;
                                    }

                                case "button3":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        if (currentState.ContainsKey(user.Id))
                                        {
                                            currentState[user.Id] = "IncomeOut";
                                        }
                                        else
                                        {
                                            currentState.Add(user.Id, "IncomeOut");
                                        }
                                        break;
                                    }
                                case "button4":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        if (currentState.ContainsKey(user.Id))
                                        {
                                            currentState[user.Id] = "ExpensesOut";
                                        }
                                        else
                                        {
                                            currentState.Add(user.Id, "ExpensesOut");
                                        }
                                        break;
                                    }
                            }
                            switch(currentState[user.Id])
                            {
                                case "Income":
                                    await botClient.SendTextMessageAsync(chat.Id, "Введите сумму пополнения");
                                    Console.WriteLine(update.Message);
                                    if (update.Message != null)
                                    {
                                        currentState[user.Id] = "IncomeSumAdd";
                                        Console.WriteLine("aboba");
                                    }
                                    else if(update.Message != null)
                                    {
                                        await botClient.SendTextMessageAsync(chat.Id, "Введены не корректные данные. Введите еще раз");
                                    }
                                    else
                                    {
                                        Console.WriteLine("хуйня");
                                    }
                                    break;
                                case "Expenses":
                                    Console.WriteLine("2");
                                    break;
                                case "IncomeOut":
                                    Console.WriteLine("3");
                                    break;
                                case "ExpensesOut":
                                    Console.WriteLine("4");
                                    break;
                                default:
                                    Console.WriteLine(currentState[user.Id]);
                                    Console.WriteLine(user.Id);
                                    break;
                            }
                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
