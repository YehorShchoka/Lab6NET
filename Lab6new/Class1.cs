namespace Lab6new
{
    public class Employee : IComparable<Employee>
    {
        private string _fullName;
        private string _position;
        private decimal _salary;
        private int _workExperience;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ПІБ не може бути порожнім");
                }
                _fullName = value;
            }
        }

        public string Position
        {
            get { return _position; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Посада не може бути порожньою");
                }
                _position = value;
            }
        }

        public decimal Salary
        {
            get { return _salary; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Зарплата не може бути від'ємною");
                }
                _salary = value;
            }
        }

        public int WorkExperience
        {
            get { return _workExperience; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Стаж не може бути від'ємним");
                }
                _workExperience = value;
            }
        }

        public Employee(string fullName, string position, decimal salary, int workExperience)
        {
            FullName = fullName;
            Position = position;
            Salary = salary;
            WorkExperience = workExperience;
        }

        public int CompareTo(Employee other)
        {
            if (other == null)
            {
                return 1;
            }
            return this.WorkExperience.CompareTo(other.WorkExperience);
        }

        public override string ToString()
        {
            return $"ПІБ: {FullName}, Посада: {Position}, Зарплата: {Salary:# ##0.00} ₴, Стаж: {WorkExperience} років";
        }
    }
}
