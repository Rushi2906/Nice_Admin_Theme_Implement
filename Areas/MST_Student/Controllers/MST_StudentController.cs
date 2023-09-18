using Microsoft.AspNetCore.Mvc;
using Nice_Admin_Theme_Implement.Areas.LOC_City.Models;
using Nice_Admin_Theme_Implement.Areas.MST_Branch.Models;
using Nice_Admin_Theme_Implement.Areas.MST_Student.Models;
using System.Data.SqlClient;
using System.Data;
using Nice_Admin_Theme_Implement.Areas.LOC_Country.Models;
using Nice_Admin_Theme_Implement.Areas.LOC_State.Models;

namespace Nice_Admin_Theme_Implement.Areas.MST_Student.Controllers
{
    [Area("MST_Student")]
    [Route("MST_Student/[controller]/[action]")]
    public class MST_StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Configuration

        private readonly IConfiguration Configuration;

        public MST_StudentController(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        #endregion

        #region Student Select All

        public IActionResult MST_StudentList()
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");

            #region Branch DropDown

            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Branch_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<MST_BranchModel> list = new List<MST_BranchModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                MST_BranchModel branchModel = new MST_BranchModel();
                branchModel.BranchID = Convert.ToInt32(dr["BranchID"]);
                branchModel.BranchName = dr["BranchName"].ToString();
                list.Add(branchModel);
            }
            ViewBag.BranchList = list;

            #endregion

            #region City DropDown

            SqlConnection connection2 = new SqlConnection(connectionStr);
            connection2.Open();
            SqlCommand objCmd2 = connection2.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_City_ComboBox";
            SqlDataReader reader2 = objCmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(reader2);
            connection2.Close();

            List<LOC_CityDropDownModel> list2 = new List<LOC_CityDropDownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                LOC_CityDropDownModel cityModel = new LOC_CityDropDownModel();
                cityModel.CityID = Convert.ToInt32(dr["CityID"]);
                cityModel.CityName = dr["CityName"].ToString();
                list2.Add(cityModel);
            }
            ViewBag.CityList = list2;

            #endregion

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Student_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("MST_StudentList", dt);
        }

        #endregion

        #region Delete

        public IActionResult MST_StudentDelete(int StudentID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Student_DeleteByPK";
            objCmd.Parameters.AddWithValue("@StudentID", StudentID);
            objCmd.ExecuteNonQuery();
            return RedirectToAction("MST_StudentList");
        }

        #endregion

        #region Add/Edit

        public IActionResult Save(MST_StudentModel model)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            if (model.StudentID == null)
            {
                objCmd.CommandText = "PR_Student_Insert";
            }
            else
            {
                objCmd.CommandText = "PR_Student_UpdateByPK";
                objCmd.Parameters.AddWithValue("@StudentID", model.StudentID);
            }
            objCmd.Parameters.AddWithValue("@BranchID", model.BranchID);
            objCmd.Parameters.AddWithValue("@CityID", model.CityID);
            objCmd.Parameters.AddWithValue("@StudentName", model.StudentName);
            objCmd.Parameters.AddWithValue("@MobileNoStudent", model.MobileNoStudent);
            objCmd.Parameters.AddWithValue("@Email", model.Email);
            objCmd.Parameters.AddWithValue("@MobileNoFather", model.MobileNoFather);
            objCmd.Parameters.AddWithValue("@Address", model.Address);
            objCmd.Parameters.AddWithValue("@BirthDate", model.BirthDate);
            objCmd.Parameters.AddWithValue("@Age", model.Age);
            objCmd.Parameters.AddWithValue("@IsActive", model.IsActive);
            objCmd.Parameters.AddWithValue("@Gender", model.Gender);
            objCmd.Parameters.AddWithValue("@Password", model.Password);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("MST_StudentList");
        }

        public IActionResult MST_StudentAdd(int? StudentID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");

            #region Branch DropDown

            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Branch_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            List<MST_BranchDropDownModel> list = new List<MST_BranchDropDownModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                MST_BranchDropDownModel branchModel = new MST_BranchDropDownModel();
                branchModel.BranchID = Convert.ToInt32(dr["BranchID"]);
                branchModel.BranchName = dr["BranchName"].ToString();
                list.Add(branchModel);
            }

            Console.WriteLine(list);
            ViewBag.BranchList = list;

            #endregion

            #region City DropDown

            SqlConnection connection2 = new SqlConnection(connectionStr);
            connection2.Open();
            SqlCommand objCmd2 = connection1.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_City_ComboBox";
            SqlDataReader reader2 = objCmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(reader2);

            List<LOC_CityDropDownModel> list2 = new List<LOC_CityDropDownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                LOC_CityDropDownModel cityModel = new LOC_CityDropDownModel();
                cityModel.CityID = Convert.ToInt32(dr["CityID"]);
                cityModel.CityName = dr["CityName"].ToString();
                list2.Add(cityModel);
            }
            Console.WriteLine(list2);
            ViewBag.CityList = list2;

            #endregion

            if (StudentID != null)
            {
                SqlConnection connection = new SqlConnection(connectionStr);
                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Student_SelectByPk";
                objCmd.Parameters.AddWithValue("@StudentID", StudentID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();

                dt.Load(objSDR);
                MST_StudentModel model = new MST_StudentModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.StudentName = (string)dr["StudentName"];
                    model.BranchID = Convert.ToInt32(dr["BranchID"]);
                    model.CityID = Convert.ToInt32(dr["CityID"]);
                    model.Address = (string)dr["Address"];
                    model.Age = Convert.ToInt32(dr["Age"]); ;
                    model.Email = (string)dr["Email"];
                    model.MobileNoStudent = (string)dr["MobileNoStudent"]; ;
                    model.MobileNoFather = (string)dr["MobileNoFather"];
                    model.Gender = (string)dr["Gender"];
                    model.BirthDate = (DateTime)dr["BirthDate"];
                    model.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    model.Password = (string)dr["Password"];
                }
                return View("MST_StudentAdd", model);
            }

            return View("MST_StudentAdd");
        }

        #endregion

        #region FILTER
        public IActionResult MST_StudentFilter(MST_StudentFilterModel FilterModel)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");

            #region Branch DropDown

            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Branch_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<MST_BranchModel> list = new List<MST_BranchModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                MST_BranchModel branchModel = new MST_BranchModel();
                branchModel.BranchID = Convert.ToInt32(dr["BranchID"]);
                branchModel.BranchName = dr["BranchName"].ToString();
                list.Add(branchModel);
            }
            ViewBag.BranchList = list;

            #endregion

            #region City DropDown

            SqlConnection connection2 = new SqlConnection(connectionStr);
            connection2.Open();
            SqlCommand objCmd2 = connection2.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_City_ComboBox";
            SqlDataReader reader2 = objCmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(reader2);
            connection2.Close();

            List<LOC_CityDropDownModel> list2 = new List<LOC_CityDropDownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                LOC_CityDropDownModel cityModel = new LOC_CityDropDownModel();
                cityModel.CityID = Convert.ToInt32(dr["CityID"]);
                cityModel.CityName = dr["CityName"].ToString();
                list2.Add(cityModel);
            }
            ViewBag.CityList = list2;

            #endregion

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Student_Filter";
            objCmd.Parameters.AddWithValue("@StudentName", FilterModel.StudentName);
            objCmd.Parameters.AddWithValue("@CityID", FilterModel.CityID);
            objCmd.Parameters.AddWithValue("@BranchID", FilterModel.BranchID);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            ModelState.Clear();
            return View("MST_StudentList", dt);
        }
        #endregion
    }
}
