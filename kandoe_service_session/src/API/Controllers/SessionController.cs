using System.Threading.Tasks;
using API.Features.Sessions;
using DAL.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class SessionController : Controller
    {
        private readonly ISessionEventsRepository _sessionRepository;
        private readonly IMediator _mediator;

        public SessionController(ISessionEventsRepository eventsRepository, IMediator mediator)
        {
            _sessionRepository = eventsRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSessions()
        {
            var mdl = await _mediator.SendAsync(new GetSessions.Request());
            if (mdl != null)
            {
                return Ok(mdl);
            }

            return NotFound();
        }

        public class ReplayModel
        {
            public Guid SessionId { get; set; }
            public Guid? ReplayKey { get; set; }
        }

        [HttpGet("GetReplay/{SessionId}/{ReplayKey?}")]
        public async Task<IActionResult> GetReplayWithReplayKey([FromHeader(Name = "X-Access-Token")] string token, [FromRoute(Name="SessionId")] Guid sessionId, [FromRoute(Name="ReplayKey")] Guid replayKey)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }

                var result = await _mediator.SendAsync(new GetReplay.Request()
                {
                    Token = token,
                    SessionId = sessionId,
                    ReplayKey = replayKey
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet("GetSessionsUserParticipates/{UserName}")]
        public async Task<IActionResult> GetSessionsUserParticipates(GetSessionsUserParticipates.Request request) {
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

        [HttpGet("GetSessionById/{sessionId}", Name = "GetSessionById")]
        public async Task<IActionResult> GetSessionById(GetSession.Request request)
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

        [HttpGet("StartSession/{sessionId}", Name = "StartSession")]
        public async Task<IActionResult> StartSession([FromHeader(Name = "X-Access-Token")] string token, [FromRoute(Name = "sessionId")] Guid sessionId)
        {
            var result = await _mediator.SendAsync(new StartSession.Request()
            {
                Token = token,
                SessionId = sessionId
            });
            return Ok(result);
        }

        [HttpGet("GetSessionsByTheme/{themeId}", Name = "GetSessionsByTheme")]
        public async Task<IActionResult> GetSessionsByTheme(GetThemeSessions.Request request)
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
        
        [HttpGet("GetParticipatingSessions", Name = "GetParticipatingSessions")]
        public async Task<IActionResult> GetParticipatingSessions([FromHeader(Name = "X-Access-Token")] string token)
        {
            var result = await _mediator.SendAsync(new GetParticipatingSessions.Request()
            {
                Token = token
            });

            if (result.HasErrors)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }


        [HttpGet("GetInvitedSessions", Name = "GetInvitedSessions")]
        public async Task<IActionResult> GetInvitedSessions([FromHeader(Name = "X-Access-Token")] string token)
        {
            var result = await _mediator.SendAsync(new GetOpenInvitations.Request()
            {
                Token = token
            });

            if (result.HasErrors)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        
        [HttpGet("AcceptSessionInvite/{sessionId}", Name = "AcceptSessionInvite")]
        public async Task<IActionResult> AcceptInvite(Guid sessionId, [FromHeader(Name = "X-Access-Token")] string token)
        {
            try
            {
                var result = await _mediator.SendAsync(new AcceptSessionInvite.Request()
                {
                    SessionId = sessionId,
                    Token = token 
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("AddSession")]
        public async Task<IActionResult> AddSession([FromHeader(Name = "X-Access-Token")] string token, [FromBody] AddSession.Request request)
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

                request.Token = token;
                var result = await _mediator.SendAsync(request);
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }

        public class InviteUserModel
        {
            public string Email { get; set; }
            public Guid SessionId {get; set;}
        }
        [HttpPost]
        [Route("InviteUsersSession")]
        public async Task<IActionResult> InviteUsersSession([FromBody] InviteUserModel body, [FromHeader(Name = "X-Access-Token")] string token)
        {
            try
            {
                if (body == null)
                {
                    return BadRequest("Geen geldige body");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Console.WriteLine("Invite token: " + token);
                var result = await _mediator.SendAsync(new InviteUsersSession.Request()
                {
                    Email = body.Email,
                    SessionId = body.SessionId,
                    Token = token 
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
    }
}