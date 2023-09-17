using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Nice_Admin_Theme_Implement.Areas.MST_Branch.Models;
using Nice_Admin_Theme_Implement.Areas.LOC_Country.Models;

namespace Nice_Admin_Theme_Implement.Areas.MST_Branch.Controllers
{
    [Area("MST_Branch")]
    [Route("MST_Branch/[controller]/[action]")]
    public class MST_BranchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Configuration

        private readonly IConfiguration Configuration;

        public MST_BranchController(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        #endregion

        #region Branch Select All

        public IActionResult MST_BranchList()
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Branch_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("MST_BranchList", dt);
        }

        #endregion

        #region Delete

        public IActionResult MST_BranchDelete(int BranchID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Branch_DeleteByPK";
            objCmd.Parameters.AddWithValue("@BranchID", BranchID);
            objCmd.ExecuteNonQuery();
            return RedirectToAction("MST_BranchList");
        }

        #endregion

        #region Add/Edit

        public IActionResult Save(MST_BranchModel model)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            if (model.BranchID == null)
            {
                objCmd.CommandText = "PR_Branch_Insert";
            }
            else
            {
                objCmd.CommandText = "PR_Branch_UpdateByPK";
                objCmd.Parameters.AddWithValue("@BranchID", model.BranchID);
            }
            objCmd.Parameters.AddWithValue("@BranchName", model.BranchName);
            objCmd.Parameters.AddWithValue("@BranchCode", model.BranchCode);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("MST_BranchList");
        }

        public IActionResult MST_BranchAdd(int? BranchID)
        {
            if (BranchID != null)
            {
                string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection connection = new SqlConnection(connectionStr);
                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Branch_SelectByPk";
                objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();

                dt.Load(objSDR);
                MST_BranchModel model = new MST_BranchModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.BranchName = (string)dr["BranchName"];
                    model.BranchCode = (string)dr["BranchCode"];
                }
                return View("MST_BranchAdd", model);
            }
            return View("MST_BranchAdd");
        }

        #endregion

        #region Filter

        public IActionResult MST_BranchFilter(MST_BranchFilterModel filterModel)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Branch_Filter";
            objCmd.Parameters.AddWithValue("@BranchName", filterModel.BranchName);
            objCmd.Parameters.AddWithValue("@BranchCode", filterModel.BranchCode);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            ModelState.Clear();
            return View("MST_BranchList", dt);
        }

        #endregion
    }
}
