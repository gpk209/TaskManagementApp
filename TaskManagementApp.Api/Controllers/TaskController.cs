using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementApp.Services;
using TaskManagementApp.Core.Entities;

namespace TaskManagementApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _svc;
        public TaskController(ITaskService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Status? status, [FromQuery] Priority? priority)
        {
            var list = await _svc.ListAsync(status, priority);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var t = await _svc.GetAsync(id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskItem dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskItem dto)
        {
            await _svc.UpdateAsync(id, dto);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}
