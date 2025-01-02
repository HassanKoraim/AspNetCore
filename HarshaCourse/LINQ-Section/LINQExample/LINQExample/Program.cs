using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;

namespace LINQExample
{
    public class Employee
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Job { get; set; }
        public string City { get; set; }
       
    }
    internal class Program
    {
        static void Main()
        {
             
            //Collection of Objects
            List<Employee> employees = new List<Employee>()
            {
                new Employee {EmpId = 1, EmpName = "Hassan", City = "Qena" , Job = "Software Developer"},
                new Employee {EmpId = 2, EmpName = "Ibrahim", City = "Cairo" , Job = "Software Developer"},
                new Employee {EmpId = 3, EmpName = "Hussien", City = "New York" , Job = "Developer"},
                new Employee {EmpId = 4, EmpName = "Mohamed", City = "Sohag" , Job = "Data Analyst"}
            };
           List<Employee> emps = employees.Where(temp => temp.City == "Qena").ToList();
            foreach (Employee emp in emps) {
                Console.WriteLine(emp.EmpName);
            }
           

            Employee employee = employees.First(emp => emp.Job == "Software Developer");
            Console.WriteLine(employee.EmpName);
        }
    }
}
