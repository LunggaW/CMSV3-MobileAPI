//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cms.api.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class KDSCMSSPRICE
    {
        public int SPRCITEMID { get; set; }
        public int SPRCVRNTID { get; set; }
        public string SPRCSITE { get; set; }
        public decimal SPRCPRICE { get; set; }
        public short SPRCVAT { get; set; }
        public System.DateTime SPRCSDAT { get; set; }
        public System.DateTime SPRCEDAT { get; set; }
        public Nullable<short> SPRCINTF { get; set; }
        public System.DateTime SPRCCDAT { get; set; }
        public System.DateTime SPRCMDAT { get; set; }
        public string SPRCCRBY { get; set; }
        public string SPRCMOBY { get; set; }
        public long SPRCNMOD { get; set; }
    
        public virtual KDSCMSSITE KDSCMSSITE { get; set; }
    }
}
