using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
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
        public IHttpActionResult PostSimpleSales([FromBody]JObject data)
        {
            string userid = data["userid"].ToString();
            string salesstr = data["salesdata"].ToString();
            if (!string.IsNullOrWhiteSpace(userid) && !string.IsNullOrWhiteSpace(salesstr))
            {
                KDSCMSUSER user = db.KDSCMSUSER.Find(userid);
                if (user != null)
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

        [Route("Complex")]
        [HttpPost]
        public IHttpActionResult PostComplexSales([FromBody]JObject data)
        {
            string userid = data["userid"].ToString();
            string salesstr = data["salesdata"].ToString();
            long sequence = 0;
            if (!string.IsNullOrWhiteSpace(userid) && !string.IsNullOrWhiteSpace(salesstr))
            {
                KDSCMSUSER user = db.KDSCMSUSER.Find(userid);
                if (user != null)
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
                                List<long> seq = db.Database.SqlQuery<long>("SELECT KDSCMSSLSCOM_INT_SEQ.NEXTVAL VAL FROM DUAL").ToList();
                                sequence = seq[0];

                                KDSCMSSALES_INT intsales = new KDSCMSSALES_INT();

                                KDSCMSSLSCOM_INT intComplexSales = new KDSCMSSLSCOM_INT();

                                intComplexSales.CMSSALNOTA = saldata.transnota;
                                intComplexSales.CMSSALBRCD = saldata.transbrcd;
                                intComplexSales.CMSSALQTY = saldata.transqty;
                                intComplexSales.CMSSALSKU = saldata.transsku;
                                intComplexSales.CMSSALFLAG = saldata.transflag;
                                intComplexSales.CMSSALSTAT = saldata.transstat;
                                intComplexSales.CMSSALCDAT = saldata.transdcre;
                                intComplexSales.CMSSALSITE = saldata.transsite;
                                intComplexSales.CMSSALCRBY = saldata.transcreby;
                                intComplexSales.CMSSALPRC = saldata.transamt;
                                intComplexSales.CMSSALSEQ = sequence;
                                intComplexSales.CMSSALTRNDATE = saldata.transdate;
                                intComplexSales.CMSSALCOMM = saldata.transcomm;

                                logger.Trace("Nota : " + saldata.transnota);
                                logger.Trace("Barcode : " + saldata.transbrcd);
                                logger.Trace("Qty : " + saldata.transqty);
                                logger.Trace("SKU : " + saldata.transsku);
                                logger.Trace("Flag : " + saldata.transflag);
                                logger.Trace("Stat : " + saldata.transstat);
                                logger.Trace("Created Date : " + saldata.transdcre);
                                logger.Trace("Site : " + saldata.transsite);
                                logger.Trace("Created By : " + saldata.transcreby);
                                logger.Trace("Price: " + saldata.transamt);
                                logger.Trace("Sequence : " + sequence);
                                logger.Trace("Transaction Date : " + saldata.transdate);
                                logger.Trace("Comment : " + saldata.transcomm);

                                db.KDSCMSSLSCOM_INT.Add(intComplexSales);

                                counter++;
                            }

                            if (counter == countjsales)
                            {
                                db.SaveChanges();
                                //db.Database.ExecuteSqlCommand("BEGIN PKKDSCMSPROSALES.MOVESALESDATA(); END;");
                                db.Database.ExecuteSqlCommand("BEGIN PKKDSCMSSLSD.MOBILE_PROCESS_COMPLEX(" + sequence + "); END;");

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

        [Route("checknotabarcode")]
        [HttpPost]
        public IHttpActionResult CheckNotaBarcode([FromBody]JObject data)
        {
            logger.Debug("GetNota function in SalesController");
            try
            {
                string salesHeader = data["salesheader"].ToString();


                JSalesHeaderPrimary jsales = JsonConvert.DeserializeObject<JSalesHeaderPrimary>(salesHeader);

                logger.Debug("nota : " + jsales.nota);
                logger.Debug("barcode : " + jsales.barcode);
                logger.Debug("date : " + jsales.date);
                logger.Debug("site : " + jsales.site);
                logger.Debug("user : " + jsales.user);

                if (!string.IsNullOrWhiteSpace(jsales.nota))
                {
                    using (var ctx = new CMSContext())
                    {


                        IEnumerable<KDSCMSSLSH> SalesHeader = from salesH in db.KDSCMSSLSH
                                                              where salesH.SLSHSITE == jsales.site
                                                              && salesH.SLSHSLNOTA == jsales.nota
                                                              && salesH.SLSHSLDATE == jsales.date
                                                              && salesH.SLSHCRBY == jsales.user
                                                              select salesH;

                        if (SalesHeader.Any())
                        {

                            logger.Debug("found Sales Header");
                            logger.Debug("Sales ID : " + SalesHeader.FirstOrDefault().SLSHSLID + "");
                            logger.Debug("Sales Internal ID : " + SalesHeader.FirstOrDefault().SLSHSLIDI + "");


                            IEnumerable<KDSCMSSLSD> SalesDetail = from salesD in db.KDSCMSSLSD
                                                                  where salesD.SLSDSITE == jsales.site
                                                                  && salesD.SLSDSLNOTA == jsales.nota
                                                                  && salesD.SLSDSLDATE == jsales.date
                                                                  && salesD.SLSDBRCD == jsales.barcode
                                                                  && salesD.SLSDSLID == SalesHeader.FirstOrDefault().SLSHSLID
                                                                  && salesD.SLSDSLIDI == SalesHeader.FirstOrDefault().SLSHSLIDI
                                                                  select salesD;
                            if (SalesDetail.Any())
                            {

                                logger.Debug("found Sales Detail");
                                logger.Debug("Sales ID : " + SalesDetail.FirstOrDefault().SLSDSLID + "");
                                logger.Debug("Sales Internal ID : " + SalesDetail.FirstOrDefault().SLSDSLIDI + "");
                                //var QtyLists = new JQtyLists();

                                //var list = SalesDetail.Select(slsDetLists => slsDetLists.SLSDSLQTY).Select(dummy => (int)dummy).ToList();

                                //QtyLists.QtyList = list;

                                return Ok();
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
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


        [Route("getsalesheader")]
        [HttpPost]
        public IHttpActionResult GetSalesHeader([FromBody]JObject data)
        {
            logger.Debug("GetSalesHeader function in SalesController");
            try
            {
                string salesHeader = data["salesheader"].ToString();


                JSalesHeaderPrimary jsales = JsonConvert.DeserializeObject<JSalesHeaderPrimary>(salesHeader);

                logger.Debug("nota : " + jsales.nota);
                //logger.Debug("barcode : " + jsales.barcode);
                logger.Debug("date : " + jsales.date);
                logger.Debug("site : " + jsales.site);
                logger.Debug("user : " + jsales.user);

                if (!string.IsNullOrWhiteSpace(jsales.nota))
                {
                    using (var ctx = new CMSContext())
                    {
                        logger.Debug("Nota Exist");



                        //var SalesHeaders =
                        //    ctx.Database.SqlQuery<JSalesHeaderView>(
                        //        "select SLSHSLNOTA, SLSHSITE, SLSHCRBY, SLSHSLDATE, SLSHFLAG, SLSHSTAT, SUM(KDSCMSSLSD.SLSDSLTOTCUS) AS SLSHTOTAMT " +
                        //        "from KDSCMSSLSH join KDSCMSSLSD " +
                        //        "on KDSCMSSLSH.SLSHSLID = KDSCMSSLSD.SLSDSLID " +
                        //        "where SLSHSLNOTA like '%" + jsales.nota + "%' " +
                        //        "and SLSHSLDATE = to_date('" + jsales.date.ToString("dd-MMM-yy") + "' , 'DD-Mon-YY') " +
                        //        "and SLSHCRBY like '%" + jsales.user + "%' " +
                        //        "and SLSHSITE like '%" + jsales.site + "%' " +
                        //        "AND SLSHSLIDI = SLSDSLIDI " +
                        //        "AND SLSHSLNOTA = KDSCMSSLSD.SLSDSLNOTA " +
                        //        "GROUP BY SLSHSLNOTA, SLSHSITE, SLSHCRBY, SLSHSLDATE, SLSHFLAG, SLSHSTAT")
                        //.ToList();



                        var SalesHeaders = from salesH in db.KDSCMSSLSH
                                           join salesD in db.KDSCMSSLSD
                                               on salesH.SLSHSLID equals salesD.SLSDSLID
                                           where salesH.SLSHSITE.Contains(jsales.site)
                                                 && salesH.SLSHSLNOTA.Contains(jsales.nota)
                                                 && salesH.SLSHSLDATE == jsales.date
                                                 && salesH.SLSHCRBY.Contains(jsales.user)
                                                 && salesH.SLSHSLIDI == salesD.SLSDSLIDI
                                                 && salesH.SLSHSLNOTA == salesD.SLSDSLNOTA
                                           group salesD by
                                               new
                                               {
                                                   salesH.SLSHSLNOTA,
                                                   salesH.SLSHCRBY,
                                                   salesH.SLSHSITE,
                                                   salesH.SLSHSLDATE,
                                                   salesH.SLSHSTAT,
                                                   salesH.SLSHFLAG
                                               }
                            into g
                                           select new
                                           {
                                               nota = g.Key.SLSHSLNOTA,
                                               site = g.Key.SLSHSITE,
                                               user = g.Key.SLSHCRBY,
                                               date = g.Key.SLSHSLDATE,
                                               status = g.Key.SLSHSTAT,
                                               type = g.Key.SLSHFLAG,
                                               totalAmount = g.Sum(salesD => salesD.SLSDSLTOTCUS)
                                           };

                        logger.Debug("Total data retreived : " + SalesHeaders.Count());

                        if (SalesHeaders.Any())
                        {

                            logger.Debug("found Sales Header");
                            
                            var SalesHeaderLists = new JSalesHeaderLists();
                            var SalesHeader = new JSalesHeader();
                            List<JSalesHeader> SalesHeaderList = new List<JSalesHeader>();
                            
                            foreach (var list in SalesHeaders)
                            {

                                SalesHeader = new JSalesHeader
                                {
                                    nota = list.nota,
                                    site = list.site,
                                    user = list.user,
                                    date = list.date,
                                    SalesStatus = (JSalesHeader.SalesStatusEnum)list.status,
                                    SalesType = (JSalesHeader.SalesTypeEnum)list.type,
                                    totalamount = list.totalAmount.GetValueOrDefault()
                                };

                                logger.Debug("Nota : " + list.date);
                                logger.Debug("Sales Status : " + SalesHeader.SalesStatus);
                                logger.Debug("Sales Type : " + SalesHeader.SalesType);

                                SalesHeaderList.Add(SalesHeader);
                            }

                            SalesHeaderLists.SalesHeaderLists = SalesHeaderList;


                            return Ok(SalesHeaderLists);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message : " + ex.Message);
                logger.Error("Inner Exception : " + ex.InnerException);
                throw;
            }
        }


        [Route("getsalesheaderbystatustype")]
        [HttpPost]
        public IHttpActionResult GetSalesHeaderByStatusType([FromBody]JObject data)
        {
            logger.Debug("GetSalesHeaderByStatusType function in SalesController");
            try
            {
                string salesHeader = data["salesheader"].ToString();


                JSalesHeaderPrimary jsales = JsonConvert.DeserializeObject<JSalesHeaderPrimary>(salesHeader);

                logger.Debug("nota : " + jsales.nota);
                //logger.Debug("barcode : " + jsales.barcode);
                logger.Debug("date : " + jsales.date);
                logger.Debug("site : " + jsales.site);
                logger.Debug("user : " + jsales.user);

                if (!string.IsNullOrWhiteSpace(jsales.nota))
                {
                    using (var ctx = new CMSContext())
                    {
                        logger.Debug("Nota Exist");



                        //var SalesHeaders =
                        //    ctx.Database.SqlQuery<JSalesHeaderView>(
                        //        "select SLSHSLNOTA, SLSHSITE, SLSHCRBY, SLSHSLDATE, SLSHFLAG, SLSHSTAT, SUM(KDSCMSSLSD.SLSDSLTOTCUS) AS SLSHTOTAMT " +
                        //        "from KDSCMSSLSH join KDSCMSSLSD " +
                        //        "on KDSCMSSLSH.SLSHSLID = KDSCMSSLSD.SLSDSLID " +
                        //        "where SLSHSLNOTA like '%" + jsales.nota + "%' " +
                        //        "and SLSHSLDATE = to_date('" + jsales.date.ToString("dd-MMM-yy") + "' , 'DD-Mon-YY') " +
                        //        "and SLSHCRBY like '%" + jsales.user + "%' " +
                        //        "and SLSHSITE like '%" + jsales.site + "%' " +
                        //        "AND SLSHSLIDI = SLSDSLIDI " +
                        //        "AND SLSHSLNOTA = KDSCMSSLSD.SLSDSLNOTA " +
                        //        "GROUP BY SLSHSLNOTA, SLSHSITE, SLSHCRBY, SLSHSLDATE, SLSHFLAG, SLSHSTAT")
                        //.ToList();



                        var SalesHeaders = from salesH in db.KDSCMSSLSH
                                           join salesD in db.KDSCMSSLSD
                                               on salesH.SLSHSLID equals salesD.SLSDSLID
                                           where salesH.SLSHSITE.Contains(jsales.site)
                                                 && salesH.SLSHSLNOTA.Contains(jsales.nota)
                                                 && salesH.SLSHSLDATE == jsales.date
                                                 && salesH.SLSHCRBY.Contains(jsales.user)
                                                 && salesH.SLSHSLIDI == salesD.SLSDSLIDI
                                                 && salesH.SLSHSLNOTA.Contains(salesD.SLSDSLNOTA) 
                                                 && salesH.SLSHSTAT == (int)jsales.SalesType
                                                 && salesH.SLSHFLAG == (int)jsales.SalesStatus
                                           group salesD by
                                               new
                                               {
                                                   salesH.SLSHSLNOTA,
                                                   salesH.SLSHCRBY,
                                                   salesH.SLSHSITE,
                                                   salesH.SLSHSLDATE,
                                                   salesH.SLSHSTAT,
                                                   salesH.SLSHFLAG
                                               }
                            into g
                                           select new
                                           {
                                               nota = g.Key.SLSHSLNOTA,
                                               site = g.Key.SLSHSITE,
                                               user = g.Key.SLSHCRBY,
                                               date = g.Key.SLSHSLDATE,
                                               status = g.Key.SLSHSTAT,
                                               type = g.Key.SLSHFLAG,
                                               totalAmount = g.Sum(salesD => salesD.SLSDSLTOTCUS)
                                           };

                        logger.Debug("Total data retreived : " + SalesHeaders.Count());

                        if (SalesHeaders.Any())
                        {

                            logger.Debug("found Sales Header");

                            var SalesHeaderLists = new JSalesHeaderLists();
                            var SalesHeader = new JSalesHeader();
                            List<JSalesHeader> SalesHeaderList = new List<JSalesHeader>();

                            foreach (var list in SalesHeaders)
                            {

                                SalesHeader = new JSalesHeader
                                {
                                    nota = list.nota,
                                    site = list.site,
                                    user = list.user,
                                    date = list.date,
                                    SalesStatus = (JSalesHeader.SalesStatusEnum)list.status,
                                    SalesType = (JSalesHeader.SalesTypeEnum)list.type,
                                    totalamount = list.totalAmount.GetValueOrDefault()
                                };

                                logger.Debug("Nota : " + list.date);
                                logger.Debug("Sales Status : " + SalesHeader.SalesStatus);
                                logger.Debug("Sales Type : " + SalesHeader.SalesType);

                                SalesHeaderList.Add(SalesHeader);
                            }

                            SalesHeaderLists.SalesHeaderLists = SalesHeaderList;


                            return Ok(SalesHeaderLists);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message : " + ex.Message);
                logger.Error("Inner Exception : " + ex.InnerException);
                throw;
            }
        }

        [Route("getsalesdet")]
        [HttpPost]
        public IHttpActionResult GetSalesDetail([FromBody]JObject data)
        {
            logger.Debug("GetSalesDetail function in SalesController");
            try
            {
                string salesDetail = data["salesdet"].ToString();


                JSalesHeaderPrimary jsales = JsonConvert.DeserializeObject<JSalesHeaderPrimary>(salesDetail);

                logger.Debug("nota : " + jsales.nota);
                //logger.Debug("barcode : " + jsales.barcode);
                logger.Debug("date : " + jsales.date);
                logger.Debug("site : " + jsales.site);
                logger.Debug("user : " + jsales.user);

                if (!string.IsNullOrWhiteSpace(jsales.nota))
                {
                    using (var ctx = new CMSContext())
                    {
                        logger.Debug("Nota Exist");

                        //var SalesDetails = from salesD in db.KDSCMSSLSD
                        //        join item in db.KDSCMSMSTITEM
                        //        on salesD.SLSDITEMID equals item.ITEMITEMID
                        //        join variant in db.KDSCMSMSTVRNT
                        //        on salesD.SLSDVRNTID equals variant.VRNTVRNTID
                        //        join sku in db.KDSCMSSKUH
                        //        on salesD.SLSDSKUID equals sku.SKUHSKUID.ToString()
                        //    where salesD.SLSDSITE.Contains(jsales.site)
                        //          && salesD.SLSDSLNOTA.Contains(jsales.nota)
                        //          && salesD.SLSDSLDATE == jsales.date
                        //          && salesD.SLSDCRBY.Contains(jsales.user)
                        //    select new
                        //    {
                        //        nota = salesD.SLSDSLNOTA,
                        //        site = salesD.SLSDSITE,
                        //        user = salesD.SLSDBRCD,
                        //        date = salesD.SLSDSLDATE,
                        //        barcode = salesD.SLSDBRCD,
                        //        qty = salesD.SLSDSLQTY,
                        //        totalamount = salesD.SLSDSLTOTCUS,
                        //        itemid = item.ITEMITEMIDX,
                        //        variant = variant.VRNTVRNTIDX,
                        //        description = variant.VRNTSDESC,
                        //        sku = sku.SKUHSKUIDX,
                        //        gross = salesD.SLSDTOTPRC,
                        //        type = salesD.SLSDFLAG,
                        //        status = salesD.SLSDSTAT
                        //    };

                        var SalesDetails =
                            ctx.Database.SqlQuery<JSalesDetail>(
                                "select SALESD.SLSDSLNOTA AS nota, SALESD.SLSDSITE as site, SALESD.SLSDCRBY AS userApps, " +
                                "SALESD.SLSDSLDATE AS salesdate, SALESD.SLSDBRCD as barcode, " +
                                "SALESD.SLSDSLQTY as qty, SALESD.SLSDSLTOTCUS AS totalamount, ITEM.ITEMITEMIDX AS itemid, " +
                                "VARIANT.VRNTVRNTIDX AS variant, VARIANT.VRNTSDESC as description, SKU.SKUHSDES as sku, " +
                                "SALESD.SLSDTOTPRC AS gross, SALESD.SLSDFLAG AS SalesType, SALESD.SLSDSTAT AS SalesStatus " +
                                "from KDSCMSSLSD SALESD " +
                                "INNER JOIN KDSCMSMSTITEM ITEM ON SALESD.SLSDITEMID = ITEM.ITEMITEMID " +
                                "INNER JOIN KDSCMSMSTVRNT VARIANT ON SALESD.SLSDVRNTID = VARIANT.VRNTVRNTID " +
                                "INNER JOIN KDSCMSSKUH SKU ON SALESD.SLSDSKUID = SKU.SKUHSKUID " +
                                "WHERE SALESD.SLSDSITE LIKE '%" + jsales.site + "%' " +
                                "AND SALESD.SLSDSLNOTA LIKE '%" + jsales.nota + "%' " +
                                "AND to_date(SALESD.SLSDSLDATE, 'DD-Mon-YY') = to_date('" + jsales.date.ToString("dd-MMM-yy") +
                                "', 'DD-Mon-YY') " +
                                "AND SALESD.SLSDCRBY LIKE '%" + jsales.user + "%'")
                                .ToList();


                        logger.Debug("Total data retreived : " + SalesDetails.Count());

                        if (SalesDetails.Any())
                        {
                            logger.Debug("found Sales Detail");

                            JSalesDetail SalesDetail = new JSalesDetail();
                            var SalesDetailLists = new JSalesDetList();
                            List<JSalesDetail> SalesDetailList = new List<JSalesDetail>();


                            foreach (var list in SalesDetails)
                            {
                                SalesDetail = new JSalesDetail
                                {
                                    nota = list.nota,
                                    site = list.site,
                                    userApps = list.userApps,
                                    salesdate = list.salesdate,
                                    barcode = list.barcode,
                                    qty = list.qty,
                                    totalamount = list.totalamount,
                                    itemid = list.itemid,
                                    variant = list.variant,
                                    description = list.description,
                                    sku = list.sku,
                                    gross = list.gross,
                                    SalesStatus = list.SalesStatus,
                                    SalesType = list.SalesType
                                };

                                SalesDetailList.Add(SalesDetail);
                            }

                            SalesDetailLists.SalesDetailLists = SalesDetailList;

                            return Ok(SalesDetailLists);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message : " + ex.Message);
                logger.Error("Inner Exception : " + ex.InnerException);
                throw;
            }
        }

        public IHttpActionResult GetComplexSales([FromBody]JObject data)
        {
            logger.Debug("GetNota function in SalesController");
            try
            {
                string salesHeader = data["salesheader"].ToString();


                JSalesHeaderPrimary jsales = JsonConvert.DeserializeObject<JSalesHeaderPrimary>(salesHeader);

                logger.Debug("nota : " + jsales.nota);
                logger.Debug("barcode : " + jsales.barcode);
                logger.Debug("date : " + jsales.date);
                logger.Debug("site : " + jsales.site);
                logger.Debug("user : " + jsales.user);

                if (!string.IsNullOrWhiteSpace(jsales.nota))
                {
                    using (var ctx = new CMSContext())
                    {


                        IEnumerable<KDSCMSSLSCOM_INT> Transaction = from sales in db.KDSCMSSLSCOM_INT
                                                                    where sales.CMSSALSITE.Contains(jsales.site)
                                                                          && sales.CMSSALNOTA.Contains(jsales.nota)
                                                                          && sales.CMSSALTRNDATE == jsales.date
                                                                          && sales.CMSSALCRBY.Contains(jsales.user)
                                                                    select sales;

                        if (Transaction.Any())
                        {

                            logger.Debug("found Sales Detail");


                            var TransLists = new JTransLists();
                            var JSales = new JSales();
                            List<JSales> TransactionList = new List<JSales>();




                            foreach (KDSCMSSLSCOM_INT Trans in Transaction)
                            {
                                JSales = new JSales { transcreby = Trans.CMSSALCRBY, transprice = Trans.CMSSALPRC.GetValueOrDefault(), transnota = Trans.CMSSALNOTA };

                                TransactionList.Add(JSales);
                            }

                            TransLists.TransactionLists = TransactionList;


                            return Ok();
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
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
