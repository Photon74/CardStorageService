using CardStorageService.Data;

namespace CardStorageService.Services.Imp
{
    public class CardRepository : ICardRepositoryService
    {
        private readonly CardStorageServiceDbContext _dbContext;
        private readonly ILogger<CardRepository> _logger;

        public CardRepository(CardStorageServiceDbContext dbContext,
                              ILogger<CardRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public string Create(Card data)
        {
            var client = _dbContext.Clients.FirstOrDefault(client => client.ClientId == data.ClientId);
            if (client == null)
            {
                _logger.LogError("Client not found!");
                throw new Exception("Client not found!");
            }

            _dbContext.Cards.Add(data);
            _dbContext.SaveChanges();
            return data.CardId.ToString();
        }

        public int Delete(string cardId)
        {
            var card = GetById(cardId);
            if (card == null)
            {
                _logger.LogError("Card not found!");
                throw new Exception("Card not found!");
            }

            _dbContext.Cards.Remove(card);
            return _dbContext.SaveChanges();
        }

        public IList<Card> GetAll()
        {
            var cardsList = _dbContext.Cards.ToList();
            if (cardsList.Count == 0)
            {
                _logger.LogError("There are no cards!");
                throw new Exception("There are no cards!");
            }
            return _dbContext.Cards.ToList();
        }

        public IList<Card> GetByClientId(int clientId)
        {
            var client = _dbContext.Clients.FirstOrDefault(client => client.ClientId == clientId);
            if (client == null)
            {
                _logger.LogError("Client not found!");
                throw new Exception("Client not found!");
            }

            return _dbContext.Cards.Where(card => card.ClientId == clientId).ToList();
        }

        public Card GetById(string cardId)
        {
            var card = _dbContext.Cards.FirstOrDefault(card => card.CardId.ToString() == cardId);
            if (card == null)
            {
                _logger.LogError("Card not found!");
                throw new Exception("Card not found!");
            }

            return card;
        }

        public int Update(Card data)
        {
            var card = _dbContext.Cards.FirstOrDefault(card => card.CardId == data.CardId);
            if (card == null)
            {
                _logger.LogError("Card not found!");
                throw new Exception("Card not found!");
            }

            _dbContext.Cards.Update(data);
            return _dbContext.SaveChanges();
        }
    }
}
