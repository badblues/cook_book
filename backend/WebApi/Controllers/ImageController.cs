namespace CookBook.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.IO;

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
            Byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "image/jpeg");
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
}
