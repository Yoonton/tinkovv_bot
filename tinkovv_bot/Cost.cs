namespace tinkovv_bot
{
    public class Cost
    {
        private List<Expenses> costsExpenses = new List<Expenses>();
        private List<Income> costsIncome = new List<Income>();
        public List<Expenses> CostsExpenses
        { 
            get { return costsExpenses; }
        }
        public List<Income> CostsIncome
        {
            get { return costsIncome; }
        }
        public void AddToListExpenses(DateTime date, string category, int sum)
        {
            Expenses expenses = new Expenses(date, category, sum);
            costsExpenses.Add(expenses);
        }
        public void AddToListIncome(DateTime date, int sum)
        {
            Income income = new Income(date, sum);
            costsIncome.Add(income);
        }
        public struct Expenses
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
        public struct Income
        {
            private DateTime date;
            private int sum;
            public Income(DateTime date, int sum)
            {
                this.date = date;
                this.sum = sum;
            }
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
