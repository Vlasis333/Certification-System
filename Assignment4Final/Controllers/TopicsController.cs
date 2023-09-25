using Assignment4Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.DTO;
using ModelLibrary.Models.DTO.Certificates;

namespace Assignment4Final.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicsController : ControllerBase
{
    private readonly TopicsService _topicsService;

    public TopicsController(TopicsService topicsService)
    {
        _topicsService = topicsService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminOrQualityControl")]
    public async Task<IActionResult> GetAll()
    {
        var topics = await _topicsService.GetAllAsync();
        var response = new BaseResponse<List<TopicDto>>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = topics
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Get(int id)
    {
        var topic = await _topicsService.GetAsync(id);
        if (topic == null)
        {
            return NotFound(
                new BaseResponse<TopicDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Topic with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<TopicDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = topic
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Add([FromBody] TopicDto topicDto)
    {
        var addedTopic = await _topicsService.AddAsync(topicDto);
        if (addedTopic == null)
        {
            return BadRequest(
                new BaseResponse<TopicDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = "Failed to add topic."
                }
            );
        }

        var response = new BaseResponse<TopicDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = addedTopic
        };

        return CreatedAtAction(nameof(Get), new { id = addedTopic.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] TopicDto topicDto)
    {
        var updatedTopic = await _topicsService.UpdateAsync(id, topicDto);
        if (updatedTopic == null)
        {
            return NotFound(
                new BaseResponse<TopicDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Topic with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<TopicDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = updatedTopic
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedTopic = await _topicsService.DeleteAsync(id);
        if (deletedTopic == null)
        {
            return NotFound(
                new BaseResponse<TopicDto>
                {
                    RequestId = Request.HttpContext.TraceIdentifier,
                    Success = false,
                    Message = $"Topic with id {id} not found."
                }
            );
        }

        var response = new BaseResponse<TopicDto>
        {
            RequestId = Request.HttpContext.TraceIdentifier,
            Success = true,
            Data = deletedTopic
        };

        return Ok(response);
    }
}
