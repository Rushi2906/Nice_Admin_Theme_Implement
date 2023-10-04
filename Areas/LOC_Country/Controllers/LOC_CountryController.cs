using Microsoft.AspNetCore.Mvc;
using Nice_Admin_Theme_Implement.Areas.LOC_Country.Models;
using Nice_Admin_Theme_Implement.Areas.SYS_User.Models;
using Nice_Admin_Theme_Implement.BAL;
using SmartBreadcrumbs.Attributes;
using System.Data;
using System.Data.SqlClient;

namespace Nice_Admin_Theme_Implement.Areas.LOC_Country.Controllers
{
    [Breadcrumb("Country")]
    [Area("LOC_Country")]
    [Route("LOC_Country/[controller]/[action]")]
    public class LOC_CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Configuration

        private readonly IConfiguration Configuration;

        public LOC_CountryController(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        #endregion

        #region Country List

        [Breadcrumb(FromAction = "Index", Title = "Country List")]
        public IActionResult LOC_CountryList()
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_SelectAll";
            //objCmd.Parameters.AddWithValue("UserID", CV.UserID());
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_CountryList", dt);
        }

        #endregion

        #region Country Delete

        public IActionResult LOC_CountryDelete(int CountryID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);
            objCmd.ExecuteNonQuery();
            return RedirectToAction("LOC_CountryList");
        }

        #endregion

        #region Country Add / Update

        public IActionResult Save(LOC_CountryModel model)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            if (model.CountryID == null)
            {
                objCmd.CommandText = "PR_Country_Insert";
                ViewBag.Data = "Add";
                TempData["LOC_Country_Insert_Message"] = "Record Inserted Successfully!!";
            }
            else
            {
                objCmd.CommandText = "PR_Country_UpdateByPK";
                objCmd.Parameters.AddWithValue("@CountryID", model.CountryID);
                ViewBag.Data = "Update";
                TempData["LOC_Country_Insert_Message"] = "Record Updated Successfully!!";
            }

            //if (Convert.ToBoolean(objCmd.ExecuteNonQuery()))
            //{
            //    if (model.CountryID == null)
            //    {
            //        TempData["LOC_Country_Insert_Message"] = "Record Inserted Successfully!!";
            //    }
            //    else
            //    {
            //        TempData["LOC_Country_Insert_Message"] = "Record Updated Successfully!!";
            //    }

            //}

            objCmd.Parameters.AddWithValue("@CountryName", model.CountryName);
            objCmd.Parameters.AddWithValue("@CountryCode", model.CountryCode);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_CountryList");
        }

        [Breadcrumb(FromAction = "Index", Title = "Add - Update Country")]
        public IActionResult LOC_CountryAdd(int? CountryID)
        {

            if (CountryID != null)
            {
                string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection connection = new SqlConnection(connectionStr);
                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Country_SelectByPk";
                objCmd.Parameters.AddWithValue("@CountryID", CountryID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();

                dt.Load(objSDR);
                LOC_CountryModel model = new LOC_CountryModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.CountryID = Convert.ToInt32(dr["CountryID"]);
                    model.CountryName = (string)dr["CountryName"];
                    model.CountryCode = (string)dr["CountryCode"];
                }
                return View("LOC_CountryAdd", model);
            }
            else
            {
                return View("LOC_CountryAdd");
            }
        }

        #endregion

        #region Filter

        public IActionResult LOC_CountryFilter(LOC_CountryFilterModel filterModel)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_filter";
            objCmd.Parameters.AddWithValue("@CountryName", filterModel.CountryName);
            objCmd.Parameters.AddWithValue("@CountryCode", filterModel.CountryCode);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            ModelState.Clear();
            return View("LOC_CountryList", dt);
        }

        #endregion

    }
}
