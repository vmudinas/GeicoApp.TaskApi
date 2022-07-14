using AutoMapper;
using GeicoApp.Data;
using GeicoApp.Data.Entities;
using GeicoApp.Models;
using GeicoApp.TaskApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GeicoApp.Services
{
    public class DataService : IDataService
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DataService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TaskModel>> GetTasks()
        {

            return await _context.Tasks.AsNoTracking().Select(x => _mapper.Map<TaskModel>(x)).ToListAsync();
        }

        public IQueryable<GTask> GetQuerableTasks()
        {

            return  _context.Tasks.AsNoTracking();
        }

        public async Task AddTask(AddTaskModel newTask)
        {
            var task = _mapper.Map<GTask>(newTask);
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTask(TaskModel updateTask)
        {
            var dbTask = _mapper.Map<GTask>(updateTask);
            if (_context.Tasks.Any(x => x.Id == dbTask.Id))
            {
                _context.Tasks.Update(_mapper.Map<GTask>(updateTask));
                await _context.SaveChangesAsync();
            }
            else 
            {
                throw new InvalidTaskException($"Task with id: {dbTask.Id} cannot be updated, as that id does not exist in the database.");
            }    
        }
    }
}