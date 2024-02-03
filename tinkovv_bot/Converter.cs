public static class Converter
{
    public static int StringToInt(string input)
    {
        int result = int.Parse(input);
        return result;
    }
    public static DateTime StringToDate(string input)
    {
        DateTime result = DateTime.Parse(input);
        return result;
    }
}