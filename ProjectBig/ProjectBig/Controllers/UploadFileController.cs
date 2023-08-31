using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjectBig.Models;

namespace WebAppThird.Controllers
{
    public class UploadFileController : Controller
    {
        private static IWebHostEnvironment _webHostEnvironment;
        public UploadFileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] FileUpload filemodel)
        {

            try
            {
                if (filemodel.File.Length > 0)
                {
                    var path = _webHostEnvironment.WebRootPath + "images\\UserProfile\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filename = Guid.NewGuid().ToString() + filemodel.File.FileName;
                    using (FileStream filestream = System.IO.File.Create(path + filename))
                    {
                        filemodel.File.CopyTo(filestream);
                        filestream.Flush();
                        return Ok(filename);
                    }
                }
                else
                {
                    return BadRequest("Failed");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message);

            }

        }







    }
}
