namespace Patterns
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Singleton

            var obj1 = SimpleSingleton.Instance;
            var obj2 = SimpleSingleton.Instance;
            if (obj1 == obj2) Console.WriteLine("True");

            #endregion
        }
    }

    class SimpleSingleton
    {
        private static SimpleSingleton instance;

        private SimpleSingleton() {}

        public static SimpleSingleton Instance { 
            get 
            {
                instance ??= new SimpleSingleton();
                return instance;
            } 
        }
    }
}