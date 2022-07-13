using System.ComponentModel.DataAnnotations;

namespace GeicoApp.Data.Entities
{
    public enum GTaskPriority
    {
        [Display(Name = "High")]
        High = 0,
        [Display(Name = "Middle")]
        Middle = 1,
        [Display(Name = "Low")]
        Low = 2,
    }
}
