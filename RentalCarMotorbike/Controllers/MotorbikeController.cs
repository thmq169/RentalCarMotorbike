using RentalCarMotorbike.Models;
using RentalCarMotorbike.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalCarMotorbike.Controllers
{
    public class MotorbikeController : Controller
    {
        [Authorize]
        // GET: Motorbike
        public ActionResult Index()
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                var cars = db.Vehicle.Where(v => v.VehicleStatus == "Available" && v.VehicleModel == "Motorbike");

                return View(cars.ToList());
            }
        }

        public ActionResult Details(string id)
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                Vehicle vehicle = new Vehicle();
                vehicle = db.Vehicle.Find(id);
                return View(vehicle);
            }
        }

        public ActionResult Rent(string id)
        {

            using (var db = new RentalVehicalDB())
            {

                Vehicle vehicle = db.Vehicle.Find(id);
                Customer customer = db.Customer.Find(Session["userID"]);

                var viewModel = new ViewModels
                {
                    vehicle = vehicle,
                    customer = customer,
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Rent(string id, FormCollection collection)
        {
            try
            {
                using (RentalVehicalDB db = new RentalVehicalDB())
                {

                    string vehicleId = collection["vehicle-id"];
                    string vehicleName = collection["vehicle-name"];
                    string vehicleFlat = collection["vehicle-flat"];
                    int vehiclePrice = Convert.ToInt32(collection["vehicle-price"]);
                    string customerId = collection["customer-id"];
                    string customerName = collection["customer-name"];
                    string customerPhone = collection["customer-phone"];
                    string customrtAddress = collection["customer-address"];
                    string startDate = collection["rent-start-date"];
                    string returnDate = collection["rent-return-date"];
                    DateTime FromYear = Convert.ToDateTime(startDate);
                    DateTime ToYear = Convert.ToDateTime(returnDate);
                    TimeSpan objTimeSpan = ToYear - FromYear;
                    int Days = Convert.ToInt32(objTimeSpan.TotalDays);
                    string rentID = Convert.ToString(db.Rent.Count() + 1) + vehicleId.Substring(4).Trim();
                    Rent rent = new Rent
                    {
                        RentID = rentID,
                        CustomerID = db.Customer.Find(customerId).CustomerID,
                        RentCreatAt = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                        RentStatus = "Processing",
                        RentPickUpDate = startDate,
                        RentReturnDate = returnDate,
                        RentTotalPrice = Days*vehiclePrice,

                    };
                    db.Rent.Add(rent);

                    RentDetail renDetail = new RentDetail
                    {
                        VehicleID = vehicleId,
                        CustomerID = customerId,
                        RentID = rentID,
                    };

                    db.RentDetail.Add(renDetail);
                    //db.Vehicle.Find(vehicleId).VehicleStatus = "Renting";
                    db.SaveChanges();

                    Session["RentConfirm"] = rentID;

                    return Json(new { code = 0, message = "Success", idRent = rentID }, JsonRequestBehavior.AllowGet); ;
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Car/Edit/5
        public ActionResult RentConfirm(string id)
        {
            using (var db = new RentalVehicalDB())
            {
                Rent rent = db.Rent.Find(id);
                Customer customer = db.Customer.Find(rent.CustomerID);
                RentDetail rentDetail = db.RentDetail.Where(rdt => rdt.RentID == rent.RentID && rdt.CustomerID == customer.CustomerID).FirstOrDefault();
                Vehicle vehicle = db.Vehicle.Find(rentDetail.VehicleID);

                var viewModels = new ViewModels
                {
                    customer = customer,
                    vehicle = vehicle,
                    rent = rent,
                    rentDetail = rentDetail
                };


                return View(viewModels);
            }
        }
    }
}
