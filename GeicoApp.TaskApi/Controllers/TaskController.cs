using GeicoApp.Data.Entities;
using GeicoApp.Models;
using GeicoApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeicoApp.TaskApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {

        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        [HttpGet(Name = "GetAllTasks")]
        public async Task<IEnumerable<TaskModel>> GetAllTasks()
        {
            return await _taskService.GetTasks();
        }

        [HttpPut(Name = "UpdateTask")]
        public async Task UpdateTask(TaskModel updateTask)
        {
            await _taskService.UpdateTask(updateTask);
        }

        [HttpPost(Name = "AddTask")]
        public async Task AddTask(AddTaskModel addTask)
        {
            await _taskService.AddTask(addTask);
        }
    }
}