using Diwas.Data;
using Diwas.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diwas.TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoDbContext _todoDbContext;

        public TodosController(TodoDbContext todoDbContext)
        {
            _todoDbContext = todoDbContext;
        }

        //get specific list based on searched name or description / get all list
        [HttpGet]
        public async Task<ActionResult<List<Todo>>> Get([FromQuery] string search)
        {
            List<Todo> todos = new List<Todo>();
            if (!string.IsNullOrEmpty(search))
            {
                todos = await _todoDbContext.Todos.Where(x => x.Name.Contains(search) || x.Description.Contains(search)).ToListAsync();
            }
            else
            {
                todos = await _todoDbContext.Todos.ToListAsync();
            }
            return todos;
        }

        //get a todo by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetDetails(int id)
        {
            var todo = await _todoDbContext.Todos.SingleOrDefaultAsync(x => x.Id == id);
            if (todo == null) return NotFound();
            return todo;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Todo todo)
        {
            _todoDbContext.Todos.Add(todo);
            await _todoDbContext.SaveChangesAsync();
            return todo.Id;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _todoDbContext.Todos.SingleOrDefaultAsync(x => x.Id == id);
            if (todo == null)
                return BadRequest();
            _todoDbContext.Todos.Remove(todo);
            await _todoDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }
            _todoDbContext.Todos.Update(todo);
            await _todoDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}