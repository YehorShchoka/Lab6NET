using System.Text;
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
                ShowResults("");
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
                ShowResults("");
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
            ShowResults("Список відсортовано за стажем роботи");
        }

        private void SortBySalaryButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.SortBySalary();
            UpdateStatus("Відсортовано за зарплатою");
            ShowResults("Список відсортовано за зарплатою");
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterSalaryTextBox.Text))
            {
                _catalog.ResetFilter();
                UpdateStatus("Фільтр скинуто");
                ShowResults("Фільтр скинуто - показано всіх співробітників");
            }
            else if (decimal.TryParse(FilterSalaryTextBox.Text, out decimal maxSalary))
            {
                _catalog.FilterBySalary(maxSalary);
                UpdateStatus($"Відфільтровано співробітників з зарплатою менше {maxSalary:C}");
                ShowResults($"Показано співробітників з зарплатою менше {maxSalary:C}");
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
            ShowResults("Фільтр скинуто - показано всіх співробітників");
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _catalog.SaveAsync();
                MessageBox.Show("Каталог успішно збережено", "Збереження",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateStatus("Каталог збережено успішно");
                ShowResults("Каталог успішно збережено");
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
                ShowResults($"Завантажено {_catalog.Employees.Count} співробітників");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus($"Помилка завантаження: {ex.Message}");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                ShowResults("Будь ласка, введіть текст для пошуку");
                return;
            }

            var results = _catalog.SearchEmployees(SearchTextBox.Text);
            if (results.Any())
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Знайдено {results.Count} співробітників:");
                foreach (var employee in results)
                {
                    sb.AppendLine($"- {employee.FullName}, {employee.Position}, {employee.Salary:C}, {employee.WorkExperience} років");
                }
                ShowResults(sb.ToString());
                UpdateVisibleCollection(results);
                UpdateStatus($"Знайдено {results.Count} співробітників за запитом '{SearchTextBox.Text}'");
            }
            else
            {
                ShowResults($"Співробітників за запитом '{SearchTextBox.Text}' не знайдено");
                UpdateStatus("Пошук не дав результатів");
            }
        }

        private void ShowUniquePositionsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var positions = _catalog.GetUniquePositions();
                if (positions.Any())
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Унікальні посади ({positions.Count}):");
                    foreach (var position in positions)
                    {
                        var count = _catalog.SearchEmployees(position).Count;
                        sb.AppendLine($"- {position} ({count} співробітників)");
                    }
                    ShowResults(sb.ToString());
                    UpdateStatus($"Знайдено {positions.Count} унікальних посад");
                }
                else
                {
                    ShowResults("Немає даних про посади");
                    UpdateStatus("Каталог порожній");
                }
            }
            catch (Exception ex)
            {
                ShowResults($"Помилка: {ex.Message}");
                UpdateStatus($"Помилка отримання унікальних посад: {ex.Message}");
            }
        }

        private void ShowTop3BySalaryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var topEmployees = _catalog.GetTop3BySalary();
                if (topEmployees.Any())
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Топ-3 співробітників за зарплатою:");
                    int place = 1;
                    foreach (var employee in topEmployees)
                    {
                        sb.AppendLine($"{place}. {employee.FullName}: {employee.Salary:C} ({employee.Position})");
                        place++;
                    }
                    ShowResults(sb.ToString());
                    UpdateVisibleCollection(topEmployees);
                    UpdateStatus("Показано топ-3 співробітників за зарплатою");
                }
                else
                {
                    ShowResults("Немає даних для формування топ-3");
                    UpdateStatus("Каталог порожній");
                }
            }
            catch (Exception ex)
            {
                ShowResults($"Помилка: {ex.Message}");
                UpdateStatus($"Помилка отримання топ-3: {ex.Message}");
            }
        }

        private void ShowAverageSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var averageSalary = _catalog.CalculateAverageSalary();
                if (averageSalary > 0)
                {
                    var allEmployees = _catalog.SearchEmployees("");
                    var maxSalary = allEmployees.Max(e => e.Salary);
                    var minSalary = allEmployees.Min(e => e.Salary);

                    var sb = new StringBuilder();
                    sb.AppendLine("Статистика зарплат:");
                    sb.AppendLine($"- Середня зарплата: {averageSalary:C}");
                    sb.AppendLine($"- Максимальна зарплата: {maxSalary:C}");
                    sb.AppendLine($"- Мінімальна зарплата: {minSalary:C}");
                    sb.AppendLine($"- Кількість співробітників: {allEmployees.Count}");

                    ShowResults(sb.ToString());
                    UpdateStatus($"Середня зарплата: {averageSalary:C}");
                }
                else
                {
                    ShowResults("Немає даних для розрахунку середньої зарплати");
                    UpdateStatus("Каталог порожній");
                }
            }
            catch (Exception ex)
            {
                ShowResults($"Помилка: {ex.Message}");
                UpdateStatus($"Помилка розрахунку середньої зарплати: {ex.Message}");
            }
        }

        private void ShowGroupByPositionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var grouped = _catalog.GroupEmployeesByPosition();
                if (grouped.Any())
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Співробітники згруповані за посадою:");
                    foreach (var group in grouped)
                    {
                        sb.AppendLine($"\n {group.Key} ({group.Value.Count} співробітників):");
                        foreach (var employee in group.Value)
                        {
                            sb.AppendLine($" {employee.FullName}, {employee.Salary:C}, {employee.WorkExperience} років");
                        }
                    }
                    ShowResults(sb.ToString());

                    _catalog.ResetFilter();
                    UpdateStatus($"Згруповано {grouped.Count} посад, {grouped.Sum(g => g.Value.Count)} співробітників");
                }
                else
                {
                    ShowResults("Немає даних для групування");
                    UpdateStatus("Каталог порожній");
                }
            }
            catch (Exception ex)
            {
                ShowResults($"Помилка: {ex.Message}");
                UpdateStatus($"Помилка групування: {ex.Message}");
            }
        }

        private void ShowExperiencedButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ExperienceFilterTextBox.Text, out int years))
            {
                try
                {
                    var experienced = _catalog.GetEmployeesWithExperienceMoreThan(years);
                    if (experienced.Any())
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine($"Співробітники зі стажем більше {years} років ({experienced.Count} осіб):");
                        foreach (var employee in experienced)
                        {
                            sb.AppendLine($"- {employee.FullName}: {employee.WorkExperience} років, {employee.Position}, {employee.Salary:C}");
                        }
                        ShowResults(sb.ToString());
                        UpdateVisibleCollection(experienced);
                        UpdateStatus($"Знайдено {experienced.Count} співробітників зі стажем більше {years} років");
                    }
                    else
                    {
                        ShowResults($"Співробітників зі стажем більше {years} років не знайдено");
                        UpdateStatus("Результатів не знайдено");
                    }
                }
                catch (Exception ex)
                {
                    ShowResults($"Помилка: {ex.Message}");
                    UpdateStatus($"Помилка пошуку: {ex.Message}");
                }
            }
            else
            {
                ShowResults("Будь ласка, введіть коректну кількість років");
                UpdateStatus("Некоректне значення стажу");
            }
        }

        private void UpdateVisibleCollection(List<Employee> employees)
        {
            _catalog.Employees.Clear();
            foreach (var employee in employees)
            {
                _catalog.Employees.Add(employee);
            }
        }

        private void ShowResults(string message)
        {
            ResultsTextBox.Text = message;
        }

        private void UpdateStatus(string message)
        {
            StatusText.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
        }
    }
}