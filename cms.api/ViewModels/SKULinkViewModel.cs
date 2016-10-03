using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.ViewModels
{
    public class SKULinkViewModel
    {
        public string siteid { set; get; }
        public string brandid { set; get; }
        public int skuid { set; get; }
        public string skuidx { set; get; }
        public string skusdesc { set; get; }
        public string skuldesc { set; get; }
        public DateTime skusdate { set; get; }
        public DateTime skuedate { set; get; }
        public DateTime skulinksdate { set; get; }
        public DateTime skulinkedate { set; get; }
    }
}