using CardStorageService.Protos;
using Grpc.Net.Client;
using static CardStorageService.Protos.CardService;
using static CardStorageService.Protos.ClientService;

namespace CardStorageClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketHttpHandler.Http2UnencryptedSupport", true);

            //CardServiceClient
            //ClientServiceClient

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            ClientServiceClient clientService = new(channel);

            var createClientResponse = clientService.Create(new CreateClientRequest
            {
                FirstName = "Алекс",
                LastName = "Колодин",
                Patronymic = "Валериевич",
            });
            Console.WriteLine($"Клиент №{createClientResponse.ClientId} создан.");

            CardServiceClient cardService = new(channel);
            var cardGetByClientIdResponse = cardService.GetByClientId(new GetByClientIdRequest { ClientId = 1 });
            foreach(var card in cardGetByClientIdResponse.Cards)
            {
                Console.WriteLine(
                    $"Номер карты: {card.CardNo}\n" +
                    $"Держатель: {card.Name}\n" +
                    $"Дата: {card.ExpDate}\n" +
                    $"CVV2: {card.CVV2}\n\n");
            }
            Console.ReadKey();
        }
    }
}