using NodaTime;
using System.ComponentModel.DataAnnotations;

namespace GeicoApp.Data.Entities
{
    public class GTask
    {
        [Key]
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
