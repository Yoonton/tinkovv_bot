namespace tinkovv_bot
{
    public class Cost
    {
        private List<Expenses> costsExpenses = new List<Expenses>();
        private List<Income> costsIncome = new List<Income>();
        private struct Expenses
        {
            private DateTime date;
            private string category;
            private int sum;
            public Expenses(DateTime date, string category, int sum)
            {
                this.date = date;
                this.category = category;
                this.sum = sum;
            }
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
