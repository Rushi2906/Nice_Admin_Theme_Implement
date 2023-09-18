namespace Nice_Admin_Theme_Implement.Areas.MST_Student.Models
{
    public class MST_StudentModel
    {
        public int? StudentID { get; set; }
        public int? BranchID { get; set; }
        public int? CityID { get; set; }
        public string StudentName { get; set; }
        public string MobileNoStudent { get; set; }
        public string MobileNoFather { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }


    }

    public class MST_StudentFilterModel
    {
        public int? StudentID { get; set; }
        public string? StudentName { get; set; }
        public int? CityID { get; set;}
        public int? BranchID { get; set; }
    }
}
