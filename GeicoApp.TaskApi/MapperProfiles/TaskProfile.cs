using AutoMapper;
using GeicoApp.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GeicoApp.Models
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {

            CreateMap<GTask, AddTaskModel>().ReverseMap();
            CreateMap<GTask, TaskModel>().ReverseMap();

        }
    }
}
