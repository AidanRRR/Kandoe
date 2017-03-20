using System.Threading.Tasks;
using API.Features.User;
using API.Features.User.GET;
using DAL.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> userLogger)
        {
            _mediator = mediator;
            _logger = userLogger;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Getting all users");
            var mdl = await _mediator.SendAsync(new GetUsers.Request());
            if (mdl != null)
            {
                return Ok(mdl);
            }

            return NotFound();
        }

        // Get niet goed hier, urls hebben een max. grote van 2,083 karakters
        [HttpPost]
        [Route("GetUsersByIds")]
        public async Task<IActionResult> GetAllUsersByIds([FromBody]GetUsersByIds.Request request)
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

        [HttpGet("GetUserById/{UserName}")]
        public async Task<IActionResult> GetUserById(GetUser.Request request)
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

        [HttpGet("GetUserByEmail/{Email}")]
        public async Task<IActionResult> GetUserById(GetUserByEmail.Request request)
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

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody]AddUser.Request request)
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
        
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody]UpdateUser.Request request)
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

        [HttpPut("DisableUser/{UserName}")]
        public async Task<IActionResult> DisableUser(DisableUser.Request request)
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