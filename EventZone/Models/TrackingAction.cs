//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventZone.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TrackingAction
    {
        public long TrackingID { get; set; }
        public long SenderID { get; set; }
        public long ReceiverID { get; set; }
        public int SenderType { get; set; }
        public Nullable<int> ReceiverType { get; set; }
        public int ActionID { get; set; }
        public System.DateTime ActionTime { get; set; }
    
        public virtual Action Action { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
