using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Features.Events;
using API.Features.Sessions;
using DAL.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.Models.Events;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class SessionEventsController : Controller
    {
        private readonly ISessionEventsRepository _sessionEventsRepository;
        private readonly IMediator _mediator;

        public SessionEventsController(ISessionEventsRepository sessionEventsRepository, IMediator mediator)
        {
            _sessionEventsRepository = sessionEventsRepository;
            _mediator = mediator;
        }
  
        [HttpPost]
        [Route("AddMoveEvent")]
        public async Task<IActionResult> AddMoveEvent([FromBody] AddMoveEvent.Request request)
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
        [Route("AddChatMessageEvent")]
        public async Task<IActionResult> AddChatMessageEvent([FromBody] AddChatMessageEvent.Request request)
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
        [Route("AddConnectEvent")]
        public async Task<IActionResult> AddConnectEvent([FromBody] AddConnectEvent.Request request)
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
        [Route("AddTurnStartEvent")]
        public async Task<IActionResult> AddTurnStartEvent([FromBody] AddTurnStartEvent.Request request)
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
    }
}