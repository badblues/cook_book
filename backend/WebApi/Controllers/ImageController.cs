using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("images")]
public class ImageController : ControllerBase
{
    public ImageController() { }

    [HttpGet("{*path}")]
    public IActionResult GetImage(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return BadRequest("Invalid image path.");
        }
        try
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "image/jpeg");
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
}
