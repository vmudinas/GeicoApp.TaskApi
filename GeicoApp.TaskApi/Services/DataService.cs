using AutoMapper;
using GeicoApp.Data;
using GeicoApp.Data.Entities;
using GeicoApp.Models;
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
            _context.Tasks.Update(_mapper.Map<GTask>(updateTask));
            await _context.SaveChangesAsync();
        }
    }
}