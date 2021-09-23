using demo_file_manager.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using demo_file_manager.Server.Services;
using System.Text.Json;

namespace demo_file_manager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IFileManager _fileManager;

        public DocumentController(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> GetFile([FromBody] GetFileModel model)
        {
            var result = await _fileManager.GetDataAsBytes(model.FilePath);
            _ = new FileExtensionContentTypeProvider().TryGetContentType(model.FilePath, out string contentType);
            Response.Headers.ContentDisposition = "attachment";
            return File(result, contentType ?? MediaTypeNames.Application.Octet, Path.GetFileName(model.FilePath));
        }

        [Route("{fileName}")]
        [HttpGet]
        public async Task<ActionResult> GetFileByName(string fileName)
        {
            var result = await _fileManager.GetDataAsBytes(fileName);
            _ = new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);
            Response.Headers.ContentDisposition = "attachment";
            return File(result, contentType ?? MediaTypeNames.Application.Octet, Path.GetFileName(fileName));
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult> GetFile()
        {
            var model = JsonSerializer.Deserialize<GetFileModel>(Request.Headers["model"]);
            var result = await _fileManager.GetDataAsBytes(model.FilePath);
            _ = new FileExtensionContentTypeProvider().TryGetContentType(model.FilePath, out string contentType);
            Response.Headers.ContentDisposition = "attachment";
            return File(result, contentType ?? MediaTypeNames.Application.Octet, Path.GetFileName(model.FilePath));
        }
    }
}
