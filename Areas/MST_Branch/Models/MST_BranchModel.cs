namespace Nice_Admin_Theme_Implement.Areas.MST_Branch.Models
{
    public class MST_BranchModel
    {
        public int? BranchID { get; set; }

        public string BranchName { get; set; }

        public string BranchCode { get; set; }
    }

    public class MST_BranchDropDownModel
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
    }

    public class MST_BranchFilterModel
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
    }
}
