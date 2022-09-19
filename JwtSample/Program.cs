namespace JwtSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя пользователя:");
            string userName = Console.ReadLine();

            Console.WriteLine("Введите пароль:");
            string password = Console.ReadLine();

            UserService userService = new UserService();
            string token = userService.Authenticate(userName, password);

            Console.WriteLine(token);
            Console.ReadLine();
        }
    }
}