using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JProfSiteLink
    {
        public string siteprofid { set; get; }
        public string siteid { set; get; }
        public DateTime linksdate { set; get; }
        public DateTime linkedate { set; get; }

        public JSite Site { set; get; }
    }
}