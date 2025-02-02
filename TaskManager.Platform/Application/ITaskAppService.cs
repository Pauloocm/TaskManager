﻿namespace TaskManager.Platform.Application
{
    public interface ITaskAppService
    {
        Task<Guid> Add(AddTaskCommand command, CancellationToken ct = default);
        Task Complete(CompleteTaskCommand command, CancellationToken ct = default);
        Task<List<TaskDto>> GetLatestFinisheds(CancellationToken ct = default);
        Task Update(UpdateTaskCommand command, CancellationToken ct = default);
    }
}
