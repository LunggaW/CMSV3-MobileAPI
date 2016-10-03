using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.ViewModels
{
    public class SiteProfileViewModel
    {
        public string profileid { set; get; }
        public string profiledesc { set; get; }
        public string siteid { set; get; }
        public short siteclass { set; get; }
        public string sitename { set; get; }
        public short siteflag { set; get; }
        public short sitestatus { set; get; }
    }
}