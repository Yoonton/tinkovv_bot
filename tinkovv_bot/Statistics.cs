namespace tinkovv_bot
{
    internal static class PrintInfo
    {
        public static Dictionary<string, int> ListOfExpenses(Cost cost)
        {
            Dictionary<string, int> expenses = new Dictionary<string, int>();
            foreach (var expense in cost.CostsExpenses)
            {
                if (!expenses.ContainsKey(expense.Category))
                {
                    expenses.Add(expense.Category, expense.Sum);
                }
                else
                {
                    expenses[expense.Category] += expense.Sum;
                }
            }
            return expenses;
        }
        public static Dictionary<string, double> PercentageOfExpenses(Cost cost)
        {
            Dictionary<string, int> exp = ListOfExpenses(cost);
            int general = 0;
            foreach (var value in exp.Values)
            {
                general += value;
            }
            Dictionary<string, double> percentage = new Dictionary<string, double>();
            foreach (var key in exp.Keys)
            {
                percentage.Add(key, (double)exp[key]);
            }
            foreach (string key in percentage.Keys)
            {
                percentage[key] = (exp[key] / (double)general) * 100;
            }
            var sortedPercentage = percentage.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return PercentageToNormal(sortedPercentage);
        }
        public static Dictionary<string, double> PercentageToNormal(Dictionary<string, double> dict)
        {
            Dictionary<string, int> intDictionary = new Dictionary<string, int>();
            foreach (var key in dict.Keys)
            {
                intDictionary.Add(key, (int)dict[key]);
            }
            int general = 0;
            string nowKey = "";
            foreach (var key in intDictionary.Keys)
            {
                general += intDictionary[key];
                nowKey = key;
            }
            if (general < 100)
                intDictionary[nowKey]++;
            foreach (var key in intDictionary.Keys)
            {
                dict[key] = (double)intDictionary[key];
            }
            return dict;
        }
        public static string DictionaryToString(Dictionary<string, int> dict)
        {
            string resultString = "";
            foreach (var key in dict.Keys)
            {
                resultString += $"{key.ToString()}: {dict[key]} рублей\n";
            }
            return resultString;
        }
        public static string DictionaryToString(Dictionary<string, double> dict)
        {
            string resultString = "";
            foreach (var key in dict.Keys)
            {
                resultString += $"{key.ToString()}: {(int)dict[key]}%\n";
            }
            return resultString;
        }
    }
}
