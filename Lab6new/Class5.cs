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
            var sortedEmployees = new List<Employee>(_allEmployees);
            sortedEmployees.Sort(new EmployeeWorkExperienceComparer());
            UpdateVisibleCollection(sortedEmployees);
        }

        public void SortBySalary()
        {
            var sortedEmployees = new List<Employee>(_allEmployees);
            sortedEmployees.Sort(new EmployeeSalaryComparer());
            UpdateVisibleCollection(sortedEmployees);
        }

        public void FilterBySalary(decimal maxSalary)
        {
            var filtered = _allEmployees.Where(e => e.Salary < maxSalary).ToList();
            UpdateVisibleCollection(filtered);
        }

        public void ResetFilter()
        {
            UpdateVisibleCollection(_allEmployees);
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
