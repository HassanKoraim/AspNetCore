using ECommerce;
using System.Collections.Generic;

namespace CollectionofObjectsExample
{
    /// <summary>
    /// This example from Harsha Course on Collection of objects
    /// https://www.udemy.com/course/asp-net-core-true-ultimate-guide-real-project/learn/lecture/34531948#overview
    /// </summary>
    public class Program
    {
        static void Main()
        {
            // Create an empty Collection
            List<Product> products = new List<Product>();

            // loop to read data from user 

            string choose;
            do
            {
                Console.WriteLine("Enter Product Id");
                int PId = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter Product Name");
                string PName = Console.ReadLine();
                Console.WriteLine("Enter Product Price");
                double Price = double.Parse(Console.ReadLine()); 
                Console.WriteLine("Enter Product Date Time");
                DateTime DoM = DateTime.Parse(Console.ReadLine());
               
                // Create a new object of Product class
                Product product = new Product() { ProductId = PId, ProductName = PName,
                    Price = Price , DateOfManufacture = DoM};
                // Add Product object to collection
                products.Add(product);
                Console.WriteLine("Product Added.\n");

                // Ask the User to continue
                Console.WriteLine("Do You want to continue to next product? (Yes/No) ");
                choose = Console.ReadLine();
            } while (choose != "No" && choose != "no" && choose != "n" && choose != "N");

            //Print the Products
            Console.WriteLine("\nProducts\n");
            foreach (Product product in products)
            {
                Console.WriteLine(product.ProductId + "," + product.ProductName +"," +
                    product.Price + "," + product.DateOfManufacture.ToShortDateString());
            }
        }
    }
}
