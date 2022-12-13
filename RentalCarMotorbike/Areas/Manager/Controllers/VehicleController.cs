using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentalCarMotorbike.Models;
using System.IO;
using System.Web.DynamicData;

namespace RentalCarMotorbike.Areas.Manager.Controllers
{
    public class VehicleController : Controller 
    {
        // GET: Manager/Vehicle
        private String connectionStringDB()
        {
            RentalVehicalDB db = new RentalVehicalDB();

            string ConnectionString = db.Database.Connection.ConnectionString;

            return ConnectionString;
        }
        public ActionResult Index()
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                return View(db.Vehicle.ToList());
            }
        }

        // GET: Manager/Vehicle/Details/5
        public ActionResult Details(string id)
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                Vehicle vehicle = new Vehicle();    
                vehicle = db.Vehicle.Find(id);
                return View(vehicle);
            }
        }
        
        public ActionResult DisplayImage(string id)
        {
            RentalVehicalDB db = new RentalVehicalDB();
            Vehicle img = db.Vehicle.Find(id.ToString());
            return File(img.VehicleImage, "image/jpg");
        }

        private int SetID()
        {
            RentalVehicalDB db = new RentalVehicalDB();
                return db.Vehicle.Count() + 1;
            
        }

        // GET: Manager/Vehicle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Vehicle/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase VehiclePostImage)
        {
            string Sql = "INSERT INTO Vehicle(VehicleID, " +
                                                "VehiclePrice, " +
                                                "VehicleName," +
                                                "VehicleDescription," +
                                                "VehicleInventory," +
                                                "VehicleFlatNumber," +
                                                "VehicleModel," +
                                                "VehicleStatus," +
                                                "VehicleImage) " +
                         "VALUES (@VehicleID, @VehiclePrice, @VehicleName,@VehicleDescription,@VehicleInventory,@VehicleFlatNumber,@VehicleModel,@VehicleStatus,@VehicleImage)";
            
            using (SqlConnection oSqlConnection = new SqlConnection(connectionStringDB()))
            {
                SqlCommand oSqlCommand = new SqlCommand(Sql, oSqlConnection);
                try
                {
                   
                    oSqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(Sql, oSqlConnection);
                    string id = collection["vehicle-flat"];
                    string name = collection["vehicle-name"];
                    string flat = id;
                    string model = collection["vehicle-model"];
                    int price = Convert.ToInt32(collection["vehicle-price"]);
                    string desc = collection["vehicle-desc"];

                    if (VehiclePostImage != null)
                    {

                        string imageName = id.ToString();
                        string extension = Path.GetExtension(VehiclePostImage.FileName);
                        if(model == "Car")
                        {
                            VehiclePostImage.SaveAs(Server.MapPath("~/Content/Image/Upload/Cars/" + imageName + extension));
                        }
                        else if(model == "Motorbike")
                        {
                            VehiclePostImage.SaveAs(Server.MapPath("~/Content/Image/Upload/Motorbikes/" + imageName + extension));
                        }
                        

                    }
                    else return View();
                    
                    oSqlCommand.Parameters.AddWithValue("@VehicleID", id.Replace("-","").Replace(".","").Replace(" ",""));
                    oSqlCommand.Parameters.AddWithValue("@VehiclePrice", price);
                    oSqlCommand.Parameters.AddWithValue("@VehicleName", name);
                    oSqlCommand.Parameters.AddWithValue("@VehicleDescription", desc);
                    oSqlCommand.Parameters.AddWithValue("@VehicleInventory", true);
                    oSqlCommand.Parameters.AddWithValue("@VehicleFlatNumber",flat );
                    oSqlCommand.Parameters.AddWithValue("@VehicleModel", model);
                    oSqlCommand.Parameters.AddWithValue("@VehicleStatus", "Available");
                    oSqlCommand.Parameters.AddWithValue("@VehicleImage", id.ToString() + Path.GetExtension(VehiclePostImage.FileName));
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.ExecuteNonQuery();

                    //Response.AddHeader("Refresh", "5");
                    return Json(new { code = 0, message = "Create Success" });
                    //return View();
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

        // GET: Manager/Vehicle/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Manager/Vehicle/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {

            // TODO: Add update logic here

            string Sql = "UPDATE Vehicle " +
                "SET VehiclePrice = @VehiclePrice," +
                "VehicleName = @VehicleName," +
                "VehicleDescription = @VehicleDescription," +
                "VehicleInventory = @VehicleInventory," +
                "VehicleFlatNumber = @VehicleFlatNumber," +
                "VehicleModel = @VehicleModel," +
                "VehicleStatus = @VehicleStatus " +
                //"VehicleID = @VehicleID " +
                "WHERE VehicleID = @VehicleID"; 

            using (SqlConnection oSqlConnection = new SqlConnection(connectionStringDB()))
            {
                SqlCommand oSqlCommand = new SqlCommand(Sql, oSqlConnection);
                try
                {
                    oSqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(Sql, oSqlConnection);

                    string name = collection["vehicle-name-edit"];
                    string flat = collection["vehicle-flat-edit"];
                    string model = collection["vehicle-model-edit"];
                    string status = collection["vehicle-status-edit"];
                    int price = Convert.ToInt32(collection["vehicle-price-edit"]);
                    string desc = collection["vehicle-desc-edit"];

                    oSqlCommand.Parameters.AddWithValue("@VehicleID", id);
                    oSqlCommand.Parameters.AddWithValue("@VehiclePrice", price);
                    oSqlCommand.Parameters.AddWithValue("@VehicleName", name);
                    oSqlCommand.Parameters.AddWithValue("@VehicleDescription", desc);
                    oSqlCommand.Parameters.AddWithValue("@VehicleInventory", true);
                    oSqlCommand.Parameters.AddWithValue("@VehicleFlatNumber", flat);
                    oSqlCommand.Parameters.AddWithValue("@VehicleModel", model);
                    oSqlCommand.Parameters.AddWithValue("@VehicleStatus", status);
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.ExecuteNonQuery();

                    return Json(new { code = 0, message = "Edit Success" });
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

        // GET: Manager/Vehicle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Manager/Vehicle/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            string Sql = "DELETE FROM Vehicle WHERE VehicleID = @VehicleID";

            using (SqlConnection oSqlConnection = new SqlConnection(connectionStringDB()))
            {
                SqlCommand oSqlCommand = new SqlCommand(Sql, oSqlConnection);
                try
                {
                    oSqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(Sql, oSqlConnection);
                    oSqlCommand.Parameters.AddWithValue("@VehicleID", id);
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.ExecuteNonQuery();

                    return Json(new { code = 0, message = "Delete Success" });
                }
                catch (Exception ex)
                {
                    return Json(new { code = 1, message = "Delete Failed" });
                }
                finally
                {
                    oSqlCommand.Dispose();
                    oSqlConnection.Close();
                    oSqlConnection.Dispose();
                }
            }
        }
    }
}
