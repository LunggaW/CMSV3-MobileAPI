using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSkuLink
    {
        public int skuid { set; get; }
        public string siteid { set; get; }
        public string brandid { set; get; }
        public DateTime linksdate { set; get; }
        public DateTime linkedate { set; get; }
        public JBrand JBrand { set; get; }
        public JSkuHeader SkuHeader { set; get; }
    }
}