using Assignment4Final.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO;
using ModelLibrary.Models.DTO.Questions;

namespace Assignment4Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly QuestionsService _questionService;

        public QuestionsController(IMapper mapper, QuestionsService questionsService)
        {
            _mapper = mapper;
            _questionService = questionsService;
        }

        // GET: api/questions
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdminOrQualityControl"
        )]
        public async Task<IActionResult> GetAll()
        {
            var response = new BaseResponse<List<QuestionDto>>
            {
                RequestId = Request.HttpContext.TraceIdentifier
            };

            var questionDtos = _mapper.Map<List<QuestionDto>>(await _questionService.GetAllAsync());

            if (questionDtos == null)
            {
                response.Success = false;
                response.Message = "Could not get all Questions.";
                return NotFound(response);
            }

            response.Success = true;
            response.Data = questionDtos;

            return Ok(response);
        }

        // GET: api/questions/5
        [HttpGet("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdminOrQualityControl"
        )]
        public async Task<IActionResult> Get(int id)
        {
            var response = new BaseResponse<QuestionDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier
            };

            if (!_questionService.QuestionsDbSetExists())
            {
                response.Success = false;
                response.Message = "Entity set 'ApplicationDbContext.Questions' is null.";
                return NotFound(response);
            }

            var questionDto = _mapper.Map<QuestionDto>(await _questionService.GetAsync(id));

            if (questionDto == null)
            {
                response.Success = false;
                response.Message = $"Could not get Question with id: {id}";
                return NotFound(response);
            }

            response.Success = true;
            response.Data = questionDto;

            return Ok(response);
        }

        // POST: api/questions
        [HttpPost]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdmin"
        )]
        public async Task<IActionResult> Create([FromBody] QuestionDto questionDto)
        {
            var response = new BaseResponse<QuestionDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier
            };

            if (!_questionService.QuestionsDbSetExists())
            {
                response.Success = false;
                response.Message = "Entity set 'ApplicationDbContext.Questions' is null.";
                return NotFound(response);
            }

            if (ModelState.IsValid)
            {
                var addedQuestionDto = _mapper.Map<QuestionDto>(
                    await _questionService.AddAsync(questionDto)
                );

                if (addedQuestionDto == null)
                {
                    response.Success = false;
                    response.Message = "Error in creating question.";
                    return BadRequest(response);
                }

                response.Success = true;
                response.Data = addedQuestionDto;

                return CreatedAtAction(nameof(Get), new { id = addedQuestionDto.Id }, response);
            }

            response.Success = false;
            response.Message = "Invalid model provided.";

            return BadRequest(response);
        }

        // PUT: api/questions/5
        [HttpPut("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdmin"
        )]
        public async Task<IActionResult> Update(int id, [FromBody] QuestionDto questionDto)
        {
            var response = new BaseResponse<QuestionDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier
            };

            if (!_questionService.QuestionsDbSetExists())
            {
                response.Success = false;
                response.Message = "Entity set 'ApplicationDbContext.Questions' is null.";
                return NotFound(response);
            }

            if (ModelState.IsValid)
            {
                var updatedQuestionDto = _mapper.Map<QuestionDto>(
                    await _questionService.UpdateAsync(id, questionDto)
                );

                if (updatedQuestionDto == null)
                {
                    response.Success = false;
                    response.Message = $"Error in updating Question with id: {id}";
                    return Conflict(response);
                }

                response.Success = true;
                response.Data = updatedQuestionDto;

                return Ok(response);
            }

            response.Success = false;
            response.Message = "Invalid model provided.";

            return BadRequest(response);
        }

        // DELETE: api/questions/5
        [HttpDelete("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "IsAdmin"
        )]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new BaseResponse<QuestionDto>
            {
                RequestId = Request.HttpContext.TraceIdentifier
            };

            if (!_questionService.QuestionsDbSetExists())
            {
                response.Success = false;
                response.Message = "Entity set 'ApplicationDbContext.Questions' is null.";
                return NotFound(response);
            }

            var deletedQuestionDto = _mapper.Map<QuestionDto>(await _questionService.Delete(id));

            if (deletedQuestionDto == null)
            {
                response.Success = false;
                response.Message = $"Error in deleting Question with id: {id}";
                return NotFound(response);
            }

            response.Success = true;
            response.Data = deletedQuestionDto;

            return Ok(response);
        }
    }
}
