using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.ViewModels
{
    public class SkuDetailViewModel
    {
        public int skudetid { set; get; }
        public string skudetidx { set; get; }
        public string skudetname { set; get; }
        public short skudetlvl { set; get; }
        public decimal skudetval { set; get; }
        public short skudetvaltype { set; get; }
        public Nullable<decimal> skudetpart { set; get; }
        public short skudetbasedon { set; get; }
    }
}