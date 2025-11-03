using System.Windows;

namespace Lab6new
{
    public partial class MainWindow : Window
    {
        private EmployeeCatalog _catalog;

        public MainWindow()
        {
            InitializeComponent();

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = System.IO.Path.Combine(documentsPath, "employees.json");

            _catalog = new EmployeeCatalog(new JsonFileWriter(), new JsonFileReader(), filePath);

            EmployeesDataGrid.ItemsSource = _catalog.Employees;

            UpdateStatus($"Каталог ініціалізовано. Файл: {filePath}");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newEmployee = new Employee(
                    FullNameTextBox.Text,
                    PositionTextBox.Text,
                    decimal.Parse(SalaryTextBox.Text),
                    int.Parse(WorkExperienceTextBox.Text)
                );
                _catalog.AddEmployee(newEmployee);

                FullNameTextBox.Clear();
                PositionTextBox.Clear();
                SalaryTextBox.Clear();
                WorkExperienceTextBox.Clear();

                UpdateStatus("Співробітника додано успішно");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus($"Помилка додавання: {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem is Employee selectedEmployee)
            {
                _catalog.DeleteEmployee(selectedEmployee);
                UpdateStatus("Співробітника видалено успішно");
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть співробітника для видалення", "Попередження",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SortByWorkExperienceButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.SortByWorkExperience();
            UpdateStatus("Відсортовано за стажем роботи");
        }

        private void SortBySalaryButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.SortBySalary();
            UpdateStatus("Відсортовано за зарплатою");
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterSalaryTextBox.Text))
            {
                _catalog.ResetFilter();
                UpdateStatus("Фільтр скинуто");
            }
            else if (decimal.TryParse(FilterSalaryTextBox.Text, out decimal maxSalary))
            {
                _catalog.FilterBySalary(maxSalary);
                UpdateStatus($"Відфільтровано співробітників з зарплатою менше {maxSalary:C}");
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректну зарплату", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.ResetFilter();
            FilterSalaryTextBox.Clear();
            UpdateStatus("Фільтр скинуто");
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _catalog.SaveAsync();
                MessageBox.Show("Каталог успішно збережено", "Збереження",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateStatus("Каталог збережено успішно");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus($"Помилка збереження: {ex.Message}");
            }
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _catalog.LoadAsync();
                MessageBox.Show("Каталог успішно завантажено", "Завантаження",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateStatus("Каталог завантажено успішно");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus($"Помилка завантаження: {ex.Message}");
            }
        }

        private void UpdateStatus(string message)
        {
            StatusText.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
        }
    }
}