using System;

namespace InheritanceExample
{
    public class SalesMan : IEmployee
    {
        public SalesMan(int empId, string empName, string Location) {
            Console.WriteLine("This is instance From (SalesMan) class");
            this._empId = empId;
            this._empName = empName;
            this._location = Location;
        }
        private int _empId;
        private string _empName;
        private string _location;
        public int EmpID
        {
            get => _empId;
            set { if (value >= 500 && value <= 1000) _empId = value; }
        }
        public string EmpName { get => _empName; set => _empName = value; }
        public string Location { get => _location; set => _location = value; }

        public string GetHealthInsuranceAmount()
        {
            return "Addtional Health Insurance is 500";
        }
    }
}
