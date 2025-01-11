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

            return Created(nameof(GetLatest), new { id = taskId });
        }

        [HttpGet]
        public async Task<IActionResult> GetLatest([FromQuery] SearchTasksFilter filter, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(filter);

            var tasks = await taskAppService.GetLatestFinisheds(filter, ct);

            return Ok(tasks);
        }

        [HttpPut("/Complete")]
        public async Task<IActionResult> Complete([FromQuery] CompleteTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            await taskAppService.Complete(command, ct);

            return Ok();
        }

        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateTaskCommand command, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            await taskAppService.Update(command.SetId(Id), ct);

            return Ok();
        }
    }
}
