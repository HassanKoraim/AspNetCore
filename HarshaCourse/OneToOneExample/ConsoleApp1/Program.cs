using Collage;

namespace OneToOneExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Student Class's Object
            Student student = new Student();
            student.StudentName = "Hassan";
            student.RollN = 15;
            student.Email = "HassanKoraim2@gmail.com";

            // Branch Class's Object

            Branch branch = new Branch();
            branch.BranchName = "Computer Science";
            branch.NoOfSemesters = 8;

            // One-to-One Relation
            student.branch = branch;

        }
    }
}
