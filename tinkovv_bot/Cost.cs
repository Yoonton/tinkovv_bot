namespace tinkovv_bot
{
    public class Cost
    {
        private struct Expenses
        {
            public Expenses(DateTime date, string category, int sum)
            {
                this.date = date;
                this.category = category;
                this.sum = sum;
            }
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
