using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Microsoft.AspNetCore.Authorization;

namespace WebApplicationCQRS.Controllers;

[ApiController]
[Route("api/minio")]
public class MinIOController : ControllerBase
{
    private readonly IMinioClient _minioClient;
    private readonly IConfiguration _configuration;

    public MinIOController(IMinioClient minioClient, IConfiguration configuration)
    {
        _minioClient = minioClient;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("presigned-urls")]
    public async Task<IActionResult> GetPresignedUrls([FromBody] List<string> fileNames)
    {
        if (fileNames == null || fileNames.Count == 0)
        {
            return BadRequest(new { error = "FileNames are required" });
        }

        var bucketName = _configuration["Minio:BucketName"];
        var expiry = int.Parse(_configuration["Minio:Expiry"] ?? "3600");

        var urls = new Dictionary<string, string>();

        foreach (var fileName in fileNames)
        {
            var args = new PresignedPutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithExpiry(expiry);

            var url = await _minioClient.PresignedPutObjectAsync(args);
            urls[fileName] = url;
        }

        return Ok(new { presignedUrls = urls });
    }

    
}