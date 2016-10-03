using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSiteProfile
    {
        public string siteprofid { set; get; }
        public string siteprofdesc { set; get; }
        public virtual IEnumerable<JProfSiteLink> ProfileSiteLinks { get; set; }
        //public virtual IEnumerable<JUser> JUser { get; set; }
    }
}