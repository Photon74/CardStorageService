using AutoMapper;
using CardStorageService.Protos;
using Grpc.Core;
using static CardStorageService.Protos.CardService;

namespace CardStorageService.Services.Imp
{
    public class CardService : CardServiceBase
    {
        private readonly ICardRepositoryService _cardRepositoryService;
        private readonly IMapper _mapper;

        public CardService(ICardRepositoryService cardRepositoryService, IMapper mapper)
        {
            _cardRepositoryService = cardRepositoryService;
            _mapper = mapper;
        }

        public override Task<GetByClientIdResponse> GetByClientId(GetByClientIdRequest request, ServerCallContext context)
        {
            var response = new GetByClientIdResponse();
            var cards = _cardRepositoryService.GetByClientId(request.ClientId);

            response.Cards.AddRange(cards.Select(card => new Card
            {
                CardNo = card.CardNo,
                Name = card.Name,
                ExpDate = card.ExpDate.ToShortDateString(),
                CVV2 = card.CVV2
            }).ToList());

            return Task.FromResult(response);
        }
    }
}
