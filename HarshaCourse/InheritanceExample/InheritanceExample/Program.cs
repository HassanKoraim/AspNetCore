namespace InheritanceExample
{
    public class Program
    {
        static void Main(string[] args)
        {
            SalesMan s = new SalesMan(5, "Hassan" , "Egypt");
            Manager m = new Manager(20, "Mohamed", "Cairo");

            string n = s.GetHealthInsuranceAmount();
            Console.WriteLine(n);
        }
    }
}
