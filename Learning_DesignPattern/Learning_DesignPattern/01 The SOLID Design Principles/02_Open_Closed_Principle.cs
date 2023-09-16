using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Learning_DesignPattern._01_The_SOLID_Design_Principles
{
    /*
        Some product I have and I want to Search to them by 
        Color and Size, and Change requirement Color & Size both
        
        With out change the class method we are Extand by other way 
        That is Open for Exntand but close for modification for main class
     
        We can not go back our mail Filter class for modification for extand
        We have to do other way for Extand

        that is Open and close principle 
        
        Answer is Inheritance How??
        1) make interface 
        2) Inheritance them and extand functionlaity 

     */
    public enum Color
    {
        Red,Green, Blue
    }
    public enum Size
    {
        Small, Medium, Large
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

       public Product(string name, Color color, Size size)
       {
            Name = name;
            Color = color;
            Size = size;
       }
    }

    // 1) Now you boss to say filter product by Size
    public class ProductFilter
    {
        public  IEnumerable<Product> FilterBySize(
            IEnumerable<Product> products, Size size)
        { 
            foreach(var p in products)
            {
                if(p.Size == size) yield return p;
            }
        }

        // 2) We have to extand functionality to findout Color Filter 
        public  IEnumerable<Product> FilterByColor(
           IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p.Color == color) yield return p;
            }
        }

        // 3) Now your boss come and say user can search by Color & Size both
        // Extand the functionality 
        public IEnumerable<Product> FilterByColorAndSize(
           IEnumerable<Product> products, Color color, Size size)
        {
            foreach (var p in products)
            {
                if (p.Color == color && p.Size == size) yield return p;
            }
        }
    }

    // Now go for acutal Open close principle 
    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Color== color;
        }
    }
    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;
        public SizeSpecification(Size size)
        {
            this.size = size;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Size== size;
        }
    }

    public class ColorAndSizeSpecification<T>: ISpecification<T>
    {
        private ISpecification<T> first, second;
        
        public ColorAndSizeSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first;
            this.second = second;
        }
        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }
    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> products, ISpecification<Product> spec)
        {
            foreach(var i in products)
            {
                if(spec.IsSatisfied(i)) yield return i;
            }
        }
    }
    public class _02_Open_Closed_Principle
    {
        public static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Small);

            Product[] products= {apple, tree, house};

            var pf = new ProductFilter();

            Console.WriteLine("Green Products (old):"); 
            foreach(var p in pf.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($" - {p.Name} is green");
            }


            // use th better filter 
            var bf = new BetterFilter();
            Console.WriteLine("Green Products");
            foreach(var p in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($" - {p.Name}");
            }

            Console.WriteLine("Large Blue Item");
            foreach(var p in bf.Filter(products, new ColorAndSizeSpecification<Product>(
                    new ColorSpecification(Color.Green),
                    new SizeSpecification(Size.Large)
                )))
            {
                Console.WriteLine($"- {p.Name}");
            }
        }
    }
}
