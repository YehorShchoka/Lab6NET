using System.Collections.ObjectModel;

namespace Lab6new
{
    public class EmployeeCatalog
    {
        private readonly IFileSaver _fileSaver;
        private readonly IFileLoader _fileLoader;
        private readonly string _filePath;
        private List<Employee> _allEmployees;

        public ObservableCollection<Employee> Employees { get; }

        public EmployeeCatalog(IFileSaver fileSaver, IFileLoader fileLoader, string filePath)
        {
            _fileSaver = fileSaver;
            _fileLoader = fileLoader;
            _filePath = filePath;
            Employees = new ObservableCollection<Employee>();
            _allEmployees = new List<Employee>();
        }

        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
            _allEmployees.Add(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            Employees.Remove(employee);
            _allEmployees.Remove(employee);
        }

        public void SortByWorkExperience()
        {
            var sortedEmployees = _allEmployees
                .OrderBy(e => e.WorkExperience)
                .ToList();
            UpdateVisibleCollection(sortedEmployees);
        }

        public void SortBySalary()
        {
            var sortedEmployees = _allEmployees
                .OrderBy(e => e.Salary)
                .ToList();
            UpdateVisibleCollection(sortedEmployees);
        }

        public void FilterBySalary(decimal maxSalary)
        {
            var filtered = _allEmployees
                .Where(e => e.Salary < maxSalary)
                .ToList();
            UpdateVisibleCollection(filtered);
        }

        public void ResetFilter()
        {
            UpdateVisibleCollection(_allEmployees);
        }

        public List<Employee> SearchEmployees(string searchTerm)
        {
            return _allEmployees
                .Where(e => e.FullName.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                           e.Position.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<string> GetUniquePositions()
        {
            return _allEmployees
                .Select(e => e.Position)
                .Distinct()
                .OrderBy(p => p)
                .ToList();
        }

        public List<Employee> GetTop3BySalary()
        {
            return _allEmployees
                .OrderByDescending(e => e.Salary)
                .Take(3)
                .ToList();
        }

        public decimal CalculateAverageSalary()
        {
            return _allEmployees.Any()
                ? _allEmployees.Average(e => e.Salary)
                : 0;
        }

        public Dictionary<string, List<Employee>> GroupEmployeesByPosition()
        {
            if (!_allEmployees.Any())
                return new Dictionary<string, List<Employee>>();

            return _allEmployees
                .GroupBy(e => e.Position)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.OrderBy(e => e.FullName).ToList());
        }

        public List<Employee> GetEmployeesWithExperienceMoreThan(int years)
        {
            return _allEmployees
                .Where(e => e.WorkExperience > years)
                .OrderByDescending(e => e.WorkExperience)
                .ToList();
        }

        private void UpdateVisibleCollection(List<Employee> newList)
        {
            Employees.Clear();
            foreach (var employee in newList)
            {
                Employees.Add(employee);
            }
        }

        public async Task SaveAsync()
        {
            await _fileSaver.SaveAsync(_filePath, _allEmployees);
        }

        public async Task LoadAsync()
        {
            var loadedEmployees = await _fileLoader.LoadAsync<Employee>(_filePath);
            if (loadedEmployees != null)
            {
                _allEmployees = loadedEmployees;
                UpdateVisibleCollection(_allEmployees);
            }
        }
    }
}
