using Assignment4Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO;
using ModelLibrary.Models.DTO.CandidateExam;

namespace Assignment4Final.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MarkersController : ControllerBase
{
    private readonly MarkersService _markersService;

    public MarkersController(MarkersService markersService)
    {
        _markersService = markersService;
    }

    [HttpGet]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = "IsAdminOrMarkerOrQualityControl"
    )]
    public async Task<IActionResult> GetAll()
    {
        var markers = await _markersService.GetAllAsync();
        var response = new BaseResponse<List<MarkerDto>>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = markers
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = "IsAdminOrMarkerOrQualityControl"
    )]
    public async Task<IActionResult> Get(string id)
    {
        var marker = await _markersService.GetAsync(id);
        if (marker == null)
        {
            return NotFound(
                new BaseResponse<MarkerDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Marker with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<MarkerDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = marker
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Add([FromBody] MarkerDto markerDto)
    {
        var addedMarker = await _markersService.AddAsync(markerDto);
        if (addedMarker == null)
        {
            return BadRequest(
                new BaseResponse<MarkerDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = "Failed to add marker."
                }
            );
        }

        var response = new BaseResponse<MarkerDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = addedMarker
        };

        return CreatedAtAction(nameof(Get), new { id = addedMarker.AppUserId }, response);
    }

    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Update(string id, [FromBody] MarkerDto markerDto)
    {
        var updatedMarker = await _markersService.UpdateAsync(id, markerDto);
        if (updatedMarker == null)
        {
            return NotFound(
                new BaseResponse<MarkerDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Marker with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<MarkerDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = updatedMarker
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Delete(string id)
    {
        var deletedMarker = await _markersService.DeleteAsync(id);
        if (deletedMarker == null)
        {
            return NotFound(
                new BaseResponse<MarkerDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Marker with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<MarkerDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = deletedMarker
        };

        return Ok(response);
    }

    [HttpGet("getallcandidateexams")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = "IsAdminOrMarkerOrQualityControl"
    )]
    public async Task<IActionResult> GetAllCandidateExams(bool include = false)
    {
        List<CandidateExamDto> candExams;
        BaseResponse<List<CandidateExamDto>> response;

        if (include)
        {
            candExams = await _markersService.GetAllCandidateExamsAsync(true);
            response = new BaseResponse<List<CandidateExamDto>>
            {
                RequestId = Request.HttpContext.TraceIdentifier,
                Success = true,
                Data = candExams
            };

            return Ok(response);
        }

        candExams = await _markersService.GetAllCandidateExamsAsync();

        response = new BaseResponse<List<CandidateExamDto>>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = candExams
        };

        return Ok(response);
    }

    [HttpPut("assign/{candExamId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> AssignCandidateExamToMarker(
        int candExamId,
        [FromBody] CandidateExamDto candExamDto
    )
    {
        var assignedCandExam = await _markersService.AssignCandidateExamToMarker(
            candExamId,
            candExamDto
        );
        if (assignedCandExam == null)
        {
            return NotFound(
                new BaseResponse<CandidateExamDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"CandidateExam with id {candExamId} not found."
                }
            );
        }

        var response = new BaseResponse<CandidateExamDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = assignedCandExam
        };

        return Ok(response);
    }

    [HttpPut("mark/{candExamId}")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = "IsAdminOrMarker"
    )]
    public async Task<IActionResult> MarkCandidateExam(
        int candExamId,
        [FromBody] CandidateExamDto candExamDto
    )
    {
        var markedCandExam = await _markersService.MarkCandidateExam(candExamId, candExamDto);
        if (markedCandExam == null)
        {
            return NotFound(
                new BaseResponse<CandidateExamDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"CandidateExam with id {candExamId} not found."
                }
            );
        }

        var response = new BaseResponse<CandidateExamDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = markedCandExam
        };

        return Ok(response);
    }
}
