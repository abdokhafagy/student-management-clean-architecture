using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Groups;
using StudentManagement.Application.Groups.Dtos;
using StudentManagement.Application.Students.Dtos;

namespace StudentManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController(IGroupServices _service) : ControllerBase
    {
     
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] byte? year)
        {

            var groups = await _service.GetAll(year);
            if (groups.IsSuccess)
                return Ok(new { groups.IsSuccess, groups.message, groups.data });
            else
                return BadRequest(new { groups.IsSuccess, groups.message });
        }
      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var group = await _service.GetByIdAsync(id);
            if (group.IsSuccess)
                return Ok(new { group.IsSuccess, group.message, group.data });
            else
                return BadRequest(new { group.IsSuccess, group.message });
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count([FromQuery]byte? year)
        {
            return Ok(await _service.CountAsync(year));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _service.AddAsync(dto);
            if (response.IsSuccess)
                return CreatedAtAction(nameof(GetAll), new { response.Id }, new { response.IsSuccess, response.Id, response.message });
            else
                return BadRequest(new { response.IsSuccess, response.message });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]string id,[FromBody] GroupDto dto)
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
