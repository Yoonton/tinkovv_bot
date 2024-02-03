﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
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
                                        if (message.Text == "/start")
                                        {
                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Сервис учета личных расходов");
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
                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Выберите пункт меню",
                                                replyMarkup: inlineKeyboard);
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
                                        await botClient.SendTextMessageAsync(chat.Id, "!доход!");
                                        return;
                                    }

                                case "button2":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        await botClient.SendTextMessageAsync(chat.Id, "Введите ");
                                        return;
                                    }

                                case "button3":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        await botClient.SendTextMessageAsync(chat.Id, "!вывод дохода!");
                                        return;
                                    }
                                case "button4":
                                    {
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        await botClient.SendTextMessageAsync(chat.Id, "!вывод расхода");
                                        return;
                                    }
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
