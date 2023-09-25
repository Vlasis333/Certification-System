using Assignment4Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO;
using ModelLibrary.Models.DTO.Candidates;

namespace Assignment4Final.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhotoIdTypesController : ControllerBase
{
    private readonly PhotoIdTypesService _photoIdTypesService;

    public PhotoIdTypesController(PhotoIdTypesService photoIdTypesService)
    {
        _photoIdTypesService = photoIdTypesService;
    }

    [HttpGet]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = "IsAdminOrQualityControlOrCandidate"
    )]
    public async Task<IActionResult> GetAll()
    {
        var photoIdTypes = await _photoIdTypesService.GetAllAsync();
        var response = new BaseResponse<List<PhotoIdTypeDto>>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = photoIdTypes
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Get(int id)
    {
        var photoIdType = await _photoIdTypesService.GetAsync(id);
        if (photoIdType == null)
        {
            return NotFound(
                new BaseResponse<PhotoIdTypeDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"PhotoIdType with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<PhotoIdTypeDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = photoIdType
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Add([FromBody] PhotoIdTypeDto photoIdTypeDto)
    {
        var addedPhotoIdType = await _photoIdTypesService.AddAsync(photoIdTypeDto);
        if (addedPhotoIdType == null)
        {
            return BadRequest(
                new BaseResponse<PhotoIdTypeDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = "Failed to add photoIdType."
                }
            );
        }

        var response = new BaseResponse<PhotoIdTypeDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = addedPhotoIdType
        };

        return CreatedAtAction(nameof(Get), new { id = addedPhotoIdType.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] PhotoIdTypeDto photoIdTypeDto)
    {
        var updatedPhotoIdType = await _photoIdTypesService.UpdateAsync(id, photoIdTypeDto);
        if (updatedPhotoIdType == null)
        {
            return NotFound(
                new BaseResponse<PhotoIdTypeDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"PhotoIdType with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<PhotoIdTypeDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = updatedPhotoIdType
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedPhotoIdType = await _photoIdTypesService.DeleteAsync(id);
        if (deletedPhotoIdType == null)
        {
            return NotFound(
                new BaseResponse<PhotoIdTypeDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"PhotoIdType with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<PhotoIdTypeDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = deletedPhotoIdType
        };

        return Ok(response);
    }
}
