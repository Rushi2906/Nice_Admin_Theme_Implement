using Microsoft.AspNetCore.Mvc;
using Nice_Admin_Theme_Implement.Areas.LOC_Country.Models;
using System.Data.SqlClient;
using System.Data;
using Nice_Admin_Theme_Implement.Areas.LOC_State.Models;
using SmartBreadcrumbs.Attributes;

namespace Nice_Admin_Theme_Implement.Areas.LOC_State.Controllers
{
    [Breadcrumb("State")]
    [Area("LOC_State")]
    [Route("LOC_State/[controller]/[action]")]
    public class LOC_StateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Configuration

        private readonly IConfiguration Configuration;

        public LOC_StateController(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        #endregion

        #region State List

        [Breadcrumb(FromAction = "Index", Title = "State List")]
        public IActionResult LOC_StateList()
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");


            #region Country DropDown
            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Country_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<LOC_CountryModel> list = new List<LOC_CountryModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryModel countryModel = new LOC_CountryModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);
            }
            ViewBag.CountryList = list;

            #endregion



            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_StateList", dt);
        }

        #endregion

        #region State Delete

        public IActionResult LOC_StateDelete(int StateID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_DeleteByPK";
            objCmd.Parameters.AddWithValue("@StateID", StateID);
            objCmd.ExecuteNonQuery();
            return RedirectToAction("LOC_StateList");
        }

        #endregion

        #region State Add / Update

        public IActionResult Save(LOC_StateModel model)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            if (model.StateID == null)
            {
                objCmd.CommandText = "PR_State_Insert";
                TempData["LOC_State_Insert_Message"] = "Record Inserted Successfully!!";
            }
            else
            {
                objCmd.CommandText = "PR_State_UpdateByPK";
                objCmd.Parameters.AddWithValue("@StateID", model.StateID);
                TempData["LOC_State_Insert_Message"] = "Record Updated Successfully!!";
            }

            objCmd.Parameters.AddWithValue("@StateName", model.StateName);
            objCmd.Parameters.AddWithValue("@StateCode", model.StateCode);
            objCmd.Parameters.AddWithValue("@CountryID", model.CountryID);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_StateList");
        }

        [Breadcrumb(FromAction = "Index", Title = "Add - Update State")]
        public IActionResult LOC_StateAdd(int? StateID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");

            #region Country DropDown

            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Country_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<LOC_CountryModel> list = new List<LOC_CountryModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryModel countryModel = new LOC_CountryModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);
            }
            ViewBag.CountryList = list;

            #endregion

            if (StateID != null)
            {
                SqlConnection connection = new SqlConnection(connectionStr);
                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_State_SelectByPk";
                objCmd.Parameters.AddWithValue("@StateID", StateID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();

                dt.Load(objSDR);
                LOC_StateModel model = new LOC_StateModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.StateID = Convert.ToInt32(dr["StateID"]);
                    model.StateName = (string)dr["StateName"];
                    model.StateCode = (string)dr["StateCode"];
                    model.CountryID = Convert.ToInt32(dr["CountryID"]);
                    model.CountryName = (string)dr["CountryName"];
                }
                return View("LOC_StateAdd", model);
            }
            else
            {
                return View("LOC_StateAdd");
            }
        }

        #endregion

        #region Filter

        public IActionResult LOC_StateFilter(LOC_StateFilterModel filterModel)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_filter";
            objCmd.Parameters.AddWithValue("@CountryID", filterModel.CountryID);
            objCmd.Parameters.AddWithValue("@StateName", filterModel.StateName);
            objCmd.Parameters.AddWithValue("@StateCode", filterModel.StateCode);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            Console.WriteLine(filterModel.CountryID + " " + "hello");
            Console.WriteLine(filterModel.StateName + " " + filterModel.StateCode);
            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine(dr["CountryName"]);
                Console.WriteLine(dr["StateName"]);
                Console.WriteLine(dr["StateCode"]);
            }
            ModelState.Clear();

            #region Country DropDown
            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Country_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<LOC_CountryModel> list = new List<LOC_CountryModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryModel countryModel = new LOC_CountryModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);
            }
            ViewBag.CountryList = list;

            #endregion

            return View("LOC_StateList", dt);
        }

        #endregion
    }
}
