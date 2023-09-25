using Assignment4Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO;
using ModelLibrary.Models.DTO.Candidates;

namespace Assignment4Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly CandidateService _candidateService;

        public CandidateController(CandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        // GET: api/<CandidateController>
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdminOrQualityControl"
        )]
        public async Task<IActionResult> GetAllCandidates()
        {
            return Ok(await _candidateService.GetAll());
        }

        // GET api/<CandidateController>/5
        [HttpGet("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdminOrQualityControlOrCandidate"
        )]
        public async Task<IActionResult> GetCandidateById(string id)
        {
            var candidate = await _candidateService.GetCandidateById(id);
            if (candidate == null)
            {
                return NotFound(
                    new BaseResponse<CandidatesDto>
                    {
                        RequestId = Request.HttpContext.TraceIdentifier,
                        Success = false,
                        Message = $"Candidate: {id} not found."
                    }
                );
            }

            var response = new BaseResponse<CandidatesDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier,
                Success = true,
                Data = candidate
            };

            return Ok(response);
        }

        // POST api/<CandidateController>
        [HttpPost]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdminOrCandidate"
        )]
        public async Task<IActionResult> CreateCandidate([FromBody] CandidatesDto candidatesDto)
        {
            var addedCandidate = await _candidateService.AddCandidate(candidatesDto);
            if (addedCandidate == null)
            {
                return BadRequest(
                    new BaseResponse<CandidatesDto>
                    {
                        RequestId = Request.HttpContext.TraceIdentifier,
                        Success = false,
                        Message = "Failed to add candidate."
                    }
                );
            }

            var response = new BaseResponse<CandidatesDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier,
                Success = true,
                Data = addedCandidate
            };

            return CreatedAtAction(
                nameof(GetCandidateById),
                new { id = addedCandidate.AppUserId },
                response
            );
        }

        // PUT api/<CandidateController>/5
        [HttpPut("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdmin"
        )]
        public async Task<IActionResult> UpdateCandidate(
            string id,
            [FromBody] CandidatesDto candidatesDto
        )
        {
            var addedCandidate = await _candidateService.UpdateCandidate(id, candidatesDto);
            if (addedCandidate == null)
            {
                return BadRequest(
                    new BaseResponse<CandidatesDto>
                    {
                        RequestId = Request.HttpContext.TraceIdentifier,
                        Success = false,
                        Message = "Failed to add candidate."
                    }
                );
            }

            var response = new BaseResponse<CandidatesDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier,
                Success = true,
                Data = addedCandidate
            };

            return Ok(response);
        }

        // DELETE api/<CandidateController>/5
        [HttpDelete("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdmin"
        )]
        public async Task<IActionResult> DeleteCandidate(string id)
        {
            var response = new BaseResponse<CandidatesDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier,
            };

            var candidateDeleted = await _candidateService.DeleteCandidate(id);

            if (candidateDeleted == null)
            {
                response.Success = false;
                response.Message = $"Error in deleting Question with id: {id}";
                return NotFound(response);
            }

            response.Success = true;
            response.Data = candidateDeleted;

            return Ok(response);
        }
    }
}
