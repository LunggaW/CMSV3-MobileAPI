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
    
    public partial class KDSCMSSITELINK
    {
        public string STLNUSER { get; set; }
        public string STLNSITE { get; set; }
        public System.DateTime STLNSDAT { get; set; }
        public System.DateTime STLNEDAT { get; set; }
        public Nullable<System.DateTime> STLNCDAT { get; set; }
        public Nullable<System.DateTime> STLNMDAT { get; set; }
        public string STLNCRBY { get; set; }
        public string STLNMOBY { get; set; }
        public Nullable<decimal> STLNINTF { get; set; }
        public string STLNCOMP { get; set; }
        public Nullable<decimal> STLNNMOD { get; set; }
    
        public virtual KDSCMSUSER KDSCMSUSER { get; set; }
    }
}