using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSkuHeader
    {
        public int skuid { set; get; }
        public string skuidx { set; get; }
        public string skusdesc { set; get; }
        public string skuldesc { set; get; }
        public DateTime skusdate { set; get; }
        public DateTime skuedate { set; get; }
        //public virtual ICollection<JSkuDetail> JSkuDetail { get; set; }
    }
}