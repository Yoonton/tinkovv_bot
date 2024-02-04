using Org.BouncyCastle.Asn1.Crmf;
using System.Text.Json.Serialization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using RestSharp;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace tinkovv_bot
{
    class Parametr
    {
        [JsonPropertyName("date-time")]
        public string dateTime { get; set; }
        [JsonPropertyName("trx-type")]
        public string trxType { get; set; }
        public int number { get; set; }
        public string category { get; set; }
    }
    class QueryResult
    {
        public string queryText { get; set; }
        public Parametr parameters { get; set; }
    }
    class Result
    {
        public QueryResult queryResult { get; set; }
    }
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
                                case MessageType.Voice:
                                    if (message.Voice != null)
                                    {
                                        var fileId = update.Message.Voice.FileId;
                                        var fileInfo = await botClient.GetFileAsync(fileId);
                                        var filePath = fileInfo.FilePath;
                                        const string destinationFilePath = $@"C:\Users\mak-s\OneDrive\Рабочий стол\voice\a.ogg";
                                        await using Stream fileStream = System.IO.File.Create(destinationFilePath);
                                        await botClient.DownloadFileAsync(
                                            filePath: filePath,
                                            destination: fileStream,
                                            cancellationToken: token);
                                        string oggFilePath = @"C:\Users\mak-s\OneDrive\Рабочий стол\voice\a.ogg";
                                        fileStream.Close();

                                        // Чтение файла OGG в байтовый массив
                                        byte[] oggBytes = System.IO.File.ReadAllBytes(oggFilePath);

                                        // Конвертирование байтового массива в строку Base64
                                        string base64String = Convert.ToBase64String(oggBytes);
                                        string argument = "{\"queryInput\": {\"audioConfig\":{\"sampleRateHertz\": 48000,\"audioEncoding\": \"AUDIO_ENCODING_OGG_OPUS\",\"languageCode\": \"ru\"}},\"inputAudio\":\"" + base64String + "\"}";


                                        var url = "https://dialogflow.googleapis.com/v2/projects/hack-in-come-mfmj/agent/sessions/123456789:detectIntent";
                                        using var client1 = new RestClient(url);

                                        client1.AddDefaultHeader("Content-Type", "application/json");
                                        client1.AddDefaultHeader("Authorization", "Bearer ya29.c.c0AY_VpZjd9dPQxWKiMMp_VDo3FdCRxLnCJd6FiZVxoXlCskDRo_HkGWDhH5DYIW2b2meEOBzZ68AovT2f0EPzXjdWCkb0lHtugMVo8fTkbSR8K07Gk2RV6Cgmmgn_tPzQ4yQ8MCxeiXF2-sUrlEBQ8T7LTGYWrFsfffFx4ZrQTIQcoFPqeOFnwVr-HPAlQrmTs2zfEmNC3F1NRk-DWBp6waAIXOgAnML4001ui-gpu9OtpXaysaDHwrm3lD6diL3JWcDzVcq-cKoUb3X3fTEK86gkO5UPMDbh3fIHwXx7Y14kfiBx17lWCp66QwGJYYlli5oo1ek33thQ0rkAmZoQmPzvNj17CvY-nkT_DaO0Wwqp6up_5kyQjatPH385AucgabBJ5BpBia0BQFQsc0dqv0U6kqSjW60kbQyol6udwxgU5Jr3WeRcVlXwfMeJQacgBB84a04paotzebO8g0gBUq4dtf1xY2j1SyR1MbMrtuXiu9q57eRbBRMZkwSJWhvJ3w8tqQoR4yMMfly9wIIRcIV_fq-bJe40IFpXlwV_hj9fkOJs2089iUQua3x7yueIIb5oxert3u10okWbfll1Uk_q2d5kaaBwacqFMmUuZe433x0XnS3Ztph4FSZmf4Mznk244aW__o0IqQ8WeJMqzMSoXam8fybj_npfX9npdmc3nxurlUym9oFnUnJmqfkuwB1aJwZcvuR6l2zSJvl72WyaBbkk3uZ54nUsfMi2JUpvs4aBp0ghIJayIkrV3wMkg_aeOmBhb8vY7xvQSxs5it2VyX6puFqa0OojwIVBcI9h-enYpn-jcb5B_32x_y9nomvhl9kuU_5MbkXWzJ7q_Qy_c1R79cs44USsvbIUmdVUh_3Qg2j_Q19wSrbbgZhqtVMMeSalpX_wkUiwmr6BQVVZF--XoRVa3XW06vey0bgg3sMSwiXrB5k_2JwBQ1ha7wS_8uMhWx8n58O4nmcFgrQxIRyp72inoRSblxI6RZ5BVrcJk71-vVv");
                                        client1.AddDefaultHeader("x-goog-user-project", "hack-in-come-mfmj");

                                        var req = new RestRequest("/");
                                        req.AddBody(argument);

                                        var response = await client1.PostAsync(req);
                                        Console.WriteLine(response.Content);
                                        Result json = JsonSerializer.Deserialize<Result>(response.Content)!;
                                        if (json.queryResult.queryText == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение");
                                            break;
                                        }
                                        if (json.queryResult.parameters.trxType == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение");
                                            break;
                                        }
                                        if (json.queryResult.parameters.number == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение");
                                            break;
                                        }
                                        if (json.queryResult.parameters.dateTime == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение");
                                            break;
                                        }
                                        if (json.queryResult.parameters.category == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение");
                                            break;
                                        }
                                        if (json.queryResult.parameters.trxType == "outcome")
                                        {
                                            userWallet[user.Id].AddToListIncome();
                                            userWallet[user.Id].CostsIncome[^1] = new Cost.Income(Converter.StringToDate(json.queryResult.parameters.dateTime), json.queryResult.parameters.number);
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                        }
                                        else if (json.queryResult.parameters.trxType == "income")
                                        {
                                            userWallet[user.Id].AddToListExpenses();
                                            userWallet[user.Id].CostsExpenses[^1] = new Cost.Expenses(Converter.StringToDate(json.queryResult.parameters.dateTime),json.queryResult.parameters.category , json.queryResult.parameters.number);
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                        }
                                    }
                                    break;
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
                                                await botClient.SendTextMessageAsync(chat.Id, "Дату нужно писать через точку \nнапример \"01.01.2000\"");
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
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите дату пополнения");
                                                await botClient.SendTextMessageAsync(chat.Id, "Дату нужно писать через точку \nнапример \"01.01.2000\"");
                                            }
                                            else
                                            {
                                                await botClient.SendTextMessageAsync(chat.Id, "Введены не корректные данные.");
                                                await botClient.SendTextMessageAsync(chat.Id, "Введите дату затрат");
                                                await botClient.SendTextMessageAsync(chat.Id, "Дату нужно писать через точку \nнапример \"01.01.2000\"");
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
                                                await botClient.SendTextMessageAsync(chat.Id, "Дату нужно писать через точку \nнапример \"01.01.2000\"");
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
                                                string[] dates = update.Message.Text.Split("-");
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