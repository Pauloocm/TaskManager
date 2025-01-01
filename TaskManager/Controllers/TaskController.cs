using Microsoft.AspNetCore.Mvc;
using TaskManager.Platform.Application;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController(ITaskAppService taskAppService) : ControllerBase
    {
        private readonly ITaskAppService taskAppService = taskAppService
            ?? throw new ArgumentNullException(nameof(taskAppService));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var taskId = await taskAppService.Add(command, ct);

            return Created(nameof(Search), new { id = taskId });
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchTasksFilter filter, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            var tasks = await taskAppService.Search(filter, ct);

            return Ok(tasks);
        }

        [HttpPut]
        public async Task<IActionResult> Complete([FromQuery] CompleteTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            await taskAppService.Complete(command, ct);

            return Ok();
        }
    }
}
