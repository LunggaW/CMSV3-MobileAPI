using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JUser
    {
        public string userid { set; get; }
        public string username { set; get; }
        public string userdesc { set; get; }
        public short userstatus { set; get; }
        public short usertype { set; get; }
        public DateTime userstartdate { set; get; }
        public DateTime userenddate { set; get; }
        public string siteprofid { set; get; }
        public string useraccprofile { set; get; }
        public string usermenuprofile { set; get; }
        public JSiteProfile SiteProfile { set; get; }
    }
}