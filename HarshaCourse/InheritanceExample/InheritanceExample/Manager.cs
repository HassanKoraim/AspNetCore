using System;

namespace InheritanceExample
{
    public class Manager : IEmployee
    {
        public Manager(int empId , string empName, string location)
        {
            Console.WriteLine("This is instance From (Manager) class");
            this._empId = empId ;
            this._empName = empName ;
            this._location = location ;
        }
        private int _empId;
        private string _empName;
        private string _location;
        public int EmpID { get => _empId;
            set{ if (value >= 1000 && value <= 2000) _empId = value; } }
        public string EmpName { get => _empName; set => _empName = value; }
        public string Location { get => _location; set => _location = value; }

        public string GetHealthInsuranceAmount()
        {
            return "Addtional Health Insurance is 1000";
        }
    }
}
