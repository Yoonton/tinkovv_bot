using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace tinkovv_bot
{
    internal class Program
    {
        public static Dictionary<long, string> currentState = new Dictionary<long, string>();
        public static Dictionary<long, Cost> userWallet = new Dictionary<long, Cost>();
        public static Cost cost = new Cost();
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
            string category;
            var inlineOutExpensesChoiseKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Вывести сумму за все время", "allTimeButton2"),
                    InlineKeyboardButton.WithCallbackData("Вывести сумму за период", "periodTimeButton2"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Вывести сумму по категориям", "categoryButton"),
                },

            });
            var inlineOutIncomeChoiseKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Вывести сумму за все время","allTimeButton"),
                    InlineKeyboardButton.WithCallbackData("Вывести сумму за период", "periodTimeButton"),
                },
            });
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

                            if (!userWallet.ContainsKey(update.Message.From.Id))
                            {
                                userWallet.Add(update.Message.From.Id, new Cost());
                            }
                            Console.WriteLine("aboba 1 case");
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
                                            await botClient.SendTextMessageAsync(chat.Id, "Сервис учета личных расходов");
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            currentState[user.Id] = "start";
                                            Console.WriteLine(chat.Id);
                                        }
                                        else if (currentState[user.Id] == "Income")
                                        {

                                            if (update.Message != null && InputValidator.IsNumeric(update.Message.Text))
                                            {
                                                DateTime dateTemp = userWallet[user.Id].CostsIncome[^1].Date;
                                                Cost.Income temp = new Cost.Income(dateTemp, 0);
                                                temp.Sum = Converter.StringToInt(update.Message.Text);
                                                userWallet[user.Id].CostsIncome[^1] = temp;
                                                currentState[user.Id] = "IncomeDateAdd";
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите дату пополнения");
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Введены не корректные данные.");
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите сумму пополнения");
                                                currentState[user.Id] = "Income";
                                            }

                                        }
                                        else if (currentState[user.Id] == "Expenses")
                                        {
                                            if (update.Message != null && InputValidator.IsNumeric(update.Message.Text))
                                            {
                                                DateTime dateTemp = userWallet[user.Id].CostsExpenses[^1].Date;
                                                string categoryTemp = userWallet[user.Id].CostsExpenses[^1].Category;
                                                Cost.Expenses temp = new Cost.Expenses(dateTemp, categoryTemp, Converter.StringToInt(update.Message.Text));
                                                userWallet[user.Id].CostsExpenses[^1] = temp;
                                                currentState[user.Id] = "ExpensesDateAdd";
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите дату затрат");
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Введены не корректные данные.");
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите дату затрат");
                                                currentState[user.Id] = "ExpensesSumAdd";
                                            }
                                        }
                                        else if (currentState[user.Id] == "IncomeOut")
                                        {

                                        }
                                        else if (currentState[user.Id] == "ExpensesOut")
                                        {
                                            //добавить 3 кнопки(за все время\по категория\за период)
                                        }
                                        else if (currentState[user.Id] == "IncomeSumAdd")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите сумму пополнения");
                                            if (update.Message != null && InputValidator.IsNumeric(update.Message.Text))
                                            {
                                                DateTime dateTemp = userWallet[user.Id].CostsIncome[^1].Date;
                                                Cost.Income temp = new Cost.Income(dateTemp, Converter.StringToInt(update.Message.Text));
                                                temp.Sum = Converter.StringToInt(update.Message.Text);
                                                userWallet[user.Id].CostsIncome[^1] = temp;
                                                currentState[user.Id] = "IncomeDateAdd";
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Введены не корректные данные. Введите еще раз");
                                            }
                                        }
                                        else if (currentState[user.Id] == "ExpensesSumAdd")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Введите сумму затарт");
                                            if (update.Message != null && InputValidator.IsNumeric(update.Message.Text))
                                            {
                                                DateTime dateTemp = userWallet[user.Id].CostsExpenses[^1].Date;
                                                string categoryTemp = userWallet[user.Id].CostsExpenses[^1].Category;
                                                Cost.Expenses temp = new Cost.Expenses(dateTemp, categoryTemp, Converter.StringToInt(update.Message.Text));
                                                userWallet[user.Id].CostsExpenses[^1] = temp;
                                                currentState[user.Id] = "ExpensesDateAdd";
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите дату затрат");
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Введены не корректные данные. Введите еще раз");
                                            }
                                        }
                                        else if (currentState[user.Id] == "IncomeDateAdd")
                                        {
                                            if (update.Message != null && InputValidator.IsDate(update.Message.Text))
                                            {
                                                int sumTemp = userWallet[user.Id].CostsIncome[^1].Sum;
                                                Cost.Income temp = new Cost.Income(Converter.StringToDate(update.Message.Text), sumTemp);
                                                userWallet[user.Id].CostsIncome[^1] = temp;
                                                currentState[user.Id] = "start";
                                                await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Введены не корректные данные. Введите еще раз");
                                            }
                                        }
                                        else if (currentState[user.Id] == "ExpensesDateAdd")
                                        {
                                            if (update.Message != null && InputValidator.IsDate(update.Message.Text))
                                            {
                                                int sumTemp = userWallet[user.Id].CostsExpenses[^1].Sum;
                                                string categoryTemp = userWallet[user.Id].CostsExpenses[^1].Category;
                                                Cost.Expenses temp = new Cost.Expenses(Converter.StringToDate(update.Message.Text), categoryTemp, sumTemp);
                                                userWallet[user.Id].CostsExpenses[^1] = temp;
                                                currentState[user.Id] = "ExpensesCategoryAdd";
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите категорию затрат");
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Введены не корректные данные. Введите еще раз");
                                            }
                                        }
                                        else if (currentState[user.Id] == "ExpensesCategoryAdd")
                                        {
                                            int sumTemp = userWallet[user.Id].CostsExpenses[^1].Sum;
                                            DateTime dateTemp = userWallet[user.Id].CostsExpenses[^1].Date;
                                            Cost.Expenses temp = new Cost.Expenses(dateTemp, update.Message.Text, sumTemp);
                                            userWallet[user.Id].CostsExpenses[^1] = temp;
                                            currentState[user.Id] = "start";
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                        }
                                        else if (currentState[user.Id] == "waitingCategory")
                                        {
                                            if(update.Message != null)
                                            {
                                                category = update.Message.Text;
                                                await botClient.SendTextMessageAsync(chat.Id, $"Трат за {category}: {userWallet[user.Id].AmountOfExpenses(category)}");
                                                currentState[user.Id] = "start";
                                                await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            }
                                        }
                                        else if (currentState[user.Id] == "waitingDate")
                                        {
                                            if(update.Message != null)
                                            {
                                                string[] dates = update.Message.Text.Split(" - ");
                                                if(dates.Length == 2)
                                                {
                                                    if (InputValidator.IsDate(dates[0]) && InputValidator.IsDate(dates[1]))
                                                    {
                                                        await botClient.SendTextMessageAsync(chat.Id, $"Трат за {dates[0]} - {dates[1]}: {userWallet[user.Id].AmountOfIncome(Converter.StringToDate(dates[0]), Converter.StringToDate(dates[1]))}");
                                                        currentState[user.Id] = "start";
                                                        await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                                    }
                                                }
                                            }
                                        }
                                        else if (currentState[user.Id] == "waitingDate2")
                                        {
                                            if (update.Message != null)
                                            {
                                                string[] dates = update.Message.Text.Split(" - ");
                                                if (dates.Length == 2)
                                                {
                                                    if (InputValidator.IsDate(dates[0]) && InputValidator.IsDate(dates[1]))
                                                    {
                                                        await botClient.SendTextMessageAsync(chat.Id, $"Трат за {dates[0]} - {dates[1]}: {userWallet[user.Id].AmountOfExpenses(Converter.StringToDate(dates[0]), Converter.StringToDate(dates[1]))}");
                                                        currentState[user.Id] = "start";
                                                        await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (currentState[user.Id] == "start")
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            }
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Используйте только текст!");
                                        break;
                                    }
                            }
                            break;
                        }

                    case UpdateType.CallbackQuery:
                        {

                            Console.WriteLine("aboba case 2");
                            var callbackQuery = update.CallbackQuery;
                            var user = callbackQuery.From;
                            Console.WriteLine($"{user.FirstName}    |    {callbackQuery.Data}");
                            var chat = callbackQuery.Message.Chat;
                            if (!userWallet.ContainsKey(user.Id))
                            {
                                userWallet.Add(update.CallbackQuery.Message.From.Id, new Cost());
                            }
                            switch (callbackQuery.Data)
                            {
                                case "button1":
                                    {
                                        userWallet[user.Id].AddToListIncome();
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        await botClient.SendTextMessageAsync(user.Id, "Введите сумму пополнения");
                                        if (currentState.ContainsKey(user.Id))
                                        {
                                            currentState[user.Id] = "Income";
                                        }
                                        else
                                        {
                                            currentState.Add(user.Id, "Income");
                                        }
                                        await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
                                        break;
                                    }

                                case "button2":
                                    {
                                        userWallet[user.Id].AddToListExpenses();
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        await botClient.SendTextMessageAsync(user.Id, "Введите сумму затрат");
                                        if (currentState.ContainsKey(user.Id))
                                        {
                                            currentState[user.Id] = "Expenses";
                                        }
                                        else
                                        {
                                            currentState.Add(user.Id, "Expenses");
                                        }
                                        await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
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
                                        await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
                                        await botClient.SendTextMessageAsync(chat.Id, "Выберете пунк меню", replyMarkup: inlineOutIncomeChoiseKeyboard);
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
                                        await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
                                        await botClient.SendTextMessageAsync(chat.Id, "Выберете пункт меню", replyMarkup: inlineOutExpensesChoiseKeyboard);
                                        break;
                                    }
                                case "allTimeButton":
                                    {
                                        await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
                                        await botClient.SendTextMessageAsync(chat.Id, $"Сумма пополнения за все время: {userWallet[user.Id].AmountOfIncome()}");
                                        await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                        currentState[user.Id] = "start";
                                        break;
                                    }
                                case "periodTimeButton":
                                    await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
                                    await botClient.SendTextMessageAsync(chat.Id, "Введите промежуток дат через тире");
                                    currentState[user.Id] = "waitingDate";
                                    break;
                                case "allTimeButton2":
                                    await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
                                    await botClient.SendTextMessageAsync(chat.Id, $"Сумма затрат за все время: {userWallet[user.Id].AmountOfExpenses()}");
                                    await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                    currentState[user.Id] = "start";
                                    break;
                                case "categoryButton":
                                    await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
                                    await botClient.SendTextMessageAsync(chat.Id, "Введите категорю трат");
                                    currentState[user.Id] = "waitingCategory";
                                    break;
                                case "periodTimeButton2":
                                    await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
                                    await botClient.SendTextMessageAsync(chat.Id, "Введите промежуток дат через тире");
                                    currentState[user.Id] = "waitingDate2";
                                    break;
                            }
                            break;
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