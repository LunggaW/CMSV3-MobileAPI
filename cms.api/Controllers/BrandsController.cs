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
namespace cms.api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Brands")]
    public class BrandsController : ApiController
    {
        private CMSContext db = new CMSContext();

        [Route("")]
        [HttpGet]
        public IEnumerable<BrandViewModel> GetSkus()
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
        [Route("")]
        [HttpPost]
        public IEnumerable<BrandViewModel> PostSiteSkus([FromBody]JObject data)
        {
            string userid = data["userid"].ToString();
            IEnumerable<BrandViewModel> brands = from allbrand in (from brand in db.KDSCMSMSTBRND
                                                from skulink in db.KDSCMSSKULINK
                                                from proflink in db.KDSCMSPROFSITELINK
                                                from user in db.KDSCMSUSER
                                                where
                                                brand.BRNDBRNDID == skulink.SKULINKBRNDID &&
                                                skulink.SKULINKSITEID == proflink.PRSTSITE &&
                                                proflink.PRSTSTPROF == user.USERSTPROF &&
                                                user.USERUSID == userid &&
                                                DbFunctions.TruncateTime(skulink.SKULINKSDATE) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                                DbFunctions.TruncateTime(skulink.SKULINKEDATE) >= DbFunctions.TruncateTime(DateTime.Today) &&
                                                DbFunctions.TruncateTime(proflink.PRSTSDAT) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                                DbFunctions.TruncateTime(proflink.PRSTEDAT) >= DbFunctions.TruncateTime(DateTime.Today)
                                                select new {brand.BRNDBRNDID, brand.BRNDDESC}
                                                ).Distinct()
                                                select new BrandViewModel
                                                {
                                                    brandid = allbrand.BRNDBRNDID,
                                                    branddesc = allbrand.BRNDDESC
                                                }
            ;
            return brands;
        }
    }
}
