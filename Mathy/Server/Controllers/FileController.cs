using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mathy.Server.Services;
using Mathy.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mathy.Server.Controllers
{
    [ApiController]
    [Route("File/")]
    public class FileController : BaseController
    {
        private readonly FileService fileService;
        public FileController(FileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpPost("Upload")]
        public IActionResult UploadFile([FromForm]IFormCollection formData)
        {
            var file = formData.Files[0];
            var filename = fileService.AddFile(file.OpenReadStream(), "." + file.FileName.Split('.').Last());
            return Json(new { errno = "0", data = new List<string> { "File/" + filename } });
        }

        [HttpGet("{fileName}")]
        public Task<FileStreamResult> Index(string fileName)
        {
            var lf = fileService.GetFiles(new List<string> { fileName }).First();
            var file = File(System.IO.File.OpenRead(lf.LocalPath), MimeMapping.MimeUtility.GetMimeMapping(lf.FileType));
            return Task.FromResult(file);
        }

    }
}
