namespace Lab6new
{
    public class EmployeeSalaryComparer : IComparer<Employee>
    {
        public int Compare(Employee x, Employee y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            return x.Salary.CompareTo(y.Salary);
        }
    }
}
