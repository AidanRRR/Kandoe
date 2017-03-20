using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;
using kandoe_service_image.Services;

namespace kandoe_service_image.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly IImageSender _imageSender;
        public ImageController(IImageSender imageSender)
        {
            _imageSender = imageSender;
        }

        [HttpGet]
        [Route("GetAllImages")]
        public IActionResult GetAllImages()
        {
            return Ok("");
        }

        [HttpPost]
        [Microsoft.AspNetCore.Cors.EnableCors("CorsPolicy")]
        [Route("PostImage")]
        public async Task<IActionResult> PostImage(IFormFile file)
        {            
            var result = await _imageSender.PostImage(file);
            return Ok(result);
        }
    }
}
