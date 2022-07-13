using System.ComponentModel.DataAnnotations;

namespace GeicoApp.Data.Entities
{
    public enum GTaskStatus
    {
        [Display(Name = "New")]
        New = 0,
        [Display(Name = "In Progress")]
        InProcess = 1,
        [Display(Name = "Finished")]
        Finished = 2,
    }
}
