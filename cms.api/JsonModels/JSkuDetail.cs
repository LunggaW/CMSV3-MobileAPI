using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSkuDetail
    {
        public int skuid { set; get; }
        public int skudetid { set; get; }
        public string skudetidx { set; get; }
        public string skudetname { set; get; }
        public short skudetlvl { set; get; }
        public decimal skudetval { set; get; }
        public Nullable<decimal> skudetpart { set; get; }
        public short skudetbson { set; get; }
        public short skudettype { set; get; }
    }
}