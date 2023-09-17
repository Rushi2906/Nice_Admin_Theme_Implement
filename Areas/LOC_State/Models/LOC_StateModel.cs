using System.ComponentModel.DataAnnotations;

namespace Nice_Admin_Theme_Implement.Areas.LOC_State.Models
{
    public class LOC_StateModel
    {
        public int? StateID { get; set; }
        [Required]
        public string? StateName { get; set; }
        [Required]
        public string? StateCode { get; set; }
        public string? CountryName { get; set; }
        [Required]
        public int? CountryID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }

    public class LOC_StateFilterModel
    {
        public string? StateName { get; set; }
        public string? StateCode { get; set; }
        public int? CountryID { get; set; }
    }
}
