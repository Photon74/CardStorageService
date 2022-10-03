using AutoMapper;
using CardStorageService.Data;
using CardStorageService.Models;
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
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly ICardRepositoryService _cardRepositoryService;
        private readonly IValidator<CreateCardRequest> _createCardValidator;
        private readonly IMapper _mapper;

        public CardController(ILogger<CardController> logger,
                              ICardRepositoryService cardRepositoryService,
                              IValidator<CreateCardRequest> createCardValidator,
                              IMapper mapper)
        {
            _logger = logger;
            _cardRepositoryService = cardRepositoryService;
            _createCardValidator = createCardValidator;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] CreateCardRequest request)
        {
            var validationResult = _createCardValidator.Validate(request);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

            try
            {
                var cardId = _cardRepositoryService.Create(_mapper.Map<Card>(request));

                _logger.LogInformation("New card created!");
                return Ok(new CreateCardResponse
                {
                    CardId = cardId.ToString(),
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Create card error!");
                return Ok(new CreateCardResponse
                {
                    ErrorCode = 1012,
                    ErrorMessage = "Create card error!"
                });
            }
        }

        [HttpGet("GetByClientId")]
        [ProducesResponseType(typeof(GetCardsResponse), StatusCodes.Status200OK)]
        public IActionResult GetByClientId([FromQuery] int clientId)
        {
            try
            {
                var cards = _cardRepositoryService.GetByClientId(clientId);
                return Ok(new GetCardsResponse
                {
                    Cards = _mapper.Map<List<CardDto>>(cards)
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get cards error!");
                return Ok(new GetCardsResponse
                {
                    ErrorCode = 1013,
                    ErrorMessage = "Get cards error!"
                });
            }
        }
    }
}
