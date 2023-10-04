using Microsoft.AspNetCore.Mvc;
using Nice_Admin_Theme_Implement.Areas.LOC_Country.Models;
using Nice_Admin_Theme_Implement.Areas.LOC_State.Models;
using System.Data.SqlClient;
using System.Data;
using Nice_Admin_Theme_Implement.Areas.LOC_City.Models;
using SmartBreadcrumbs.Attributes;

namespace Nice_Admin_Theme_Implement.Areas.LOC_City.Controllers
{
    [Breadcrumb("City")]
    [Area("LOC_City")]
    [Route("LOC_City/[controller]/[action]")]
    public class LOC_CityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Configuration

        private readonly IConfiguration Configuration;

        public LOC_CityController(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        #endregion

        #region City List

        [Breadcrumb(FromAction = "Index", Title = "City List")]
        public IActionResult LOC_CityList()
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

            #region State DropDown

            SqlConnection connection2 = new SqlConnection(connectionStr);
            connection2.Open();
            SqlCommand objCmd2 = connection2.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_State_ComboBox2";
            SqlDataReader reader2 = objCmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(reader2);
            connection2.Close();

            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                LOC_StateDropDownModel stateModel = new LOC_StateDropDownModel();
                stateModel.StateID = Convert.ToInt32(dr["StateID"]);
                stateModel.StateName = dr["StateName"].ToString();
                list2.Add(stateModel);
            }
            ViewBag.StateList = list2;

            #endregion

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_City_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_CityList", dt);
        }

        #endregion

        #region City Delete

        public IActionResult LOC_CityDelete(int CityID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_City_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CityID", CityID);
            objCmd.ExecuteNonQuery();
            return RedirectToAction("LOC_CityList");
        }

        #endregion

        #region City Add / Update

        public IActionResult Save(LOC_CityModel model)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            if (model.CityID == null)
            {
                objCmd.CommandText = "PR_City_Insert";
                TempData["LOC_City_Insert_Message"] = "Record Inserted Successfully!!";
            }
            else
            {
                objCmd.CommandText = "PR_City_UpdateByPK";
                objCmd.Parameters.AddWithValue("@CityID", model.CityID);
                TempData["LOC_City_Insert_Message"] = "Record Updated Successfully!!";
            }

            objCmd.Parameters.AddWithValue("@CityName", model.CityName);
            objCmd.Parameters.AddWithValue("@CityCode", model.CityCode);
            objCmd.Parameters.AddWithValue("@CountryID", model.CountryID);
            objCmd.Parameters.AddWithValue("@StateID", model.StateID);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_CityList");
        }

        [Breadcrumb(FromAction = "Index", Title = "Add - Update City")]
        public IActionResult LOC_CityAdd(int? CityID)
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

            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel countryModel = new LOC_CountryDropDownModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);
            }
            ViewBag.CountryList = list;

            #endregion

            #region State DropDown

            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list2;

            #endregion

            if (CityID != null)
            {
                SqlConnection connection = new SqlConnection(connectionStr);
                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_City_SelectByPk";
                objCmd.Parameters.AddWithValue("@CityID", CityID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();

                dt.Load(objSDR);
                if (dt.Rows.Count > 0)
                {
                    LOC_CityModel model = new LOC_CityModel();

                    foreach (DataRow dr in dt.Rows)
                    {
                        model.CityID = Convert.ToInt32(dr["CityID"]);
                        model.CityName = (string)dr["CityName"];
                        model.CityCode = (string)dr["CityCode"];
                        model.CountryID = Convert.ToInt32(dr["CountryID"]);
                        model.StateID = Convert.ToInt32(dr["StateID"]);
                    }

                    DataTable dt2 = new DataTable();
                    SqlConnection conn = new SqlConnection(connectionStr);
                    conn.Open();
                    SqlCommand objCmd2 = connection.CreateCommand();
                    objCmd2.CommandType = CommandType.StoredProcedure;
                    objCmd2.CommandText = "PR_State_ComboBox";
                    objCmd2.Parameters.AddWithValue("@CountryID", model.CountryID);
                    SqlDataReader objSDR2 = objCmd2.ExecuteReader();
                    dt2.Load(objSDR2);

                    List<LOC_StateDropDownModel> list3 = new List<LOC_StateDropDownModel>();

                    foreach (DataRow dr in dt2.Rows)
                    {
                        LOC_StateDropDownModel vlst = new LOC_StateDropDownModel();
                        vlst.StateID = Convert.ToInt32(dr["StateID"]);
                        vlst.StateName = dr["StateName"].ToString();
                        list3.Add(vlst);
                    }
                    ViewBag.StateList = list3;

                    return View("LOC_CityAdd", model);

                }

                return View("LOC_CityAdd");
            }
            else
            {
                return View("LOC_CityAdd");
            }
        }

        #endregion

        #region Cascade DropDown of State
        public IActionResult StateDropDownByCountryID(int CountryID)
        {
            string myconnStr1 = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection1 = new SqlConnection(myconnStr1);
            DataTable dt1 = new DataTable();
            connection1.Open();
            SqlCommand cmd1 = connection1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_State_ComboBox";
            cmd1.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            dt1.Load(reader1);

            List<LOC_StateDropDownModel> list = new List<LOC_StateDropDownModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_StateDropDownModel lstList = new LOC_StateDropDownModel();
                lstList.StateID = Convert.ToInt32(dr["StateID"]);
                lstList.StateName = dr["StateName"].ToString();
                list.Add(lstList);
            }
            Console.WriteLine(list);
            var vModel = list;
            return Json(vModel);

        }



        #endregion

        #region FILTER
        public IActionResult LOC_CityFilter(LOC_CityFilterModel FilterModel)
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

            #region State DropDown

            SqlConnection connection2 = new SqlConnection(connectionStr);
            connection2.Open();
            SqlCommand objCmd2 = connection2.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_State_ComboBox2";
            SqlDataReader reader2 = objCmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(reader2);
            connection2.Close();

            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                LOC_StateDropDownModel stateModel = new LOC_StateDropDownModel();
                stateModel.StateID = Convert.ToInt32(dr["StateID"]);
                stateModel.StateName = dr["stateName"].ToString();
                list2.Add(stateModel);
            }
            ViewBag.StateList = list2;

            #endregion

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_City_filter";
            objCmd.Parameters.AddWithValue("@CountryID", FilterModel.CountryID);
            objCmd.Parameters.AddWithValue("@StateID", FilterModel.StateID);
            objCmd.Parameters.AddWithValue("@CityName", FilterModel.CityName);
            objCmd.Parameters.AddWithValue("@CityCode", FilterModel.CityCode);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            ModelState.Clear();
            return View("LOC_CityList", dt);
        }
        #endregion
    }
}
