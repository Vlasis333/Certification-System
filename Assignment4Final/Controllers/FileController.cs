using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO.Questions;

namespace Assignment4Final.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file received");

        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string hash = ComputeHashWithExtension(file);
        string filePath = Path.Combine(folderPath, hash);

        string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/api/File/image";

        // NOTE:(akotro) Check if the file already exists
        if (System.IO.File.Exists(filePath))
        {
            var uploadedFile = new FileDto
            {
                FileName = hash,
                // FilePath = filePath,
                Url = $"{baseUrl}/{hash}"
            };
            return Ok(uploadedFile);
        }

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        var newFile = new FileDto
        {
            FileName = hash,
            // FilePath = filePath,
            Url = $"{baseUrl}/{hash}"
        };

        return Ok(newFile);
    }

    private string ComputeHashWithExtension(IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(stream);
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            string fileExtension = Path.GetExtension(file.FileName);
            return hash + fileExtension;
        }
    }

    [HttpGet("download/{fileName}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public IActionResult DownloadFile(string fileName)
    {
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        string filePath = Path.Combine(folderPath, fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var stream = new FileStream(filePath, FileMode.Open);
        return File(stream, "application/octet-stream", fileName);
    }

    [HttpGet("image/{fileName}")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAny")]
    public IActionResult GetImage(string fileName)
    {
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        string filePath = Path.Combine(folderPath, fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var imageBytes = System.IO.File.ReadAllBytes(filePath);
        var contentType = GetContentType(filePath);

        return File(imageBytes, contentType);
    }

    private string GetContentType(string filePath)
    {
        string contentType;
        switch (Path.GetExtension(filePath).ToLower())
        {
            case ".jpg":
            case ".jpeg":
                contentType = "image/jpeg";
                break;
            case ".png":
                contentType = "image/png";
                break;
            case ".gif":
                contentType = "image/gif";
                break;
            default:
                contentType = "application/octet-stream";
                break;
        }

        return contentType;
    }
}
