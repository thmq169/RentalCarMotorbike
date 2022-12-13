//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentalCarMotorbike.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Vehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle()
        {
            this.RentDetail = new HashSet<RentDetail>();
        }
        [DisplayName("Vehicle Identity")] 
        public string VehicleID { get; set; }
        [DisplayName("Vehicle Name")]
        public string VehicleName { get; set; }
        [DisplayName("Vehicle Cost")]
        public Nullable<int> VehiclePrice { get; set; }
        [DisplayName("Vehicle Checking")]
        public Nullable<bool> VehicleInventory { get; set; }
        [DisplayName("Vehicle Flat Number")]
        public string VehicleFlatNumber { get; set; }
        [DisplayName("Vehicle Model")]
        public string VehicleModel { get; set; }
        [DisplayName("Vehicle Status")]
        public string VehicleStatus { get; set; }
        public string VehicleImage { get; set; }
        [DisplayName("Vehicle Description")]
        public string VehicleDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RentDetail> RentDetail { get; set; }
    }
}
