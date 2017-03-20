using System.Threading.Tasks;
using API.Features.Theme;
using API.Features.Theme.ADD;
using API.Features.Theme.GET;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ThemeController : Controller
    {
        private readonly IMediator _mediator;

        public ThemeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetThemes")]
        public async Task<IActionResult> GetAllThemes()
        {
            var mdl = await _mediator.SendAsync(new GetThemes.Request());
            if (mdl != null)
            {
                return Ok(mdl);
            }

            return NotFound();
        }
        
        [HttpGet("GetTheme/{themeId}")]
        public async Task<IActionResult> GetThemeById(GetTheme.Request request)
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

        [HttpGet("GetThemesByUser/{UserName}")]
        public async Task<IActionResult> GetThemesByUser(GetThemesByUser.Request request)
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

        /*
        [HttpGet("GetThemesUserParticipates/{UserName}")]
        public async Task<IActionResult> GetThemesUserParticipates(GetThemesUserParticipates.Request request)
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

        [HttpGet("CloneTheme/{FromThemeId}/{ToThemeId}")]
        public async Task<IActionResult> CloneThemeByIds(CloneTheme.Request request)
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

        

        [HttpGet("GetThemeModerator/{UserName}/{ThemeId}")]
        public async Task<IActionResult> GetThemeModerator(GetThemeModerator.Request request)
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

        */

        [HttpPost]
        [Route("AddTheme")]
        public async Task<IActionResult> AddCard([FromBody]AddTheme.Request request)
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
        [Route("UpdateTheme")]
        public async Task<IActionResult> UpdateTheme([FromBody]UpdateTheme.Request request)
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

        [HttpPut("DisableTheme/{ThemeId}")]
        public async Task<IActionResult> DisableTheme(DisableTheme.Request request)
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

        [HttpGet]
        [Route("PublicThemes")]
        public async Task<IActionResult> GetPublicTheme()
        {
            try
            {
                var res = await _mediator.SendAsync(new GetPublicTheme.Request());
                if(res == null) return BadRequest();
                return Ok(res);
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }
    }
}