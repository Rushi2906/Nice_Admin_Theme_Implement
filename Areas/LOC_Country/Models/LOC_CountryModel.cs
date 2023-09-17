using System.ComponentModel.DataAnnotations;

namespace Nice_Admin_Theme_Implement.Areas.LOC_Country.Models
{
    public class LOC_CountryModel
    {
        public int? CountryID { get; set; }
        [Required]
        public string? CountryName { get; set; }
        [Required]
        public string? CountryCode { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

    public class LOC_CountryDropDownModel
    {
        public int CountryID { get; set; }
        public string? CountryName { get; set; }
    }

    public class LOC_CountryFilterModel
    {
        public int? CountryID { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
    }
}
