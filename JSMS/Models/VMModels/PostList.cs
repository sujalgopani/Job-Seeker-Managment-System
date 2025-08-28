using JSMS.Models.DataBaseModels;

namespace JSMS.Models.VMModels
{
    public class PostList
    {
        public string? Post_Name { get; set; }
        public int? Post_Id { get; set; }
        public int Post_Count { get; set; }
        public string? Post_Description { get; set; }
        public int? FdId { get; set; }
        public string? Company_Name{ get; set; }

    }
}
