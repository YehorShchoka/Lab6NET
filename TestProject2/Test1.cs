using Lab6new;

namespace TestProject2
{
    [TestClass]
    public class EmployeeTests
    {
        [TestMethod]
        public void Constructor_WithValidArguments_CreatesEmployeeObject()
        {

            string fullName = "Іванов Іван Іванович";
            string position = "Інженер";
            decimal salary = 25000;
            int workExperience = 5;

            var employee = new Employee(fullName, position, salary, workExperience);

            Assert.AreEqual(fullName, employee.FullName);
            Assert.AreEqual(position, employee.Position);
            Assert.AreEqual(salary, employee.Salary);
            Assert.AreEqual(workExperience, employee.WorkExperience);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void Constructor_WithInvalidFullName_ThrowsArgumentException(string invalidFullName)
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Employee(invalidFullName, "Інженер", 25000, 5));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void Constructor_WithInvalidPosition_ThrowsArgumentException(string invalidPosition)
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Employee("Іванов І.І.", invalidPosition, 25000, 5));
        }

        [TestMethod]
        public void Constructor_WithNegativeSalary_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Employee("Іванов І.І.", "Інженер", -1000, 5));
        }

        [TestMethod]
        public void Constructor_WithNegativeWorkExperience_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Employee("Іванов І.І.", "Інженер", 25000, -1));
        }

        [TestMethod]
        public void CompareTo_WithOtherEmployee_ReturnsCorrectValue()
        {
            var employee1 = new Employee("Іванов І.І.", "Інженер", 25000, 5);
            var employee2 = new Employee("Петров П.П.", "Менеджер", 30000, 3);
            var employee3 = new Employee("Сидоров С.С.", "Аналітик", 28000, 5);

            Assert.IsTrue(employee1.CompareTo(employee2) > 0, "Employee1 should be greater than Employee2 by work experience");
            Assert.IsTrue(employee2.CompareTo(employee1) < 0, "Employee2 should be less than Employee1 by work experience");
            Assert.AreEqual(0, employee1.CompareTo(employee3), "Employee1 and Employee3 should be equal by work experience");
        }

        [TestMethod]
        public void CompareTo_WithNull_ReturnsPositiveValue()
        {
            var employee = new Employee("Іванов І.І.", "Інженер", 25000, 5);
            Assert.IsTrue(employee.CompareTo(null) > 0, "Comparing to null should return positive value.");
        }

        [TestMethod]
        public void ToString_ReturnsCorrectlyFormattedString()
        {
            var employee = new Employee("Іванов Іван Іванович", "Інженер", 25000, 5);
            string expectedString = "ПІБ: Іванов Іван Іванович, Посада: Інженер, Зарплата: 25 000.00 ₴, Стаж: 5 років";
            string actualString = employee.ToString();
            Assert.AreEqual(expectedString, actualString);
        }
    }
}
