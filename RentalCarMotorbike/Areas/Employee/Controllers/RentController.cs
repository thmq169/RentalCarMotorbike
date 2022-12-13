using RentalCarMotorbike.Models;
using RentalCarMotorbike.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalCarMotorbike.Areas.Employee.Controllers
{
    public class RentController : Controller
    {
        private List<Vehicle> vehiclesRent = new List<Vehicle>();
        private List<Customer> customerRent = new List<Customer>();
        private List<Rent> rentList = new List<Rent>();
        // GET: Employee/Rent
        public ActionResult Index()
        {
            using (var db = new RentalVehicalDB())
            {
                var rents = db.Rent.ToList();
                
                foreach(var r in rents)
                {
                    var rentDetails = db.RentDetail.Where(rd => rd.RentID == r.RentID && rd.CustomerID == r.CustomerID).FirstOrDefault();
                    var vehicle = db.Vehicle.Find(rentDetails.VehicleID);
                    var customer = db.Customer.Find(rentDetails.CustomerID);
                    vehiclesRent.Add(vehicle);
                    customerRent.Add(customer);
                    rentList.Add(r);
                }

                var viewModels = new ViewModels
                {
                    Vehicle = vehiclesRent.ToList(),
                    Customer = customerRent.ToList(),
                    Rent = rentList.ToList(),
                };

                return View(viewModels);
            }
        }

        /*public ActionResult GetAllRent()
        {
            using (var db = new RentalVehicalDB())
            {
                
                return View(db.Rent);
            }
        }*/

        // GET: Employee/Rent/Details/5
        public ActionResult Details(string id, string idCus, string idVeh)
        {
            using(var db = new RentalVehicalDB())
            {
                var rent = db.Rent.Where(r => r.RentID == id && r.CustomerID == idCus).FirstOrDefault();
                var vehicle = db.Vehicle.Find(idVeh);
                var customer = db.Customer.Find(idCus);

                var viewModels = new ViewModels
                {
                    vehicle = vehicle,
                    customer = customer,
                    rent = rent
                };

                return View(viewModels);
            }
        }
        
        [HttpPost]
        public ActionResult Accept(FormCollection collection)
        {
            using(var db = new RentalVehicalDB())
            {
                string idRent = collection["id-rent"].ToString();
                string idCustomer = collection["id-customer"].ToString();
                string idVehicle = collection["id-vehicle"].ToString();
                string idEmployee = collection["id-employee"].ToString();

                var rent = db.Rent.Where(r => r.RentID == idRent && r.CustomerID == idCustomer).FirstOrDefault();
                var rentDetail = db.RentDetail.Where(r => r.RentID == rent.RentID && r.CustomerID == rent.CustomerID).FirstOrDefault();
                var vehicle = db.Vehicle.Find(rentDetail.VehicleID);
                var employee = db.Employee.Find(idEmployee);
                rent.EmployeeName = employee.EmployeeName;
                rent.EmployeePhone = employee.EmployeePhone;
                rent.RentStatus = "Renting";
                vehicle.VehicleStatus = "Renting";
                db.SaveChanges();

                return Json(new { Msg = "Accepted", idRent = rent.RentID }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Decline(FormCollection collection)
        {
            using (var db = new RentalVehicalDB())
            {
                string idRent = collection["id-rent"].ToString();
                string idCustomer = collection["id-customer"].ToString();
                string idVehicle = collection["id-vehicle"].ToString();
                string idEmployee = collection["id-employee"].ToString();

                var rent = db.Rent.Where(r => r.RentID == idRent && r.CustomerID == idCustomer).FirstOrDefault();
                var rentDetail = db.RentDetail.Where(r => r.RentID == rent.RentID && r.CustomerID == rent.CustomerID).FirstOrDefault();
                var vehicle = db.Vehicle.Find(rentDetail.VehicleID);
                var employee = db.Employee.Find(idEmployee);
                rent.EmployeeName = employee.EmployeeName;
                rent.EmployeePhone = employee.EmployeePhone;
                
                if(vehicle.VehicleStatus == "Available")
                {
                    rent.RentStatus = "Decline";
                    vehicle.VehicleStatus = "Noavailable";
                }
                else if(vehicle.VehicleStatus == "Renting")
                {
                    rent.RentStatus = "Failed";
                }
                db.SaveChanges();

                return Json(new { Msg = "Decline" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ConfirmBill(string id)
        {
            using(var db = new RentalVehicalDB())
            {
                var rent = db.Rent.Find(id);
                var customer = db.Customer.Find(rent.CustomerID);
                var renDetail = db.RentDetail.Where(r => r.RentID == rent.RentID && r.CustomerID == customer.CustomerID).FirstOrDefault();
                var vehicle = db.Vehicle.Find(renDetail.VehicleID);
                var employee = db.Employee.Find(Session["empId"]);
                rent.EmployeeName = employee.EmployeeName;
                rent.EmployeePhone = employee.EmployeePhone;
                if(rent.RentStatus == "Processing")
                {
                    rent.RentStatus = "Renting";
                    vehicle.VehicleStatus = "Renting";
                }else if (rent.RentStatus == "Renting" || rent.RentStatus == "Returning")
                {
                    rent.RentStatus = "Returned";
                    vehicle.VehicleStatus = "Available";
                }
                var viewModels = new ViewModels
                {
                    rent = rent,
                    customer = customer,
                    rentDetail = renDetail,
                    vehicle = vehicle
                };
                db.SaveChanges();

                return View(viewModels);
            }   
        }
    }
}
