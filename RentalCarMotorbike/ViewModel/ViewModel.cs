using RentalCarMotorbike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalCarMotorbike.ViewModel
{
    public class ViewModels
    {
        public IList<Vehicle> Vehicle { get; set; }
        public IList<Customer> Customer { get; set; }
        public IList<Rent> Rent { get; set; }

        public IList<RentDetail> RentDetail { get; set; }

        public Vehicle vehicle { get; set; }
        public Customer customer { get; set; }

        public Rent rent { get; set; }
        public RentDetail rentDetail { get; set; }
        public RentDetail employee { get; set; }
    }
}