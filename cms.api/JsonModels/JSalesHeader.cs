using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSalesHeader
    {

        public enum SalesTypeEnum { Sales = 1, Return = 2 }
        public enum SalesStatusEnum { Created, Validated, Approved, Rejected, Deleted }

        public string nota { set; get; }
        public string site { set; get; }
        public string user { set; get; }
        public string barcode { set; get; }
        public DateTime date { set; get; }
        public Decimal totalamount { set; get; }
        public SalesTypeEnum SalesType { set; get; }
        public SalesStatusEnum SalesStatus { set; get; }
    }
}