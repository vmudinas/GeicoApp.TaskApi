using GeicoApp.Data.Entities;
using GeicoApp.Models;

namespace GeicoApp.Services
{
    public interface IDataService
    {
        Task<List<TaskModel>> GetTasks();
        Task AddTask(AddTaskModel newTask);
        Task UpdateTask(TaskModel updateTask);
        IQueryable<GTask> GetQuerableTasks();
    }
}