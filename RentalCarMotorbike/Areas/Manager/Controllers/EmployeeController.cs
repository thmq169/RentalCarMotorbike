using RentalCarMotorbike.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using System.Web.Security;
using BCrypt.Net;
using RentalCarMotorbike.Common;

namespace RentalCarMotorbike.Areas.Manager.Controllers
{
    public class EmployeeController : Controller
    {
        private String connectionStringDB()
        {
            RentalVehicalDB db = new RentalVehicalDB();

            string ConnectionString = db.Database.Connection.ConnectionString;

            return ConnectionString;
        }
        public ActionResult Index()
        {
            using(RentalVehicalDB db = new RentalVehicalDB())
            {
                return View(db.Employee.ToList());
            }
        }

        // GET: Manager/Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Manager/Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Employee/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string Sql = "INSERT INTO Employee(EmployeeID," +
                                                "EmployeeName," +
                                                "EmployeePassword," +
                                                "EmployeeEmail," +
                                                "EmployeePhone) " +
                         "VALUES (@EmployeeID, @EmployeeName, @EmployeePassword,@EmployeeEmail,@EmployeePhone)";
            string sqlSelect = "SELECT * FROM Employee WHERE @EmployeeID = EmployeeID";
            using (SqlConnection oSqlConnection = new SqlConnection(connectionStringDB()))
            {
                oSqlConnection.Open();
                SqlCommand cmd = new SqlCommand(Sql, oSqlConnection);
                SqlCommand commmand = new SqlCommand(sqlSelect, oSqlConnection);

                string idSelect = collection["employee-identity"];
                commmand.Parameters.AddWithValue("@EmployeeID", idSelect);
                var sqlReader = commmand.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    oSqlConnection.Close();
                    return Json(new { code = 1, message = "ID or Phone Employee has been existed." });
                }
                else
                {
                    SqlCommand oSqlCommand = new SqlCommand(Sql, oSqlConnection);
                    string id = collection["employee-identity"];
                    string name = collection["employee-name"];
                    string email = collection["employee-email"];
                    string password = collection["employee-password"];
                    string phone = collection["employee-phone"];
                    
                    oSqlCommand.Parameters.AddWithValue("@EmployeeID", id);
                    oSqlCommand.Parameters.AddWithValue("@EmployeeName", name);
                    oSqlCommand.Parameters.AddWithValue("@EmployeePassword", BCrypt.Net.BCrypt.HashPassword(password,12));
                    oSqlCommand.Parameters.AddWithValue("@EmployeeEmail", email);
                    oSqlCommand.Parameters.AddWithValue("@EmployeePhone", phone);
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.ExecuteNonQuery();

                    return Json(new { code = 0, message = "Create Employee Success " });
                }
            }
        }
        
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var db = new RentalVehicalDB();
                db.Employee.Remove(db.Employee.Find(id));
                db.SaveChanges();
                return Json(new { code = 0, message = "Delete Employee Success" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = "Delete Employee Failed" });
            }
        }
    }
}
