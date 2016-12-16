using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.api.JsonModels
{
    public class JTransLists
    {
        public virtual IEnumerable<JSales> TransactionLists { get; set; }

    }
}