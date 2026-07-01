//using _00_TaskManager.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace _00_TaskManager.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TaskManagerController : ControllerBase
//    {
//        private static List<TaskItem> tasks = new List<TaskItem>();
//        private static int nextId = 1;

//        [HttpGet]
//        public ActionResult<IEnumerable<TaskItem>> GetAllTasks()
//        {
//            return Ok(tasks);
//        }

//        [HttpGet("{id}")]
//        public ActionResult<TaskItem> GetTask(int id)
//        {
//            var task = tasks.FirstOrDefault(t => t.Id == id);
//            if (task == null)
//                return NotFound();
//            return Ok(task);
//        }

//        [HttpPost]
//        public ActionResult<TaskItem> CreateTask([FromBody] TaskItem newTask)
//        {
//            newTask.Id = nextId++;
//            tasks.Add(newTask);
//            return CreatedAtAction(nameof(GetTask), new { id = newTask.Id }, newTask);
//        }

//        [HttpPut("{id}")]
//        public IActionResult UpdateTask(int id, [FromBody] TaskItem updatedTask)
//        {
//            var existingTask = tasks.FirstOrDefault(t => t.Id == id);
//            if (existingTask == null)
//                return NotFound();

//            existingTask.Title = updatedTask.Title;
//            existingTask.Description = updatedTask.Description;
//            existingTask.IsCompleted = updatedTask.IsCompleted;
//            existingTask.Priority = updatedTask.Priority;

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public IActionResult DeleteTask(int id)
//        {
//            var task = tasks.FirstOrDefault(t => t.Id == id);
//            if (task == null)
//                return NotFound();

//            tasks.Remove(task);
//            return NoContent();
//        }
//    }
//}


using _00_TaskManager.Models;
using _00_TaskManager.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _00_TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskManagerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskManagerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
        {
            var tasks = await _context.TaskItems.ToListAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItem newTask)
        {
            _context.TaskItems.Add(newTask);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = newTask.Id }, newTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            var existingTask = await _context.TaskItems.FindAsync(id);
            if (existingTask == null)
                return NotFound();

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.IsCompleted = updatedTask.IsCompleted;
            existingTask.Priority = updatedTask.Priority;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
