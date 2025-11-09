using Lab6new;

namespace TestProject2
{
    [TestClass]
    public class EmployeeCatalogTests
    {
        private string _testFilePath;

        [TestInitialize]
        public void TestInitialize()
        {
            _testFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName() + ".json");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (System.IO.File.Exists(_testFilePath))
            {
                System.IO.File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public async Task SaveAsync_WithValidData_WritesDataToFile()
        {
            var employees = new List<Employee>
        {
            new Employee("Чувак Ч.Ч.", "Касир", 25000, 5),
            new Employee("Альберт П.П.", "Менеджер", 30000, 3)
        };
            var fileSaver = new JsonFileWriter();

            await fileSaver.SaveAsync(_testFilePath, employees);

            Assert.IsTrue(System.IO.File.Exists(_testFilePath), "Файл має бути створений");
        }

        [TestMethod]
        public async Task LoadAsync_WithValidFile_ReadsDataFromFile()
        {
            var jsonContent = @"[{""FullName"":""Чувак Ч.Ч."",""Position"":""Касир"",""Salary"":25000,""WorkExperience"":5}]";
            await System.IO.File.WriteAllTextAsync(_testFilePath, jsonContent);
            var fileReader = new JsonFileReader();

            var loadedEmployees = await fileReader.LoadAsync<Employee>(_testFilePath);

            Assert.IsNotNull(loadedEmployees, "Завантажені дані не повинні бути null");
            Assert.AreEqual(1, loadedEmployees.Count, "Має завантажитись один співробітник");
            Assert.AreEqual("Чувак Ч.Ч.", loadedEmployees[0].FullName);
            Assert.AreEqual("Касир", loadedEmployees[0].Position);
        }

        [TestMethod]
        public async Task LoadAsync_WithNonExistentFile_ReturnsNull()
        {
            var fileReader = new JsonFileReader();

            var loadedEmployees = await fileReader.LoadAsync<Employee>("nonexistent.json");

            Assert.IsNull(loadedEmployees, "Should return null for non-existent file");
        }
    }
}
