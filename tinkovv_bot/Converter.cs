using System.Globalization;

public static class Converter
{
    public static int StringToInt(string input)
    {
        int result;
        if (int.TryParse(input, out result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException("Невозможно преобразовать в число");
        }
    }

    public static DateTime StringToDate(string input)
    {

        string cleanedInput = input.Replace(":", "").Replace(",", ".");

        DateTime result;
        if (DateTime.TryParse(cleanedInput, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException("Невозможно преобразовать в дату");
        }
    }
}