using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentalCarMotorbike.Models;
using RentalCarMotorbike.ViewModel;

namespace RentalCarMotorbike.Areas.Manager.Controllers
{
    public class RentController : Controller
    {

        private List<Vehicle> vehiclesRent = new List<Vehicle>();
        private List<Customer> customerRent = new List<Customer>();
        private List<Rent> rentList = new List<Rent>();

        Vehicle vehicle = null;
        Customer customer = null;
        Rent rent = null;
        public ViewModels rentDB =new ViewModels();
        
        private String connectionStringDB()
        {
            RentalVehicalDB db = new RentalVehicalDB();

            string ConnectionString = db.Database.Connection.ConnectionString;

            return ConnectionString;
        }

        // GET: Manager/Order
        public ActionResult Index()
        {
            string Sql = "SELECT V.VehicleName, V.VehiclePrice, V.VehicleInventory, V.VehicleFlatNumber, V.VehicleModel, V.VehicleStatus, V.VehicleImage, " +
                "C.CustomerName, C.CustomerEmail, C.CustomerAddress, C.CustomerPhone, C.CustomerID ," +
                "R.RentID, R.RentCreatAt, R.RentTotalPrice, R.RentStatus " +
                "FROM Vehicle V, Customer C, Rent R, RentDetail RD " +
                "WHERE V.VehicleID = RD.VehicleID AND R.RentID = RD.RentID AND R.CustomerID = C.CustomerID";

            using (SqlConnection oSqlConnection = new SqlConnection(connectionStringDB()))
            {
                SqlCommand oSqlCommand = new SqlCommand(Sql, oSqlConnection);
                try
                {
                    oSqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(Sql, oSqlConnection);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        vehicle = new Vehicle();
                        customer = new Customer();
                        rent = new Rent();
                        vehicle.VehicleName = Convert.ToString(reader.GetSqlValue(0));
                        vehicle.VehiclePrice = reader.GetInt32(1);
                        vehicle.VehicleInventory = Convert.ToBoolean(reader.GetBoolean(2));
                        vehicle.VehicleFlatNumber = Convert.ToString(reader.GetSqlValue(3));
                        vehicle.VehicleModel = Convert.ToString(reader.GetSqlValue(4));
                        vehicle.VehicleStatus = Convert.ToString(reader.GetSqlValue(5));
                        vehicle.VehicleImage = Convert.ToString(reader.GetSqlValue(6));
                        customer.CustomerName = Convert.ToString(reader.GetSqlValue(7));
                        customer.CustomerEmail = Convert.ToString(reader.GetSqlValue(8));
                        customer.CustomerAddress = Convert.ToString(reader.GetSqlValue(9));
                        customer.CustomerPhone = Convert.ToString(reader.GetSqlValue(10));
                        customer.CustomerID = Convert.ToString(reader.GetSqlValue(11));
                        rent.RentID = Convert.ToString(reader.GetSqlValue(12));
                        rent.RentCreatAt = Convert.ToString(reader.GetSqlValue(13));
                        rent.RentTotalPrice = reader.GetInt32(14);
                        rent.RentStatus = Convert.ToString(reader.GetSqlValue(15));
                        vehiclesRent.Add(vehicle);
                        customerRent.Add(customer);
                        rentList.Add(rent);
                    }

                    //oSqlCommand.CommandType = CommandType.Text;
                    //oSqlCommand.ExecuteNonQuery();

                    this.rentDB.Vehicle = vehiclesRent.ToList();
                    this.rentDB.Customer = customerRent.ToList();
                    this.rentDB.Rent = rentList.ToList();
                    
                    return View(rentDB);
                }
                catch (Exception ex)
                {
                    return Json(new { Msg = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    oSqlCommand.Dispose();
                    oSqlConnection.Close();
                    oSqlConnection.Dispose();
                }
            }
        }

        // GET: Manager/Order/Details/5
        public ActionResult Details(int id)
        {
            string Sql = "SELECT V.VehicleName, V.VehiclePrice, V.VehicleInventory, V.VehicleFlatNumber, V.VehicleModel, V.VehicleStatus, V.VehicleImage, " +
                "C.CustomerName, C.CustomerEmail, C.CustomerAddress, C.CustomerPhone, C.CustomerID, " +
                "R.RentID, R.RentCreatAt, R.RentTotalPrice, R.RentStatus " +
                "FROM Vehicle V, Customer C, Rent R, RentDetail RD " +
                "WHERE V.VehicleID = RD.VehicleID AND R.RentID = RD.RentID AND R.CustomerID = C.CustomerID";

            using (SqlConnection oSqlConnection = new SqlConnection(connectionStringDB()))
            {
                SqlCommand oSqlCommand = new SqlCommand(Sql, oSqlConnection);
                try
                {
                    oSqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(Sql, oSqlConnection);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        vehicle = new Vehicle();
                        customer = new Customer();
                        rent = new Rent();
                        vehicle.VehicleName = Convert.ToString(reader.GetSqlValue(0));
                        vehicle.VehiclePrice = reader.GetInt32(1);
                        vehicle.VehicleInventory = Convert.ToBoolean(reader.GetBoolean(2));
                        vehicle.VehicleFlatNumber = Convert.ToString(reader.GetSqlValue(3));
                        vehicle.VehicleModel = Convert.ToString(reader.GetSqlValue(4));
                        vehicle.VehicleStatus = Convert.ToString(reader.GetSqlValue(5));
                        vehicle.VehicleImage = Convert.ToString(reader.GetSqlValue(6));
                        customer.CustomerName = Convert.ToString(reader.GetSqlValue(7));
                        customer.CustomerEmail = Convert.ToString(reader.GetSqlValue(8));
                        customer.CustomerAddress = Convert.ToString(reader.GetSqlValue(9));
                        customer.CustomerPhone = Convert.ToString(reader.GetSqlValue(10));
                        customer.CustomerID = Convert.ToString(reader.GetSqlValue(11));
                        rent.RentID = Convert.ToString(reader.GetSqlValue(12));
                        rent.RentCreatAt = Convert.ToString(reader.GetSqlValue(13));
                        rent.RentTotalPrice = reader.GetInt32(14);
                        rent.RentStatus = Convert.ToString(reader.GetSqlValue(15));
                        vehiclesRent.Add(vehicle);
                        customerRent.Add(customer);
                        rentList.Add(rent);
                    }

                    //oSqlCommand.CommandType = CommandType.Text;
                    //oSqlCommand.ExecuteNonQuery();

                    this.rentDB = new ViewModels
                    {
                        Vehicle = vehiclesRent.ToList(),
                        Customer = customerRent.ToList(),
                        Rent = rentList.ToList(),

                    };

                    ViewBag.id = id;

                    return View(rentDB);
                }
                catch (Exception ex)
                {
                    return Json(new { Msg = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    oSqlCommand.Dispose();
                    oSqlConnection.Close();
                    oSqlConnection.Dispose();
                }
            }
        }

        // GET: Manager/Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Order/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Manager/Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Manager/Order/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Manager/Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Manager/Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
