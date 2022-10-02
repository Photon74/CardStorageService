using CardStorageService.Data;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientRepositoryService _clientRepositoryService;
        private readonly IValidator<CreateClientRequest> _createClientValidator;

        public ClientController(ILogger<ClientController> logger,
                                IClientRepositoryService clientRepositoryService,
                                IValidator<CreateClientRequest> createClientValidator)
        {
            _logger = logger;
            _clientRepositoryService = clientRepositoryService;
            _createClientValidator = createClientValidator;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(CreateClientResponse), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] CreateClientRequest request)
        {
            var validationResult = _createClientValidator.Validate(request);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

            try
            {
                var clientId = _clientRepositoryService.Create(new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Patronymic = request.Patronymic
                });
                return Ok(new CreateClientResponse
                {
                    ClientId = clientId
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Create client error!");
                return Ok(new CreateClientResponse
                {
                    ErrorCode = 912,
                    ErrorMessage = "Create client error!"
                });
            }
        }
    }
}
