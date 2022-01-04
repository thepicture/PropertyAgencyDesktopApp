//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PropertyAgencyDesktopApp.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PropertyAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertyAddress()
        {
            this.Property = new HashSet<Property>();
            this.Demand = new HashSet<Demand>();
        }
    
        public int Id { get; set; }
        public Nullable<int> AddressCityId { get; set; }
        public Nullable<int> DistrictId { get; set; }
        public string AddressHouse { get; set; }
        public string AddressNumber { get; set; }
    
        public virtual City City { get; set; }
        public virtual District District { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Property> Property { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Demand> Demand { get; set; }
    }
}
