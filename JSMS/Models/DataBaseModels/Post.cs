using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSMS.Models.DataBaseModels
{
    public class Post
    {
        [Key]
        public int Post_Id { get; set; }

        [ForeignKey("Founder")]
        public int FdId { get; set; }
        public string? Post_Name { get; set; }
        public int Post_Count { get; set; }
        public string?  Post_Description { get; set; }

        public virtual Founder? Founder { get; set; }

        public virtual ICollection<Apply>? Applies { get; set; }
    }
}