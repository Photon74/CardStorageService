using CardStorageService.Data;

namespace CardStorageService.Services.Imp
{
    public class ClientRepository : IClientRepositoryService
    {
        private readonly CardStorageServiceDbContext _dbContext;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(CardStorageServiceDbContext dbContext,
                              ILogger<ClientRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public int Create(Client data)
        {
            _dbContext.Clients.Add(data);
            _dbContext.SaveChanges();
            return data.ClientId;
        }

        public int Delete(int id)
        {
            var client = _dbContext.Clients.FirstOrDefault(client => client.ClientId == id);
            if (client == null)
            {
                _logger.LogError("Client not found!");
                throw new Exception("Client not found!");
            }

            _dbContext.Clients.Remove(client);
            return _dbContext.SaveChanges();
        }

        public IList<Client> GetAll()
        {
            var clientsList = _dbContext.Clients.ToList();
            if (clientsList.Count == 0)
            {
                _logger.LogError("There are no clients!");
                throw new Exception("There are no clients!");
            }
            return clientsList;
        }

        public Client GetById(int id)
        {
            var client = _dbContext.Clients.FirstOrDefault(client => client.ClientId == id);
            if (client == null)
            {
                _logger.LogError("Client not found!");
                throw new Exception("Client not found!");
            }

            return client;
        }

        public int Update(Client data)
        {
            var client = _dbContext.Clients.FirstOrDefault(client => client.ClientId == data.ClientId);
            if (client == null)
            {
                _logger.LogError("Client not found!");
                throw new Exception("Client not found!");
            }

            _dbContext.Clients.Update(data);
            return _dbContext.SaveChanges();
        }
    }
}
