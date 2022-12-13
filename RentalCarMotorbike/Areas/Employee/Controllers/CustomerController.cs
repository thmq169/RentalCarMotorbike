using RentalCarMotorbike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalCarMotorbike.Areas.Employee.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                return View(db.Customer.ToList());
            }
        }
        public ActionResult Details(string id)
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                Customer customer = new Customer();
                customer = db.Customer.Find(id);
                return View(customer);
            }
        }
        //[HttpPost]
        public JsonResult Search(string searchValue, string searchSelect)
        {
            using (var db = new RentalVehicalDB())
            {
                List<Customer> customers = new List<Customer>();
                if(searchSelect == "name")
                {
                    customers = db.Customer.Where(c => c.CustomerName.Contains(searchValue) || searchValue == null).ToList();
                    
                    return Json(customers,JsonRequestBehavior.AllowGet);
                }
                else if(searchSelect == "address")
                {
                    try
                    {
                        customers = db.Customer.Where(c => c.CustomerAddress.Contains(searchValue) || searchValue == null).ToList();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Not Found", searchValue);
                    }
                    return Json(customers, JsonRequestBehavior.AllowGet);
                }
                else if (searchSelect == "email")
                {
                    try
                    {
                        customers = db.Customer.Where(c => c.CustomerEmail.Contains(searchValue) || searchValue == null).ToList();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Not Found", searchValue);
                    }
                    return Json(customers, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    customers = db.Customer.Where(c => c.CustomerPhone.Contains(searchValue) || searchValue == null).ToList();
                    return Json(customers, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
