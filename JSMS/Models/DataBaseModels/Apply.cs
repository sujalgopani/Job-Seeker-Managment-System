using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSMS.Models.DataBaseModels
{
    public class Apply
    {
        [Key]
        public int ApplyId { get; set; }

        [ForeignKey("Employee")]
        public int EmpId { get; set; }

        [ForeignKey("Post")]
        public int Post_Id { get; set; }

        public string? remark { get; set; }

        public int FdId { get; set; }
        public bool Status { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Post? Post { get; set; }
        public virtual Founder? Founder { get; set; }
    }
}
