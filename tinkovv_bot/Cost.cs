namespace tinkovv_bot
{
    public class Cost
    {
        List<Expenses> expenses = new List<Expenses>();
        private struct Expenses
        {
            private DateTime date;
            private string category;
            private int sum;
            public DateTime Date
            {
                get { return date; }
                set { date = value; }
            }
            public string Category
            {
                get { return category; }
                set { category = value; }
            }
            public int Sum
            {
                get { return sum; }
                set { sum = value; }
            }
        }
        private struct Income
        {
            private DateTime date;
            private int sum;
            public DateTime Date
            {
                get { return date; }
                set { date = value; }
            }
            public int Sum
            {
                get { return sum; }
                set { sum = value; }
            }
        }
    }
}
