using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using cms.api.JsonModels;
using cms.api.Models;
using cms.api.ViewModels;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Targets;

namespace cms.api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Price")]
    public class PriceController : ApiController
    {
        private CMSContext db = new CMSContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Route("getprice")]
        [HttpPost]
        public IHttpActionResult GetPrice([FromBody]JObject data)
        {
            logger.Debug("GetPrice function in PriceController");
            try
            {
                string barcode = data["barcode"].ToString();
                string site = data["site"].ToString();
                string userid = data["userid"].ToString();

                logger.Debug("barcode : " + barcode);
                logger.Debug("site : " + site);

                if (!string.IsNullOrWhiteSpace(barcode) && !string.IsNullOrWhiteSpace(site))
                {
                    KDSCMSUSER user = db.KDSCMSUSER.Find(userid);
                    using (var ctx = new CMSContext())
                    {
                        var Price =
                            ctx.Database.SqlQuery<Decimal>("select SPRCPRICE " +
                                                      "from KDSCMSSPRICE, KDSCMSMSTBRCD, KDSCMSSASS " +
                                                      "WHERE " +
                                                      "SASSITEMID = BRCDITEMID " +
                                                      " AND SASSVRNT = BRCDVRNTID " +
                                                      " AND BRCDBRCDID = '" + barcode + "' " +
                                                      " AND SASSSITEID = '"+site+"' " +
                                                      " AND SPRCITEMID = BRCDITEMID " +
                                                      " AND SPRCVRNTID = BRCDVRNTID " +
                                                      " AND SPRCSITE = SASSSITEID " +
                                                      " AND SPRCCOMP = '"+ user .USERCOMP+ "' ").ToList();

                        if (Price != null)
                        {
                            //add IMEI

                            foreach (var q in Price)
                            {
                                logger.Debug("Price : " + q);
                                return Ok(q.ToString());
                            }

                            
                        }
                        else
                        {
                            return NotFound();
                        }
                        return null;

                    }

                    //IEnumerable<string> Price = from price in db.KDSCMSSPRICE
                    //            join brcd in db.KDSCMSMSTBRCD
                    //                on price.SPRCITEMID equals brcd.BRCDITEMID
                    //            join assortment in db.KDSCMSSASS
                    //                on price.SPRCSITE equals assortment.SASSSITEID
                    //            where assortment.SASSITEMID == brcd.BRCDITEMID
                    //               && assortment.SASSVRNT == brcd.BRCDVRNTID.ToString()
                    //               && brcd.BRCDBRCDID == barcode
                    //               && assortment.SASSSITEID == site
                    //               && price.SPRCITEMID == brcd.BRCDITEMID
                    //               && price.SPRCVRNTID == brcd.BRCDVRNTID
                    //            select price.SPRCPRICE.ToString();

                    //if (Price != null)
                    //{
                    //    //add IMEI

                    //    foreach (var q in Price)
                    //    {
                    //        logger.Debug("Price : " + q);
                    //    }

                    //    return Ok(Price);
                    //}
                    //else
                    //{
                    //    return NotFound();
                    //}
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


        [Route("{id}")]
        [HttpGet]
        [ResponseType(typeof(UserViewModel))]
        public IHttpActionResult GetUsers(string id)
        {
            try
            {
                KDSCMSUSER KDSCMSUSER = db.KDSCMSUSER.Find(id);
                if (KDSCMSUSER == null)
                {
                    return NotFound();
                }
                UserViewModel user = new UserViewModel();
                user.userid = KDSCMSUSER.USERUSID;
                user.username = KDSCMSUSER.USERUSNM;
                user.userdesc = KDSCMSUSER.USERUSDSC;
                user.usertype = KDSCMSUSER.USERTYPE;
                user.userstatus = KDSCMSUSER.USERSTAT;
                user.useraccprofile = KDSCMSUSER.USERACPROF;
                user.usermenuprofile = KDSCMSUSER.USERMEPROF;
                user.usersiteprofile = KDSCMSUSER.USERSTPROF;
                user.userstartdate = KDSCMSUSER.USERSDAT;
                user.userenddate = KDSCMSUSER.USEREDAT;
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool UserExists(string id)
        {
            return db.KDSCMSUSER.Count(e => e.USERUSID == id) > 0;
        }
    }
}
