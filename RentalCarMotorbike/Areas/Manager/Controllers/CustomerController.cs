
 using RentalCarMotorbike.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlTypes;

namespace RentalCarMotorbike.Areas.Manager.Controllers
{
    public class CustomerController : Controller
    {
        private String connectionStringDB()
        {
            RentalVehicalDB db = new RentalVehicalDB();

            string ConnectionString = db.Database.Connection.ConnectionString;

            return ConnectionString;
        }

        private int SetID()
        {
            RentalVehicalDB db = new RentalVehicalDB();
            return db.Customer.Count() + 1;

        }
        // GET: Manager/Customer
        public ActionResult Index()
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                return View(db.Customer.ToList());
            }
        }

        // GET: Manager/Customer/Details/5
        public ActionResult Details(string id)
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                Customer customer = new Customer();
                customer = db.Customer.Find(id);
                return View(customer);
            }
        }

        // GET: Manager/Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Customer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string Sql = "INSERT INTO Customer(CustomerID," +
                                                "CustomerName," +
                                                "CustomerPassword," +
                                                "CustomerEmail," +
                                                "CustomerAddress," +
                                                "CustomerPhone," +
                                                "CustomerCreateAt)" +
                         "VALUES (@CustomerID, @CustomerName, @CustomerPassword, @CustomerEmail, @CustomerAddress, @CustomerPhone,@CustomerCreateAt)";
            string sqlSelect = "SELECT * FROM Customer WHERE @CustomerID = CustomerID";
            using (SqlConnection oSqlConnection = new SqlConnection(connectionStringDB()))
            {
                oSqlConnection.Open();
                SqlCommand cmd = new SqlCommand(Sql, oSqlConnection);
                SqlCommand commmand = new SqlCommand(sqlSelect, oSqlConnection);

                string idSelect = collection["customer-identity"];
                commmand.Parameters.AddWithValue("@CustomerID", idSelect);
                var sqlReader = commmand.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    oSqlConnection.Close();
                    return Json(new {code = 1, message = "ID or Phone has been existed."});
                }
                else
                {
                    SqlCommand oSqlCommand = new SqlCommand(Sql, oSqlConnection);
                    
                    string id = collection["customer-identity"];
                    string name = collection["customer-name"];
                    string email = collection["customer-email"];
                    string password = collection["customer-password"];
                    string address = collection["customer-address"];
                    string phone = collection["customer-phone"];
                    oSqlCommand.Parameters.AddWithValue("@CustomerID", id);
                    oSqlCommand.Parameters.AddWithValue("@CustomerName", name);
                    oSqlCommand.Parameters.AddWithValue("@CustomerPassword", BCrypt.Net.BCrypt.HashPassword(password, 12));
                    oSqlCommand.Parameters.AddWithValue("@CustomerEmail", email);
                    oSqlCommand.Parameters.AddWithValue("@CustomerAddress", address);
                    oSqlCommand.Parameters.AddWithValue("@CustomerPhone", phone);
                    oSqlCommand.Parameters.AddWithValue("@CustomerCreateAt", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.ExecuteNonQuery();

                    oSqlConnection.Close();
                    return Json(new { code = 0, message = "Create Customer Success." });
                }            }
        }

        // GET: Manager/Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Manager/Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            string Sql = "UPDATE Customer " +
                        "SET CustomerName = @CustomerName," +
                            "CustomerPassword = @CustomerPassword," +
                            "CustomerEmail = @CustomerEmail," +
                            "CustomerAddress = @CustomerAddress," +
                            "CustomerPhone = @CustomerPhone," +
                            "CustomerCreateAt = @CustomerCreateAt " +
                          "WHERE CustomerID = @CustomerID";

            using (SqlConnection oSqlConnection = new SqlConnection(connectionStringDB()))
            {
                SqlCommand oSqlCommand = new SqlCommand(Sql, oSqlConnection);
                try
                {
                    oSqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(Sql, oSqlConnection);

                    oSqlCommand.Parameters.AddWithValue("@CustomerID", id);
                    oSqlCommand.Parameters.AddWithValue("@CustomerName", "Quân");
                    oSqlCommand.Parameters.AddWithValue("@CustomerPassword", "12345");
                    oSqlCommand.Parameters.AddWithValue("@CustomerEmail", "quan@gmail.com");
                    oSqlCommand.Parameters.AddWithValue("@CustomerAddress", "Quan 7, HCM");
                    oSqlCommand.Parameters.AddWithValue("@CustomerPhone", "0971462609");
                    oSqlCommand.Parameters.AddWithValue("@CustomerCreateAt", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.ExecuteNonQuery();
                    return Json(new { Msg = "Success" });
                }
                catch (Exception ex)
                {
                    return Json(new { Msg = ex.Message });
                }
                finally
                {
                    oSqlCommand.Dispose();
                    oSqlConnection.Close();
                    oSqlConnection.Dispose();
                }
            }
        }

        // GET: Manager/Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Manager/Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            string Sql = "DELETE FROM Customer WHERE CustomerID = @CustomerID";

            using (SqlConnection oSqlConnection = new SqlConnection(connectionStringDB()))
            {
                SqlCommand oSqlCommand = new SqlCommand(Sql, oSqlConnection);
                try
                {
                    oSqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(Sql, oSqlConnection);
                    oSqlCommand.Parameters.AddWithValue("@CustomerID", id);
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.ExecuteNonQuery();

                    return Json(new { code = 0, message = "Delete Customer Success" }) ;
                }
                catch (Exception ex)
                {
                    return Json(new { code = 1, message = "Delete Customer Failed" });
                }
                finally
                {
                    oSqlCommand.Dispose();
                    oSqlConnection.Close();
                    oSqlConnection.Dispose();
                }
            }
        }

        public ActionResult ChangePassword(string id, FormCollection collection)
        {
            using(var db = new RentalVehicalDB())
            {
                string newPass = collection["new-password"].ToString();
                Customer customer = db.Customer.Find(id);
                customer.CustomerPassword = BCrypt.Net.BCrypt.HashPassword(newPass, 12);
                db.SaveChanges();

                return Json(new { code = 0, message = "Change New Password Success" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

