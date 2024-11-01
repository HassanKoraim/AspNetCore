using System;

namespace ExtensionMethods.Example
{
    public static class ProductExtension
    {
        public static double GetDiscount(this Product product)
        {
            return product.ProductCost * product.DiscountPrecentage / 100 ;
        }
    }
}
