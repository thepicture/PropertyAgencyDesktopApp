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
    
    public partial class Demand
    {
        public int ClientId { get; set; }
        public int AgentId { get; set; }
        public int PropertyId { get; set; }
    
        public virtual Agent Agent { get; set; }
        public virtual Client Client { get; set; }
        public virtual Property Property { get; set; }
    }
}