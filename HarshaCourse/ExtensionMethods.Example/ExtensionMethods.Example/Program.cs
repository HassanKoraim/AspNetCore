using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace ExtensionMethods.Example
{
    internal class Program
    {
        static void Main(string[] arg)
        {
            Apple apple = new Apple();
            Orange orange = new Orange();
            Fruit  fruit = new Fruit();
            CreateFruitDelegate createFruitDelegate = apple.CreateApple;
            createFruitDelegate += orange.CreateOrange;
            //createFruitDelegate();

            AppleJucieFactory appleJucieFactory = new AppleJucieFactory();
            FruitJuicFactory fruitJuicFactory = new FruitJuicFactory(); 
            AppleJucedel appledel = appleJucieFactory.CreateAppleJucie; // normal
            appledel += fruitJuicFactory.CreateFruitJucie;

        }

    }

   // public delegate Apple CreateAppleDelegate(); 
  //  public delegate Orange CreateOrangeDelegate(); 
    public delegate Fruit CreateFruitDelegate();    
    public class Fruit
    {

    }
    public class Apple : Fruit
    {
        public Apple CreateApple()
        {
            Console.WriteLine("Apple was created");
            return new Apple();
        }
    }
    public class Orange : Fruit
    {
        public Orange CreateOrange()
        {
            Console.WriteLine("Orange was created");
            return new Orange();
        }
    }
    public delegate void AppleJucedel(Apple apple);
    public delegate void FruitJucedel(Fruit fruit);
    public class AppleJucieFactory
    {
        public void CreateAppleJucie(Apple apple)
        {
            Console.WriteLine("Create Apple Juic");
        }
    }

    public class FruitJuicFactory
    {
        public void CreateFruitJucie(Fruit fruit)
        {
            Console.WriteLine("Create Fruit Juic");
        }
    }





}
