using Assignment4Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO;
using ModelLibrary.Models.DTO.Candidates;

namespace Assignment4Final.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LanguagesController : ControllerBase
{
    private readonly LanguagesService _languagesService;

    public LanguagesController(LanguagesService languagesService)
    {
        _languagesService = languagesService;
    }

    [HttpGet]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = "IsAdminOrQualityControlOrCandidate"
    )]
    public async Task<IActionResult> GetAll()
    {
        var languages = await _languagesService.GetAllAsync();
        var response = new BaseResponse<List<LanguageDto>>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = languages
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Get(int id)
    {
        var language = await _languagesService.GetAsync(id);
        if (language == null)
        {
            return NotFound(
                new BaseResponse<LanguageDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Language with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<LanguageDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = language
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Add([FromBody] LanguageDto languageDto)
    {
        var addedLanguage = await _languagesService.AddAsync(languageDto);
        if (addedLanguage == null)
        {
            return BadRequest(
                new BaseResponse<LanguageDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = "Failed to add language."
                }
            );
        }

        var response = new BaseResponse<LanguageDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = addedLanguage
        };

        return CreatedAtAction(nameof(Get), new { id = addedLanguage.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] LanguageDto languageDto)
    {
        var updatedLanguage = await _languagesService.UpdateAsync(id, languageDto);
        if (updatedLanguage == null)
        {
            return NotFound(
                new BaseResponse<LanguageDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Language with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<LanguageDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = updatedLanguage
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedLanguage = await _languagesService.DeleteAsync(id);
        if (deletedLanguage == null)
        {
            return NotFound(
                new BaseResponse<LanguageDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Language with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<LanguageDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = deletedLanguage
        };

        return Ok(response);
    }
}
