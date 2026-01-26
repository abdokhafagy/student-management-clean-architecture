using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Attendances.Dto;
using StudentManagement.Application.Payments;
using StudentManagement.Application.Payments.Dtos;

namespace StudentManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentServices _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? studentId)
        {
            var payments = await _service.GetAllAsync(studentId);
            if (payments.IsSuccess)
                return Ok(new { payments.IsSuccess, payments.message, payments.data });
            else
                return BadRequest(new { payments.IsSuccess, payments.message });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var payment = await _service.GetByIdAsync(id);
            if (payment.IsSuccess)
                return Ok(new { payment.IsSuccess, payment.message, payment.data });
            else
                return BadRequest(new { payment.IsSuccess, payment.message });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentDto dto)
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
        public async Task<IActionResult> Update([FromRoute] string id, [FromRoute] bool IsPaid)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _service.UpdateAsync(id, IsPaid);
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
