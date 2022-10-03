using AutoMapper;
using CardStorageService.Data;
using CardStorageService.Protos;
using Grpc.Core;
using static CardStorageService.Protos.ClientService;

namespace CardStorageService.Services.Imp
{
    public class ClientService : ClientServiceBase
    {
        private readonly IClientRepositoryService _clientRepositoryService;
        private readonly IMapper _mapper;

        public ClientService(IClientRepositoryService clientRepositoryService, IMapper mapper)
        {
            _clientRepositoryService = clientRepositoryService;
            _mapper = mapper;
        }

        public override Task<CreateClientRespose> Create(CreateClientRequest request, ServerCallContext context)
        {
            var ClientId = _clientRepositoryService.Create(_mapper.Map<Client>(request));

            return Task.FromResult(new CreateClientRespose { ClientId = ClientId });
        }
    }
}
