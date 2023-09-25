using Assignment4Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO;
using ModelLibrary.Models.DTO.Exams;

namespace Assignment4Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly ExamService _examService;

        public ExamController(ExamService examService)
        {
            _examService = examService;
        }

        [HttpGet] //GET: Return all the available Exams in db
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdminOrQualityControl"
        )]
        public async Task<ActionResult<List<ExamDto>>> GetAll()
        {
            var exams = await _examService.GetListOfExamsAsync();
            return Ok(_examService.GetExamDtoList(exams));
        }

        [HttpGet("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdminOrQualityControl"
        )]
        public async Task<IActionResult> Get(int id)
        {
            var exam = await _examService.GetAsync(id);
            if (exam == null)
            {
                return NotFound(
                    new BaseResponse<ExamDto>
                    {
                        RequestId = Request.HttpContext.TraceIdentifier,
                        Success = false,
                        Message = $"Exam with id {id} not found."
                    }
                );
            }

            var response = new BaseResponse<ExamDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier,
                Success = true,
                Data = exam
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdmin"
        )]
        public async Task<IActionResult> Add([FromBody] ExamDto examDto)
        {
            var addedExam = await _examService.AddAsync(examDto);
            if (addedExam == null)
            {
                return BadRequest(
                    new BaseResponse<ExamDto>
                    {
                        RequestId = Request.HttpContext.TraceIdentifier,
                        Success = false,
                        Message = "Failed to add exam."
                    }
                );
            }

            var response = new BaseResponse<ExamDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier,
                Success = true,
                Data = addedExam
            };

            return CreatedAtAction(nameof(Get), new { id = addedExam.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdmin"
        )]
        public async Task<IActionResult> Update(int id, [FromBody] ExamDto examDto)
        {
            var updatedExam = await _examService.UpdateAsync(id, examDto);
            if (updatedExam == null)
            {
                return NotFound(
                    new BaseResponse<ExamDto>
                    {
                        RequestId = Request.HttpContext.TraceIdentifier,
                        Success = false,
                        Message = $"Exam with id {id} not found."
                    }
                );
            }

            var response = new BaseResponse<ExamDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier,
                Success = true,
                Data = updatedExam
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdmin"
        )]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedExam = await _examService.DeleteAsync(id);
            if (deletedExam == null)
            {
                return NotFound(
                    new BaseResponse<ExamDto>
                    {
                        RequestId = Request.HttpContext.TraceIdentifier,
                        Success = false,
                        Message = $"Exam with id {id} not found."
                    }
                );
            }

            var response = new BaseResponse<ExamDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier,
                Success = true,
                Data = deletedExam
            };

            return Ok(response);
        }
    }
}
