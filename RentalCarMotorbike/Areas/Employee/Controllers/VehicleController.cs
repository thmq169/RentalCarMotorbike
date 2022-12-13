using RentalCarMotorbike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalCarMotorbike.Areas.Employee.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Employee/Vehicle
        public ActionResult Index()
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                return View(db.Vehicle.ToList());
            }
        }

        // GET: Employee/Vehicle/Details/5
        public ActionResult Details(string id)
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                Vehicle vehicle = new Vehicle();
                vehicle = db.Vehicle.Find(id);
                return View(vehicle);
            }
        }

        // GET: Employee/Vehicle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Vehicle/Create
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

        // GET: Employee/Vehicle/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Vehicle/Edit/5
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

        // GET: Employee/Vehicle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Vehicle/Delete/5
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
