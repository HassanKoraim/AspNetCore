using System;

namespace ECommerce
{
    /// <summary>
    /// Represent a Product in Ecommerce Application
    /// </summary>
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public DateTime DateOfManufacture {  get; set; }
    }
}
