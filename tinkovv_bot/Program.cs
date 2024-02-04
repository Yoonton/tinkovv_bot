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
                    InlineKeyboardButton.WithCallbackData("Сводка по категориям", "categoryButton3"),
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
                           // Console.WriteLine("aboba 1 case");
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
                                        client1.AddDefaultHeader("Authorization", "Bearer ya29.c.c0AY_VpZhSRjCYi2TN6yprsyTwrmdcDj1GlBPtWlqcIKpeY5faas5C8N16TNWnirQxfIimu88__me6BWWfM605pLKnryqpTKZNdCyKkCAiIlOZfvGo4Gy1M2rBkaMzmyy6Y-kHwNlrQ4Q1ctoc6dClLKzuqZ6flkoaWjv6pXFwogQzxYsQiw7iTWRyNwe0ti6InJKcNJa0_ndogN6XZBp4ryxZR438I7GhCkwioUCnihd2kh3d5t3nMlY1C_DnQkOFMCqlBxEbJ2o-YHeDB6zG2fg5lETz-BgQaMsVKj01PY1-V_a44GiUgDKsU4cnOVWHJLUZV_d8jvWWlnGB-LX5d45dq_rhXvmU6A1QwK593Fyww0XHdE6lR3gT384Aih206asOj5qbZ5F9r-xuFBmxFqkbhMU_SbJjzedbnn7vzW92Vf8Mtgm1nxa-1hpUb1dYBm6z6e-FeRYm2VIwxpgqeI98rvZFJzysFgdghiUVMbuelMIfjJ47zn7f1k-6i0e5miR2s5zgetd74h5ybMRVglQUUyfgt5M7yZVxgnoh78o94zm_ifwe5pXWzxgue0krViZYp_YFYqvvVkynb3SYcxJqkrFocXReeWj2MeeIqrwq7mjiXq4QhQ-_splZpq4peuttXirwUV1ByvIIU_laqa_qbqapcp0jeJ3_W72c1jF7qZRJfv_pbd2aFF63x-8V31RaF9Z879fwvY_QR55h5V47vJ0_qUMcXh8VmkVv7I9qkR_rpu__bpdMtrZa_sVFbyFb_pjnsbBlXbclt4OSJ5M7lW2-Sps7ch3Sc5YlyepZOdFjV-y7rgw1VYwpiZIwBlorvOsWu47Jp2xRXpap1Jsudl51Fuufv3Wy4lOdS_lcFrr8pWvbjuvVwQdRh4QuwbZnuJXbawYh2IFJ1b6ysWBtsgMmQsB2YJ96vovFQ2obzm7pvcVb1659Ud_JUWxV7zvJwu44WqomBlMdqQX6BjgJzn29WhoczXzJYBqts2J266qVrt9ZxVM");
                                        client1.AddDefaultHeader("x-goog-user-project", "hack-in-come-mfmj");

                                        var req = new RestRequest("/");
                                        req.AddBody(argument);

                                        var response = await client1.PostAsync(req);
                                        Console.WriteLine(response.Content);
                                        Result json = JsonSerializer.Deserialize<Result>(response.Content)!;
                                        if (json.queryResult.queryText == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение");
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            break;
                                        }
                                        if (json.queryResult.parameters.trxType == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение, вот что я получил:");
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            await botClient.SendTextMessageAsync(chat.Id, json.queryResult.queryText);
                                            break;
                                        }
                                        if (json.queryResult.parameters.number == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение, вот что я получил:");
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            await botClient.SendTextMessageAsync(chat.Id, json.queryResult.queryText);
                                            break;
                                        }
                                        if (json.queryResult.parameters.dateTime == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение, вот что я получил:");
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            await botClient.SendTextMessageAsync(chat.Id, json.queryResult.queryText);
                                            break;
                                        }
                                        if (json.queryResult.parameters.category == null)
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, "Повтори сообщение, вот что я получил:");
                                           await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                            await botClient.SendTextMessageAsync(chat.Id, json.queryResult.queryText);
                                            break;
                                        }
                                        if (json.queryResult.parameters.trxType == "outcome")
                                        {
                                            userWallet[user.Id].AddToListExpenses();
                                            await botClient.SendTextMessageAsync(chat.Id, $"Вы сказали: Дата - {json.queryResult.parameters.dateTime} \n Сумма - {json.queryResult.parameters.number.ToString()} \n Категория - {json.queryResult.parameters.category}");
                                            userWallet[user.Id].CostsExpenses[^1] = new Cost.Expenses(Converter.StringToDate(json.queryResult.parameters.dateTime), json.queryResult.parameters.category, json.queryResult.parameters.number);
                                            await botClient.SendTextMessageAsync(chat.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                                        }
                                        else if (json.queryResult.parameters.trxType == "income")
                                        {
                                            await botClient.SendTextMessageAsync(chat.Id, $"Вы сказали: Дата - {json.queryResult.parameters.dateTime} \n Сумма - {json.queryResult.parameters.number.ToString()} \n Категория - {json.queryResult.parameters.category}");
                                            userWallet[user.Id].AddToListIncome();
                                            userWallet[user.Id].CostsIncome[^1] = new Cost.Income(Converter.StringToDate(json.queryResult.parameters.dateTime), json.queryResult.parameters.number);
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

                           // Console.WriteLine("aboba case 2");
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
                                case "categoryButton3":
                                    await botClient.SendTextMessageAsync(chat.Id, PrintInfo.DictionaryToString(PrintInfo.ListOfExpenses(userWallet[user.Id])));
                                    await botClient.DeleteMessageAsync(chat.Id, update.CallbackQuery.Message.MessageId, token);
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