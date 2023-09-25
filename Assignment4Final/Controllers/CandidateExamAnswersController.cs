using Assignment4Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO;
using ModelLibrary.Models.DTO.CandidateExam;

namespace Assignment4Final.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CandidateExamAnswersController : ControllerBase
{
    private readonly CandidateExamAnswersService _candidateExamAnswersService;

    public CandidateExamAnswersController(CandidateExamAnswersService candidateExamAnswersService)
    {
        _candidateExamAnswersService = candidateExamAnswersService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var candidateExamAnswers = await _candidateExamAnswersService.GetAllAsync();
        var response = new BaseResponse<List<CandidateExamAnswersDto>>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = candidateExamAnswers
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Get(int id)
    {
        var candidateExamAnswer = await _candidateExamAnswersService.GetAsync(id);
        if (candidateExamAnswer == null)
        {
            return NotFound(
                new BaseResponse<CandidateExamAnswersDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"CandidateExamAnswer with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<CandidateExamAnswersDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = candidateExamAnswer
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Add([FromBody] CandidateExamAnswersDto candidateExamAnswerDto)
    {
        var addedCandidateExamAnswer = await _candidateExamAnswersService.AddAsync(
            candidateExamAnswerDto
        );
        if (addedCandidateExamAnswer == null)
        {
            return BadRequest(
                new BaseResponse<CandidateExamAnswersDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = "Failed to add candidateExamAnswer."
                }
            );
        }

        var response = new BaseResponse<CandidateExamAnswersDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = addedCandidateExamAnswer
        };

        return CreatedAtAction(nameof(Get), new { id = addedCandidateExamAnswer.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = "IsAdminOrCandidate"
    )]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CandidateExamAnswersDto candidateExamAnswerDto
    )
    {
        var updatedCandidateExamAnswer = await _candidateExamAnswersService.UpdateAsync(
            id,
            candidateExamAnswerDto
        );
        if (updatedCandidateExamAnswer == null)
        {
            return NotFound(
                new BaseResponse<CandidateExamAnswersDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"CandidateExamAnswer with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<CandidateExamAnswersDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = updatedCandidateExamAnswer
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedCandidateExamAnswer = await _candidateExamAnswersService.DeleteAsync(id);
        if (deletedCandidateExamAnswer == null)
        {
            return NotFound(
                new BaseResponse<CandidateExamAnswersDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"CandidateExamAnswer with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<CandidateExamAnswersDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = deletedCandidateExamAnswer
        };

        return Ok(response);
    }
}
