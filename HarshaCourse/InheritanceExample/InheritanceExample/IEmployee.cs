using System;

namespace InheritanceExample
{
    public interface IEmployee
    {
        int EmpID {  get; set; }
        string EmpName { get; set; }
        string Location { get; set; }

       public string GetHealthInsuranceAmount();

    }
}
