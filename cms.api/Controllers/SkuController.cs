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
    [RoutePrefix("api/Sku")]
    public class SkuController : ApiController
    {
        private CMSContext db = new CMSContext();

        [Route("")]
        [HttpGet]
        public IEnumerable<SkuHeaderViewModel> GetSkus()
        {
            IEnumerable<SkuHeaderViewModel> headers = from header in db.KDSCMSSKUH
                                               select new SkuHeaderViewModel
                                               {
                                                   skuid = header.SKUHSKUID,
                                                   skuidx = header.SKUHSKUIDX,
                                                   skusdesc = header.SKUHSDES,
                                                   skuldesc = header.SKUHLDES,
                                                   skusdate = header.SKUHSDAT,
                                                   skuedate = header.SKUHEDAT
                                               }
            ;
            return headers;
        }

        [Route("site")]
        [HttpPost]
        public IEnumerable<SKULinkViewModel> PostSiteSkus([FromBody]JObject data)
        {
            string siteid = data["siteid"].ToString();
            DateTime salesdate = Convert.ToDateTime(data["salesdate"].ToString());
            IEnumerable<SKULinkViewModel> skulinks = from skulink in db.KDSCMSSKULINK
                                                     from sku in db.KDSCMSSKUH
                                                     where 
                                                        skulink.SKULINKSITEID == siteid &&
                                                        skulink.SKULINKSKUID == sku.SKUHSKUID &&
                                                        DbFunctions.TruncateTime(skulink.SKULINKSDATE) <= DbFunctions.TruncateTime(salesdate) &&
                                                        DbFunctions.TruncateTime(skulink.SKULINKEDATE) >= DbFunctions.TruncateTime(salesdate) &&
                                                        DbFunctions.TruncateTime(sku.SKUHSDAT) <= DbFunctions.TruncateTime(salesdate) &&
                                                        DbFunctions.TruncateTime(sku.SKUHEDAT) >= DbFunctions.TruncateTime(salesdate)
                                                      select new SKULinkViewModel
                                                      {
                                                          siteid = skulink.SKULINKSITEID,
                                                          brandid = skulink.SKULINKBRNDID,
                                                          skuid = skulink.SKULINKSKUID,
                                                          skuidx = sku.SKUHSKUIDX,
                                                          skusdesc = sku.SKUHSDES,
                                                          skuldesc = sku.SKUHLDES,
                                                          skusdate = sku.SKUHSDAT,
                                                          skuedate = sku.SKUHEDAT,
                                                          skulinksdate = skulink.SKULINKSDATE,
                                                          skulinkedate = skulink.SKULINKEDATE
                                                      }
            ;
            return skulinks;
        }

        [Route("{id}")]
        [HttpGet]
        [ResponseType(typeof(SkuHeaderViewModel))]
        public IHttpActionResult GetSites(int id)
        {
            KDSCMSSKUH KDSCMSSKUH = db.KDSCMSSKUH.Find(id);
            if (KDSCMSSKUH == null)
            {
                return NotFound();
            }
            SkuHeaderViewModel header = new SkuHeaderViewModel();
            header.skuid = KDSCMSSKUH.SKUHSKUID;
            header.skuidx = KDSCMSSKUH.SKUHSKUIDX;
            header.skusdesc = KDSCMSSKUH.SKUHSDES;
            header.skuldesc = KDSCMSSKUH.SKUHLDES;
            header.skusdate = KDSCMSSKUH.SKUHSDAT;
            header.skuedate = KDSCMSSKUH.SKUHEDAT;
            return Ok(header);
        }

        [Route("{id}/details")]
        [HttpGet]
        public IEnumerable<SkuDetailViewModel> GetSkuDetails(int id)
        {
            IEnumerable<SkuDetailViewModel> details = from det in db.KDSCMSSKUD
                                                      from parpost in db.KDSCMSPARDTABLE
                                                      where parpost.PARDTABID == 17 && parpost.PARDVAC3 == det.SKUDNM && det.SKUDSKUID == id
                                                      select new SkuDetailViewModel
                                                      {
                                                          skudetid = det.SKUDSKUIDD,
                                                          skudetidx = det.SKUDSKUIDDX,
                                                          skudetname = parpost.PARDSDESC,
                                                          skudetlvl = det.SKUDLVL,
                                                          skudetval = det.SKUDVAL,
                                                          skudetvaltype = det.SKUDTYPE,
                                                          skudetpart = det.SKUDPART,
                                                          skudetbasedon=det.SKUDBSON
                                                      }
            ;
            return details;
        }

        [Route("{id}/details/{detid}")]
        [HttpGet]
        [ResponseType(typeof(SkuDetailViewModel))]
        public IHttpActionResult GetDetail(int id, int detid)
        {
            KDSCMSSKUD KDSCMSSKUD = db.KDSCMSSKUD.FirstOrDefault(d => d.SKUDSKUID == id && d.SKUDSKUIDD == detid);
            if (KDSCMSSKUD == null)
            {
                return NotFound();
            }
            SkuDetailViewModel detail = new SkuDetailViewModel();
            detail.skudetid = KDSCMSSKUD.SKUDSKUIDD;
            detail.skudetidx = KDSCMSSKUD.SKUDSKUIDDX;
            detail.skudetname = db.KDSCMSPARDTABLE.FirstOrDefault(d => d.PARDTABID == 17 && d.PARDVAC3 == KDSCMSSKUD.SKUDNM).PARDSDESC;
            detail.skudetlvl = KDSCMSSKUD.SKUDLVL;
            detail.skudetval = KDSCMSSKUD.SKUDVAL;
            detail.skudetvaltype = KDSCMSSKUD.SKUDTYPE;
            detail.skudetpart = KDSCMSSKUD.SKUDPART;
            detail.skudetbasedon = KDSCMSSKUD.SKUDBSON;
            return Ok(detail);
        }
    }
}
