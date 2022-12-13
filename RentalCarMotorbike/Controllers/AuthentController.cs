using RentalCarMotorbike.Common;
using RentalCarMotorbike.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BCrypt.Net;
using static System.Collections.Specialized.BitVector32;

namespace RentalCarMotorbike.Controllers
{
    public class AuthentController : Controller
    {
        // GET: Authent
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account account)
        {
            using (RentalVehicalDB db = new RentalVehicalDB())
            {
                string role = account.Role;
                if (role == "Customer")
                {
                    var customer = db.Customer.Where(x => x.CustomerEmail == account.Email).FirstOrDefault();
                    bool isValidPassword = BCrypt.Net.BCrypt.Verify(account.Password, customer.CustomerPassword);
                    if (!isValidPassword || customer == null)
                    {
                        ModelState.AddModelError("login-error", "*Invalid email or password");
                        return View();
                    }
                    else
                    {
                        Session["userID"] = customer.CustomerID;
                        Session.Remove("RentConfirm");
                        FormsAuthentication.SetAuthCookie(customer.CustomerName, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (role == "Employee")
                {

                    var employee = db.Employee.Where(e => e.EmployeeEmail == account.Email).FirstOrDefault();
                    bool isValidPassEmp = BCrypt.Net.BCrypt.Verify(account.Password, employee.EmployeePassword);
                    if (!isValidPassEmp || employee == null)
                    {
                        //user.LoginErrorMsg = "Invalid UserName or Password";
                        ModelState.AddModelError("login-error", "*Invalid email or password");
                        return View();
                    }
                    else
                    {
                        Session["empID"] = employee.EmployeeID;
                        Session.Remove("RentConfirm");
                        FormsAuthentication.SetAuthCookie(employee.EmployeeName, false);
                        return RedirectToAction("Index", "Rent", new { area = "Employee" });
                    }
                }
               
                    if (account.Email == "admin@gmail.com" && account.Password == "admin")
                    {
                        Session["admin"] = "admin";
                        return RedirectToAction("Index", "Vehicle", new { area = "Manager" });
                    }

            }
            ModelState.AddModelError("login-error", "*Invalid email or password");
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Customer customer)
        {
              using (RentalVehicalDB db = new RentalVehicalDB())
                {
                    var account = db.Customer.Where(c => c.CustomerEmail == customer.CustomerEmail || c.CustomerID == customer.CustomerID).FirstOrDefault();
                    if(account == null)
                    {
                        if (ModelState.IsValid)
                        {
                            customer.CustomerPassword = BCrypt.Net.BCrypt.HashPassword(customer.CustomerPassword, 12);
                            customer.CustomerCreateAt = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                                            db.Customer.Add(customer);
                                            db.SaveChanges();
                               return RedirectToAction("Login");
                        }
                    }
                    
                     ModelState.AddModelError("signup-error", "*Acount has been register");
                     return View();
                    
                }
           
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToAction("Login");
        }
    }
}