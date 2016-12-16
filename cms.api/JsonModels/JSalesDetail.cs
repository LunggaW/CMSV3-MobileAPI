using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSalesDetail
    {

        public enum SalesTypeEnum { Sales = 1, Return = 2 }
        public enum SalesStatusEnum { Created, Validated, Approved, Rejected, Deleted }

        public string nota { set; get; }
        public string site { set; get; }
        public string userApps { set; get; }
        public DateTime salesdate { set; get; }
        public string barcode { set; get; }
        public int qty { set; get; }
        public Decimal totalamount { set; get; }
        public string itemid{ set; get; }
        public string variant { set; get; }
        public string description { set; get; }
        public string sku { set; get; }
        public Decimal gross { set; get; }
        public SalesTypeEnum SalesType { set; get; }
        public SalesStatusEnum SalesStatus { set; get; }
    }
}