using cms.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using NLog;

namespace cms.api.Controllers
{
    [Authorize]
    public class IdentityController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public IEnumerable<ViewClaim> Get()
        {
            try
            {
                var principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

                return from c in principal.Claims
                       select new ViewClaim
                       {
                           Type = c.Type,
                           Value = c.Value
                       };
            }
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }
            
        }
    }
}
