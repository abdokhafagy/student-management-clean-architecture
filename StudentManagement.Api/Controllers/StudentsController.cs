using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Students.Dtos;
using StudentManagement.Application.Users;

namespace StudentManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(IStudentServices _service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? groupId)
    {
        var students = await _service.GetAllAsync(groupId);
        if (students.IsSuccess)
            return Ok(new { students.IsSuccess, students.message, students.data });
        else
            return BadRequest(new { students.IsSuccess, students.message });
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {

        var student = await _service.GetByIdAsync(id);
        if (student.IsSuccess)
            return Ok(new { student.IsSuccess, student.message, student.data });
        else
            return BadRequest(new { student.IsSuccess, student.message });
    }
 
    [HttpGet("count")]
    public async Task<IActionResult> Count([FromQuery] string? groupId,[FromQuery] byte? year)
    {
        return Ok(await _service.CountAsync(groupId, year));
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var response = await _service.AddAsync(dto);
        if (response.IsSuccess)
            return CreatedAtAction(nameof(GetAll), new { response.Id }, new {response.IsSuccess, response.Id,response.message });
        else
            return BadRequest(new { response.IsSuccess, response.message });
    }
    [HttpPost("active/{id}")]
    public async Task<IActionResult> Active([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var response = await _service.ActiveAsync(id);
        if (response.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { response.Id }, new { response.IsSuccess, response.Id, response.message });
        else
            return BadRequest(new { response.IsSuccess, response.message });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] StudentDto dto)
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

       var row = await _service.DeleteAsync(id);

        return Ok(row);
    }

}
