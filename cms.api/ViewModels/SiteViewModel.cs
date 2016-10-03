using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.ViewModels
{
    public class SiteViewModel
    {
        public string siteid { set; get; }
        public short siteclass { set; get; }
        public string sitename { set; get; }
        public decimal siteflag { set; get; }
        public decimal sitestatus { set; get; }
    }
}