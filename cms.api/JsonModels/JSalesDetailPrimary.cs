using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSalesDetailPrimary
    {

        public string nota { set; get; }
        public string site { set; get; }
        public string user { set; get; }
        public string barcode { set; get; }
        
        public DateTime date { set; get; }

    }
}