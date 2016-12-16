using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSalesHeaderView
    {

        public enum SalesTypeEnum { Sales = 1, Return = 2 }
        public enum SalesStatusEnum { Created, Validated, Approved, Rejected, Deleted }

        public string SLSHSLNOTA { set; get; }
        public string SLSHSITE { set; get; }
        public string SLSHCRBY { set; get; }
        public string SLSHLDATE { set; get; }
        public string SLSHFLAG { set; get; }
        public string SLSHSTAT { set; get; }
        public string SLSHTOTAMT { set; get; }
        
        
        
        
    }
}