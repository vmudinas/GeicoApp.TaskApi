using GeicoApp.Data.Entities;
using GeicoApp.Models;

namespace GeicoApp.Services
{
    public interface ITaskService
    {
        Task<List<TaskModel>> GetTasks();
        Task AddTask(AddTaskModel newTask);
        Task UpdateTask(TaskModel updateTask);
    }
}