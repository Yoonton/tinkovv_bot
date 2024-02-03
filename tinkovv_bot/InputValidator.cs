namespace tinkovv_bot
{
    public static class InputValidator
    {
        public static bool IsNumeric(string input)
        {
            double number;
            return Double.TryParse(input, out number);
        }

        public static bool IsDate(string input)
        {
            DateTime date;
            return DateTime.TryParse(input, out date);
        }
    }
}
