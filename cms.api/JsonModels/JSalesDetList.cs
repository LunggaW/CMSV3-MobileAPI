using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSalesDetList
    {
        public virtual IEnumerable<JSalesDetail> SalesDetailLists { get; set; }

    }
}