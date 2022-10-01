namespace Patterns
{
    internal class Sample03
    {
        static void Main()
        {
            var pr1 = ProductFactory.Create<SampleProduct01>();
            var pr2 = ProductFactory.Create<SampleProduct02>();

            Console.ReadKey(true);
        }
    }

    public abstract class Product
    {
        public abstract void PostConstruction();
    }

    public class SampleProduct01 : Product
    {
        public SampleProduct01() { }

        public SampleProduct01(int a, int b)
        {
        }

        public override void PostConstruction()
        {
            Console.WriteLine("Product01 created");
        }
    }

    public class SampleProduct02 : Product
    {
        public SampleProduct02() { }

        public SampleProduct02(string str)
        {
        }

        public override void PostConstruction()
        {
            Console.WriteLine("Product02 created");
        }
    }

    public static class ProductFactory
    {
        public static T Create<T>() where T : Product, new()
        {
            var t = new T();
            t.PostConstruction();
            return t;
        }
    }
}
