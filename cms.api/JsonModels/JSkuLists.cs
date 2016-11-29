using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSkuLists
    {
        public virtual IEnumerable<JSkuList> SkuList { get; set; }
    }
}