using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JSalesHeaderLists
    {
        public virtual IEnumerable<JSalesHeader> SalesHeaderLists { get; set; }

    }
}