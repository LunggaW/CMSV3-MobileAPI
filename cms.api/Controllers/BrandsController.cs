using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using cms.api.Models;
using cms.api.ViewModels;
using Newtonsoft.Json.Linq;
using NLog;

namespace cms.api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Brands")]
    public class BrandsController : ApiController
    {
        private CMSContext db = new CMSContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Route("")]
        [HttpGet]
        public IEnumerable<BrandViewModel> GetSkus()
        {
            try
            {
                IEnumerable<BrandViewModel> brands = from brand in db.KDSCMSMSTBRND
                                                     select new BrandViewModel
                                                     {
                                                         brandid = brand.BRNDBRNDID,
                                                         branddesc = brand.BRNDDESC
                                                     }
            ;
                return brands;
            }
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }
            
        }

        //[Route("")]
        //[HttpPost]
        //public IEnumerable<BrandViewModel> PostSiteSkus([FromBody]JObject data)
        //{
        //    try
        //    {
        //        string userid = data["userid"].ToString();
        //        IEnumerable<BrandViewModel> brands = from allbrand in (from brand in db.KDSCMSMSTBRND
        //                                                               from skulink in db.KDSCMSSKULINK
        //                                                               from proflink in db.KDSCMSPROFSITELINK
        //                                                               from user in db.KDSCMSUSER
        //                                                               where
        //                                                               brand.BRNDBRNDID == skulink.SKULINKBRNDID &&
        //                                                               skulink.SKULINKSITEID == proflink.PRSTSITE &&
        //                                                               proflink.PRSTSTPROF == user.USERSTPROF &&
        //                                                               user.USERUSID == userid &&
        //                                                               DbFunctions.TruncateTime(skulink.SKULINKSDATE) <= DbFunctions.TruncateTime(DateTime.Today) &&
        //                                                               DbFunctions.TruncateTime(skulink.SKULINKEDATE) >= DbFunctions.TruncateTime(DateTime.Today) &&
        //                                                               DbFunctions.TruncateTime(proflink.PRSTSDAT) <= DbFunctions.TruncateTime(DateTime.Today) &&
        //                                                               DbFunctions.TruncateTime(proflink.PRSTEDAT) >= DbFunctions.TruncateTime(DateTime.Today)
        //                                                               select new { brand.BRNDBRNDID, brand.BRNDDESC }
        //                                            ).Distinct()
        //                                             select new BrandViewModel
        //                                             {
        //                                                 brandid = allbrand.BRNDBRNDID,
        //                                                 branddesc = allbrand.BRNDDESC
        //                                             }
        //        ;
        //        return brands;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Error Message" + ex.Message);
        //        logger.Error("Inner Exception" + ex.InnerException);
        //        throw;
        //    }
            
        //}
    }
}
