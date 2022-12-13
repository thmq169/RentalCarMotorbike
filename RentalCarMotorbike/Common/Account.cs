using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RentalCarMotorbike.Common
{
    public class Account
    {

        public string ID { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        [DisplayName("You are:")]
        public string Role { get; set; }
    }
}