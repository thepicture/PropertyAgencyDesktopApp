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
    
    public partial class Deal
    {
        public int DealId { get; set; }
        public int OfferId { get; set; }
    
        public virtual Offer Offer { get; set; }
    }
}