using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using cms.api.JsonModels;
using cms.api.Models;
using cms.api.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NLog;

namespace cms.api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Sales")]
    public class SalesController : ApiController
    {
        private CMSContext db = new CMSContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Route("")]
        [HttpPost]
        public IHttpActionResult PostSales([FromBody]JObject data)
        {
            string userid = data["userid"].ToString();
            string salesstr = data["salesdata"].ToString();
            if(!string.IsNullOrWhiteSpace(userid) && !string.IsNullOrWhiteSpace(salesstr))
            {
                KDSCMSUSER user = db.KDSCMSUSER.Find(userid);
                if(user != null)
                {
                    try
                    {
                        List<JSales> jsales = JsonConvert.DeserializeObject<List<JSales>>(salesstr);
                        int countjsales = jsales.Count();
                        int counter = 0;

                        if (countjsales > 0)
                        {
                            foreach (JSales saldata in jsales)
                            {
                                List<long> seq = db.Database.SqlQuery<long>("SELECT KDSCMSSALES_INTSEQ.NEXTVAL VAL FROM DUAL").ToList();
                                long sequence = seq[0];

                                KDSCMSSALES_INT intsales = new KDSCMSSALES_INT();


                                //update GAGAN
                                intsales.CMSSALNOTA = "MBL";


                                //intsales.CMSSALNOTA = saldata.transnota;
                                intsales.CMSSALBRCD = saldata.transbrcd;
                                intsales.CMSSALQTY = saldata.transqty;
                                intsales.CMSSALSKU = saldata.transsku;
                                intsales.CMSSALFLAG = saldata.transflag;
                                intsales.CMSSALSTAT = saldata.transstat;
                                intsales.CMSSALCDAT = saldata.transdcre;
                                intsales.CMSSALMDAT = saldata.transdcre;
                                intsales.CMSSALSITE = saldata.transsite;
                                intsales.CMSSALCRBY = saldata.transcreby;
                                intsales.CMSSALAMT = saldata.transamt;
                                intsales.CMSSALSEQ = sequence;
                                intsales.CMSSALTYPE = saldata.transtype;
                                intsales.CMSSALTRNDATE = saldata.transdate;

                                //Update GAGAN
                                intsales.CMSDISCOUNT = saldata.transdiscount;
                                intsales.CMSFINALPRICE = saldata.transfinalprice;
                                intsales.CMSNORMALPRICE = saldata.transprice;


                                db.KDSCMSSALES_INT.Add(intsales);

                                counter++;
                            }

                            if (counter == countjsales)
                            {
                                db.SaveChanges();
                                db.Database.ExecuteSqlCommand("BEGIN PKKDSCMSPROSALES.MOVESALESDATA(); END;");
                                db.SaveChanges();
                                Dictionary<string, int> total = new Dictionary<string, int>();
                                total.Add("total", counter);
                                return Ok(total);
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            logger.Error("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                logger.Error("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error Message" + ex.Message);
                        logger.Error("Inner Exception" + ex.InnerException);
                        return BadRequest();
                    }


                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
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

    }
}
