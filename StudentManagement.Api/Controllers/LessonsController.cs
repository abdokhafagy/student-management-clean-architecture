using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Groups.Dtos;
using StudentManagement.Application.Lessons;
using StudentManagement.Application.Lessons.Dtos;

namespace StudentManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController(ILessonServices _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? monthId, [FromQuery] string? groupId)
        {
            var lessons = await _service.GetAll(monthId,groupId);
            if (lessons.IsSuccess)
                return Ok(new { lessons.IsSuccess, lessons.message, lessons.data });
            else
                return BadRequest(new { lessons.IsSuccess, lessons.message });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var lesson = await _service.GetByIdAsync(id);
            if (lesson.IsSuccess)
                return Ok(new { lesson.IsSuccess, lesson.message, lesson.data });
            else
                return BadRequest(new { lesson.IsSuccess, lesson.message });
        }
        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            return Ok(await _service.CountAsync());
        }
      
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LessonDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _service.AddAsync(dto);
            if (response.IsSuccess)
                return CreatedAtAction(nameof(GetAll), new { response.Id }, new { response.IsSuccess, response.Id, response.message });
            else
                return BadRequest(new { response.IsSuccess, response.message });
        }
        [HttpPost("finish/{lessonId}")]
        public async Task<IActionResult> Finish([FromRoute] string lessonId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _service.FinishAsync(lessonId);
            if (response.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { response.Id }, new { response.IsSuccess, response.Id, response.message });
            else
                return BadRequest(new { response.IsSuccess, response.message });
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateLessonDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _service.UpdateAsync(id, dto);
            if (response.IsSuccess)
                return CreatedAtAction(nameof(GetAll), new { response.Id }, new { response.IsSuccess, response.Id, response.message });
            else
                return BadRequest(new { response.IsSuccess, response.message });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id cannot be null or empty.");
            }

            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
