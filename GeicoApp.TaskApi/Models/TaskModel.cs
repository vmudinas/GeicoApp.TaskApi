using GeicoApp.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GeicoApp.Models
{
    public class TaskModel
    {
        public int Id { get; set; }            
        public string? Name { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public GTaskPriority Priority { get; set; }
        public GTaskStatus Status { get; set; }
    }
}
