using System;
using System.Threading.Tasks;
using API.Features.Card;
using API.Features.Theme;
using DAL.Repositories.Cards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class CardController : Controller
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMediator _mediator;

        public CardController(ICardRepository cardRepository, IMediator mediator)
        {
            _cardRepository = cardRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var mdl = await _mediator.SendAsync(new GetCards.Request());
            if (mdl != null)
            {
                return Ok(mdl);
            }

            return NotFound();
        }

        
        [HttpGet("{cardId}", Name = "GetCardById")]
        public async Task<IActionResult> GetCardsById(GetCard.Request request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _mediator.SendAsync(request);
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        [Route("AddCard")]
        public async Task<IActionResult> AddCard([FromBody]AddCard.Request request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _mediator.SendAsync(request);
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("AddCards")]
        public async Task<IActionResult> AddCards([FromBody]AddCards.Request request){
            try
            {
                if(request == null) return BadRequest("Request is null");
                if(!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _mediator.SendAsync(request);
                return Ok(result);
            }
            catch (Exception)
            {
                
                return NotFound();
            }
        }

        [HttpPost]
        [Route("UpdateCard")]
        public async Task<IActionResult> UpdateCard([FromBody]UpdateCard.Request request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _mediator.SendAsync(request);
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPut("DisableCard/{CardId}")]
        public async Task<IActionResult> DisableUser(DisableCard.Request request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _mediator.SendAsync(request);
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpPost("AddReaction")]
        public async Task<IActionResult> AddReaction([FromBody]AddReaction.Request req){
            try
            {
                if(req == null) return BadRequest("Request is empty");
                if(!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _mediator.SendAsync(req);
                return Ok(result);
            }
            catch (System.Exception)
            { 
                return NotFound();
            }
        }
    }
}
