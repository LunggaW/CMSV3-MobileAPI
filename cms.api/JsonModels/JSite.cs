using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSite
    {
        public string siteid { set; get; }
        public short siteclass { set; get; }
        public string sitename { set; get; }
        public short siteflag { set; get; }
        public short sitestatus { set; get; }
        public virtual ICollection<JSkuLink> SkuLink { get; set; }
    }
}