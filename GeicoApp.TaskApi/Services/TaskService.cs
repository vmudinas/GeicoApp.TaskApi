using GeicoApp.Data.Entities;
using GeicoApp.Models;
using GeicoApp.TaskApi.Exceptions;

namespace GeicoApp.Services
{
    public class TaskService : ITaskService
    {
        private IDataService _dataService;

        public TaskService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<TaskModel>> GetTasks()
        {

            return await _dataService.GetTasks();
        }

        public async Task AddTask(AddTaskModel newTask)
        {
            if (!IsValidDate(newTask.DueDate))
            {
                throw new InvalidTaskException($"Due date is not valid {newTask.DueDate}");
            }

            if (!IsValidPriority(newTask.Priority, newTask.Status))
            {
                throw new InvalidTaskException($"The system should not have more than 100 High Priority tasks which have the same due date and are not finished yet at any time.");
            }

            await _dataService.AddTask(newTask);

        }

        public async Task UpdateTask(TaskModel updateTask)
        {
            if (!IsValidDate(updateTask.DueDate))
            {
                throw new InvalidTaskException($"Due date is not valid {updateTask.DueDate}");
            }

            if (!IsValidPriority(updateTask.Priority, updateTask.Status))
            {
                throw new InvalidTaskException($"The system should not have more than 100 High Priority tasks which have the same due date and are not finished yet at any time.");
            }

            await _dataService.UpdateTask(updateTask);

        }

        private bool IsValidDate(DateTime? dateTime)
        {
            return (dateTime == null) ? false : (DateTime.UtcNow.Date <= dateTime);
        }

        private bool IsValidPriority(GTaskPriority priority, GTaskStatus status)
        {

            if (priority != GTaskPriority.High || status == GTaskStatus.Finished)
            {
                return true;
            }

            var result = _dataService.GetQuerableTasks()
                .Where(x => x.Priority == Data.Entities.GTaskPriority.High && x.Status != GTaskStatus.Finished)
                .GroupBy(x => x.DueDate)
                .Select(group => group.Count()).OrderByDescending(x=>x).Take(1);

            return (result?.FirstOrDefault() == null) ? true : (result.FirstOrDefault()) <= 99;
        }
    }
}