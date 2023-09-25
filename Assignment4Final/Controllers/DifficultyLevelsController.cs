using Assignment4Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO;
using ModelLibrary.Models.DTO.Certificates;

namespace Assignment4Final.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DifficultyLevelsController : ControllerBase
{
    private readonly DifficultyLevelsService _difficultyLevelsService;

    public DifficultyLevelsController(DifficultyLevelsService difficultyLevelsService)
    {
        _difficultyLevelsService = difficultyLevelsService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminOrQualityControl")]
    public async Task<IActionResult> GetAll()
    {
        var diffLevels = await _difficultyLevelsService.GetAllAsync();
        var response = new BaseResponse<List<DifficultyLevelDto>>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = diffLevels
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminOrQualityControl")]
    public async Task<IActionResult> Get(int id)
    {
        var diffLevel = await _difficultyLevelsService.GetAsync(id);
        if (diffLevel == null)
        {
            return NotFound(
                new BaseResponse<DifficultyLevelDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Difficulty level with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<DifficultyLevelDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = diffLevel
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Add([FromBody] DifficultyLevelDto diffLevelDto)
    {
        var addedDiffLevel = await _difficultyLevelsService.AddAsync(diffLevelDto);
        if (addedDiffLevel == null)
        {
            return BadRequest(
                new BaseResponse<DifficultyLevelDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = "Failed to add difficulty level."
                }
            );
        }

        var response = new BaseResponse<DifficultyLevelDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = addedDiffLevel
        };

        return CreatedAtAction(nameof(Get), new { id = addedDiffLevel.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] DifficultyLevelDto diffLevelDto)
    {
        var updatedDiffLevel = await _difficultyLevelsService.UpdateAsync(id, diffLevelDto);
        if (updatedDiffLevel == null)
        {
            return NotFound(
                new BaseResponse<DifficultyLevelDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Difficulty level with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<DifficultyLevelDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = updatedDiffLevel
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedDiffLevel = await _difficultyLevelsService.DeleteAsync(id);
        if (deletedDiffLevel == null)
        {
            return NotFound(
                new BaseResponse<DifficultyLevelDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Topic with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<DifficultyLevelDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = deletedDiffLevel
        };

        return Ok(response);
    }
}
