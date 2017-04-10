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
using System.Web.UI;
using cms.api.JsonModels;
using cms.api.Models;
using cms.api.ViewModels;
using Newtonsoft.Json.Linq;
using NLog;

namespace cms.api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Sku")]
    public class SkuController : ApiController
    {
        private CMSContext db = new CMSContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Route("")]
        [HttpGet]
        public IEnumerable<SkuHeaderViewModel> GetSkus()
        {
            try
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
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }

        }

        [Route("site")]
        [HttpPost]
        public IEnumerable<SKULinkViewModel> PostSiteSkus([FromBody]JObject data)
        {
            try
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
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }

        }

        [Route("{id}")]
        [HttpGet]
        [ResponseType(typeof(SkuHeaderViewModel))]
        public IHttpActionResult GetSites(int id)
        {
            try
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
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }

        }

        [Route("{id}/details")]
        [HttpGet]
        public IEnumerable<SkuDetailViewModel> GetSkuDetails(int id)
        {
            try
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
                                                              skudetbasedon = det.SKUDBSON
                                                          }
           ;
                return details;
            }
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }

        }

        [Route("{id}/details/{detid}")]
        [HttpGet]
        [ResponseType(typeof(SkuDetailViewModel))]
        public IHttpActionResult GetDetail(int id, int detid)
        {
            try
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
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }

        }


        [Route("getskulists")]
        [HttpPost]
        public IHttpActionResult GetSkuLists([FromBody]JObject data)
        {
            logger.Debug("GetSkuLists function in SkuController");
            try
            {
                string barcode = data["barcode"].ToString();
                string site = data["site"].ToString();
                string userid = data["userid"].ToString();

                logger.Debug("barcode : " + barcode);
                logger.Debug("site : " + site);

                if (!string.IsNullOrWhiteSpace(barcode) && !string.IsNullOrWhiteSpace(site))
                {
                    JSkuList list = new JSkuList();
                    List<JSkuList> jskulists = new List<JSkuList>();

                    KDSCMSUSER user = db.KDSCMSUSER.Find(userid);

                    logger.Debug("Company : "+user.USERCOMP);

                    using (var ctx = new CMSContext())
                    {
                        var lists =
                            ctx.KDSCMSSKUH.SqlQuery("select * " +
                                                    "from KDSCMSSKUH  " +
                                                    "where SKUHEDAT >= CURRENT_DATE " +
                                                    "AND SKUHCOMP = '"+user.USERCOMP+"' " +
                                                    "AND SKUHSKUID IN " +
                                                    "(select distinct SKULINKSKUID " +
                                                    "from KDSCMSSKULINK, KDSCMSMSTITEM " +
                                                    "WHERE SKULINKSITEID = '"+site+"' " +
                                                    "and SKULINKBRNDID = ITEMBRNDID " +
                                                    "and KDSCMSMSTITEM.ITEMITEMID = " +
                                                    "  (SELECT BRCDITEMID FROM KDSCMSMSTBRCD, KDSCMSSASS " +
                                                    "    WHERE  SASSITEMID = BRCDITEMID " +
                                                    "     AND SASSVRNT = BRCDVRNTID " +
                                                    "     AND BRCDBRCDID = '"+barcode+"' " +
                                                    "     AND SASSSITEID = KDSCMSSKULINK.SKULINKSITEID " +
                                                    "     AND SASSCOMP = BRCDCOMP " +
                                                    "     AND SASSCOMP = '"+ user.USERCOMP +"' ) " +
                                                    "AND SKULINKCOMP = ITEMCOMP " +
                                                    "AND SKULINKCOMP = '"+user.USERCOMP+"' ) ").ToList<KDSCMSSKUH>();
                        logger.Debug("masuk");
                        


                        foreach (KDSCMSSKUH skuLists in lists)
                        {
                            logger.Debug("Sku ID : " + skuLists.SKUHSKUID);
                            logger.Debug("Sku Desc : " + skuLists.SKUHSDES);
                            list = new JSkuList { skuid = skuLists.SKUHSKUID, skuDesc = skuLists.SKUHSDES };

                            jskulists.Add(list);
                        }

                        JSkuLists skuLists2 = new JSkuLists();

                        skuLists2.SkuList = jskulists;

                        return Ok(skuLists2);

                    }

                    

                    //IEnumerable<KDSCMSSKUH> tests = db.KDSCMSSKUH.Where(skuHeader => (from skuLink in db.KDSCMSSKULINK
                    //                                                                  join itemLists in db.KDSCMSMSTITEM
                    //                                                                      on skuLink.SKULINKBRNDID equals itemLists.ITEMBRNDID
                    //                                                                  where skuLink.SKULINKSITEID == site
                    //                                                                        &&
                    //                                                                        (from brcd in db.KDSCMSMSTBRCD
                    //                                                                         join assortment in db.KDSCMSSASS
                    //               on brcd.BRCDITEMID equals assortment.SASSITEMID
                    //                                                                         where assortment.SASSVRNT == brcd.BRCDVRNTID.ToString()
                    //                 && brcd.BRCDBRCDID == barcode
                    //                 && assortment.SASSSITEID == skuLink.SKULINKSITEID
                    //                                                                         select brcd.BRCDITEMID)
                    //                                                                            .Contains(itemLists.ITEMITEMID)
                    //                                                                  select skuLink.SKULINKSKUID)
                    //    .Contains(skuHeader.SKUHSKUID));

                   
                }
                else
                {
                    return BadRequest();
                }
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
