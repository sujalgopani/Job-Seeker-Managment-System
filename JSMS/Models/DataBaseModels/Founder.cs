using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace JSMS.Models.DataBaseModels
{
    public class Founder
    {
        [Key]
        public  int FdId { get; set; }
        public  string? FdName { get; set; }
        public string? Fd_Company_Name { get; set; }
        public string? Fd_Email { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }

    }
}
