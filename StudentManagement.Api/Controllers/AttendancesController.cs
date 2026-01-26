using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Attendances;
using StudentManagement.Application.Attendances.Dto;
using StudentManagement.Application.Groups.Dtos;

namespace StudentManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController(IAttendanceServices _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? studentId)
        {
            var attendances = await _service.GetAll(studentId);
            if (attendances.IsSuccess)
                return Ok(new { attendances.IsSuccess, attendances.message, attendances.data });
            else
                return BadRequest(new { attendances.IsSuccess, attendances.message });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var attendance = await _service.GetByIdAsync(id);
            if (attendance.IsSuccess)
                return Ok(new { attendance.IsSuccess, attendance.message, attendance.data });
            else
                return BadRequest(new { attendance.IsSuccess, attendance.message });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AttendanceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _service.AddAsync(dto);
            if (response.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { response.Id }, new { response.IsSuccess, response.Id, response.message });
            else
                return BadRequest(new { response.IsSuccess, response.message });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromRoute] bool AttendanceStatus)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _service.UpdateAsync(id, AttendanceStatus);
            if (response.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { response.Id }, new { response.IsSuccess, response.Id, response.message });
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
