using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSales
    {
        public int transid { set; get; }
        public string transnota { set; get; }
        public string transsite { set; get; }
        public DateTime transdate { set; get; }
        public string transbrcd { set; get; }
        public int transsku { set; get; }
        public int transqty { set; get; }
        public decimal transprice { set; get; }
        public decimal transamt { set; get; }
        public short transstat { set; get; }
        public short transtype { set; get; }
        public short transflag { set; get; }
        public DateTime transdcre { set; get; }
        public string transcreby { set; get; }

        //Update GAGAN
        public int transdiscount { set; get; }
        public decimal transfinalprice { set; get; }
    }
}