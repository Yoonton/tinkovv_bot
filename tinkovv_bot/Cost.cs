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
        public void AddToListExpenses()
        {
            Expenses expenses = new Expenses(DateTime.Today, "", 0);
            costsExpenses.Add(expenses);
        }
        public void AddToListIncome()
        {
            Income income = new Income(DateTime.Today, 0);
            costsIncome.Add(income);
        }
        public int AmountOfExpenses(DateTime dateTime0, DateTime dateTime1)
        {
            int sum = 0;
            foreach (Expenses expenses in costsExpenses)
            {
                if (dateTime0 <= expenses.Date && expenses.Date <= dateTime1)
                {
                    sum += expenses.Sum;
                }
            }
            return sum;
        }
        public int AmountOfExpenses()
        {
            int sum = 0;
            foreach (Expenses expenses in costsExpenses)
            {
                sum += expenses.Sum;
            }
            return sum;
        }
        public int AmountOfExpenses(string category)
        {
            int sum = 0;
            foreach (Expenses expenses in costsExpenses)
            {
                if (category == expenses.Category)
                {
                    sum += expenses.Sum;
                }
            }
            return sum;
        }
        public int AmountOfIncome(DateTime dateTime0, DateTime dateTime1)
        {
            int sum = 0;
            foreach (Income income in costsIncome)
            {
                if (dateTime0 <= income.Date && income.Date <= dateTime1)
                {
                    sum += income.Sum;
                }
            }
            return sum;
        }
        public int AmountOfIncome()
        {
            int sum = 0;
            foreach (Income income in costsIncome)
            {
                sum += income.Sum;
            }
            return sum;
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
