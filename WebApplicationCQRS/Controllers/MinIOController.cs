using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using System;
using System.Threading.Tasks;
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
    [HttpPost("presigned-url")]
    public async Task<IActionResult> GetPresignedUrl([FromForm] string FileName)
    {
        if (string.IsNullOrEmpty(FileName))
        {
            return BadRequest(new { error = "FileName is required" });
        }

        var bucketName = _configuration["Minio:BucketName"];
        var expiry = int.Parse(_configuration["Minio:Expiry"] ?? "3600");

        var args = new PresignedPutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(FileName)
            .WithExpiry(expiry);

        var url = await _minioClient.PresignedPutObjectAsync(args);

        return Ok(new { presignedUrl = url });
    }

    
}