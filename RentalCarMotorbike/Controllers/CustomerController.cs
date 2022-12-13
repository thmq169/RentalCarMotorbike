using RentalCarMotorbike.Models;
using RentalCarMotorbike.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalCarMotorbike.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private List<Vehicle> vehiclesRent = new List<Vehicle>();
        private List<Customer> customerRent = new List<Customer>();
        private List<Rent> rentList = new List<Rent>();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rented(string id)
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                var returnedVehicles = db.Rent.Where(r => r.CustomerID == id && (r.RentStatus == "Returned" || r.RentStatus == "Failed" || r.RentStatus == "Decline"));
                ViewModels viewModels = new ViewModels();
                foreach (var vehicleItem in returnedVehicles)
                {
                    var details = db.RentDetail.Where(d => d.CustomerID == vehicleItem.CustomerID && d.RentID == vehicleItem.RentID).FirstOrDefault();
                    var vehicleDetail = db.Vehicle.Find(details.VehicleID);

                    rentList.Add(vehicleItem);
                    vehiclesRent.Add(vehicleDetail);

                }
                    viewModels.Rent = rentList.ToList();
                    viewModels.Vehicle = vehiclesRent.ToList();
                return View(viewModels);
            }
        }

        public ActionResult Renting(string id)
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                var returnedVehicles = db.Rent.Where(r => r.CustomerID == id && (r.RentStatus == "Renting" || r.RentStatus == "Processing"));
                ViewModels viewModels = new ViewModels();
                foreach (var vehicleItem in returnedVehicles)
                {
                    var details = db.RentDetail.Where(d => d.CustomerID == vehicleItem.CustomerID && d.RentID == vehicleItem.RentID).FirstOrDefault();
                    var vehicleDetail = db.Vehicle.Find(details.VehicleID);

                    rentList.Add(vehicleItem);
                    vehiclesRent.Add(vehicleDetail);

                }
                viewModels.Rent = rentList.ToList();
                viewModels.Vehicle = vehiclesRent.ToList();
                return View(viewModels);
            }
        }

        public ActionResult Return(string id, string idRent)
        {
            using (var db = new RentalVehicalDB())
            {

                Vehicle vehicle = db.Vehicle.Find(id);
                //Customer customer = db.Customer.Find(Session["userID"]);
                Rent rent = db.Rent.Find(idRent);

                var viewModel = new ViewModels
                {
                    vehicle = vehicle,
                    rent = rent,
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Return(FormCollection collection)
        {
            try
            {
                using (RentalVehicalDB db = new RentalVehicalDB())
                {
                    string vehicleId = collection["vehicle-id"];
                    /*string vehicleName = collection["vehicle-name"];
                    string vehicleFlat = collection["vehicle-flat"];
                    string vehiclePrice = collection["vehicle-price"];
                    string startDate = collection["rent-start-date"];
                    string returnDate = collection["rent-return-date"];
                    string numberDaysRent = collection["rent-number-days"];
                    string rentTotalCost = collection["rent-total-cost"];*/
                    string rentID = collection["rent-id"];
                    Customer customer = db.Customer.Find(Session["userID"]);
                    db.Rent.Where(r => r.CustomerID == customer.CustomerID && r.RentID == rentID).FirstOrDefault().RentStatus = "Returning";
                    
                    db.Vehicle.Find(vehicleId).VehicleStatus = "Returning";
                    
                    db.SaveChanges();

                    return Json(new { code = 0, message = "Success", id = vehicleId, idRent = rentID }, JsonRequestBehavior.AllowGet); ;
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ReturnConfirm(string id, string idRent)
        {
            using (var db = new RentalVehicalDB())
            {

                Vehicle vehicle = db.Vehicle.Find(id);
                Customer customer = db.Customer.Find(Session["userID"]);
                Rent rent = db.Rent.Where(r => r.RentID == idRent && r.CustomerID == customer.CustomerID).FirstOrDefault();

                var viewModel = new ViewModels
                {
                    vehicle = vehicle,
                    rent = rent
                };
                return View(viewModel);
            }
        }
    }
}
