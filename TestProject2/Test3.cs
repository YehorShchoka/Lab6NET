
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Lab6new;

namespace TestProject2
{
    [TestClass]
    public class EmployeeCatalogLinqTests
    {
        private EmployeeCatalog _catalog;
        private List<Employee> _testEmployees;

        [TestInitialize]
        public void TestInitialize()
        {
            _catalog = new EmployeeCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            _testEmployees = new List<Employee>
        {
            new Employee("Іванов Іван Іванович", "Інженер", 25000, 5),
            new Employee("Петров Петро Петрович", "Менеджер", 30000, 3),
            new Employee("Сидоров Сергій Сергійович", "Інженер", 28000, 7),
            new Employee("Коваленко Олександр Михайлович", "Аналітик", 35000, 2),
            new Employee("Шевченко Тарас Григорович", "Менеджер", 32000, 4)
        };

            foreach (var employee in _testEmployees)
            {
                _catalog.AddEmployee(employee);
            }
        }

        [TestMethod]
        public void SearchEmployees_WithExistingName_ReturnsEmployees()
        {

            var result = _catalog.SearchEmployees("Іванов");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Іванов Іван Іванович", result[0].FullName);
        }

        [TestMethod]
        public void SearchEmployees_WithExistingPosition_ReturnsEmployees()
        {

            var result = _catalog.SearchEmployees("Інженер");

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(e => e.Position == "Інженер"));
        }

        [TestMethod]
        public void SearchEmployees_WithNonExistingTerm_ReturnsEmptyList()
        {

            var result = _catalog.SearchEmployees("Директор");

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetUniquePositions_ReturnsDistinctPositions()
        {

            var result = _catalog.GetUniquePositions();

            Assert.AreEqual(3, result.Count);
            CollectionAssert.Contains(result, "Інженер");
            CollectionAssert.Contains(result, "Менеджер");
            CollectionAssert.Contains(result, "Аналітик");
        }

        [TestMethod]
        public void GetTop3BySalary_ReturnsThreeHighestPaidEmployees()
        {

            var result = _catalog.GetTop3BySalary();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Коваленко Олександр Михайлович", result[0].FullName);
            Assert.AreEqual("Шевченко Тарас Григорович", result[1].FullName);
            Assert.AreEqual("Петров Петро Петрович", result[2].FullName);
        }

        [TestMethod]
        public void CalculateAverageSalary_ReturnsCorrectAverage()
        {

            var expectedAverage = (25000m + 30000m + 28000m + 35000m + 32000m) / 5;

            var result = _catalog.CalculateAverageSalary();

            Assert.AreEqual(expectedAverage, result);
        }

        [TestMethod]
        public void CalculateAverageSalary_WithEmptyCatalog_ReturnsZero()
        {

            var emptyCatalog = new EmployeeCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            var result = emptyCatalog.CalculateAverageSalary();

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GroupEmployeesByPosition_ReturnsCorrectGroups()
        {

            var result = _catalog.GroupEmployeesByPosition();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, result["Аналітик"].Count);
            Assert.AreEqual(2, result["Інженер"].Count);
            Assert.AreEqual(2, result["Менеджер"].Count);
        }

        [TestMethod]
        public void GetEmployeesWithExperienceMoreThan_ReturnsCorrectEmployees()
        {

            var result = _catalog.GetEmployeesWithExperienceMoreThan(4);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(e => e.WorkExperience > 4));
            Assert.AreEqual("Сидоров Сергій Сергійович", result[0].FullName);
            Assert.AreEqual("Іванов Іван Іванович", result[1].FullName);
        }
    }
}
