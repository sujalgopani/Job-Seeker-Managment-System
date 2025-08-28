using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSMS.Models.DataBaseModels
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        public string? EmpName { get; set; }
        public string? EmpResumeUrl { get; set; }
        public string? EmpEmail { get; set; }
        public string? EmpExpriance { get; set; }
        public string? EmpEducation { get; set; }

        [NotMapped]
        public IFormFile? EmpResume { get; set; }

        public virtual ICollection<Apply>? Applies { get; set; }

    }
}
