using Microsoft.AspNetCore.Mvc;

namespace BiiGBackend.Web.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly string _videoDirectory = "./Videos/"; // Change this to your desired path

        [HttpGet("{filename}")]
        public IActionResult GetVideo(string filename)
        {
            var filePath = (_videoDirectory + filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Video not found");
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(stream, "video/mp4", enableRangeProcessing: true);
        }
    }

}
