using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;

namespace LINQExample
{
    
    public class Employee
    {

        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Job { get; set; }
        public int Salary { get; set; }
        public void i(Func<Employee, bool> func , Employee employee)
        {
            if (func(employee))
            {

            }
            Console.WriteLine("");
        }
    internal class Program
    {
        static void Main()
        {
            //Collection of Objects
            List<Employee> employees = new List<Employee>()
            {
                new Employee {EmpId = 1, EmpName = "Hassan", Salary = 50000 , Job = "Software Developer"},
                new Employee {EmpId = 2, EmpName = "Ibrahim", Salary = 5658 , Job = "Software"},
                new Employee {EmpId = 3, EmpName = "Hussien", Salary = 10000000 , Job = "Developer"},
                new Employee {EmpId = 4, EmpName = "Mohamed", Salary = 5564454 , Job = "Data Analyst"}
            };
            IOrderedEnumerable<Employee> emps =  employees.OrderBy(emp => emp.EmpName).
                 ThenBy(emp => emp.Salary).ThenBy(emp => emp.Job);
            foreach (Employee emp in emps)
            {
                Console.WriteLine(emp.EmpId + "," + emp.EmpName + "," + emp.Job +',' + emp.Salary);
            }
            Employee emp3 = emps.ElementAt(1);
            emps.ElementAt(0).i(emp2 => emp2.EmpName == "has" , emp3);

               
        }
    }
}
