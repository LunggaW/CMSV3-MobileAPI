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

namespace cms.api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private CMSContext db = new CMSContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();


        [Route("")]
        [HttpGet]
        public IEnumerable<UserViewModel> GetUsers()
        {
            try
            {
                IEnumerable<UserViewModel> users = from user in db.KDSCMSUSER
                                                   select new UserViewModel
                                                   {
                                                       userid = user.USERUSID,
                                                       username = user.USERUSNM,
                                                       userdesc = user.USERUSDSC,
                                                       usertype = user.USERTYPE,
                                                       userstatus = user.USERSTAT,
                                                       useraccprofile = user.USERACPROF,
                                                       usermenuprofile = user.USERMEPROF,
                                                       usersiteprofile = user.USERSTPROF,
                                                       userstartdate = user.USERSDAT,
                                                       userenddate = user.USEREDAT
                                                   }
          ;
                return users;
            }
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }
          
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult GetUsersData([FromBody]JObject data)
        {
            try
            {
                string userid = data["userid"].ToString();
                //string company = data["company"].ToString();

                DateTime enddate = DateTime.Today.AddDays(-7);

                if (!string.IsNullOrWhiteSpace(userid))
                {
                    KDSCMSUSER KDSCMSUSER = db.KDSCMSUSER.Find(userid);
                    if (KDSCMSUSER != null)
                    {
                        JSiteProfile siteprof = new JSiteProfile();
                        List<JProfSiteLink> jprofsitelinks = new List<JProfSiteLink>();

                        //KDSCMSSITEPROF KDSCMSSITEPROF = KDSCMSUSER.KDSCMSSITEPROF;
                        //siteprof.siteprofid = KDSCMSSITEPROF.STPRSTPROF;
                        //siteprof.siteprofdesc = KDSCMSSITEPROF.STPRSTDESC;

                        IEnumerable<KDSCMSSITELINK> sitelinks = db.KDSCMSSITELINK.Where(d =>
                                    d.STLNUSER == KDSCMSUSER.USERUSID &&
                                    DbFunctions.TruncateTime(d.STLNSDAT) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                    DbFunctions.TruncateTime(d.STLNEDAT) >= DbFunctions.TruncateTime(DateTime.Today) &&
                                    d.STLNCOMP == KDSCMSUSER.USERCOMP
                        );

                        foreach (KDSCMSSITELINK sitelink in sitelinks)
                        {
                            logger.Debug("Site Link Site : " + sitelink.STLNSITE);
                            KDSCMSSITE KDSCMSSITE = db.KDSCMSSITE.FirstOrDefault(d => d.SITESITE == sitelink.STLNSITE && d.SITESITEFLAG == 1 && d.SITESITESTATUS == 1 && d.SITESITECOMP == KDSCMSUSER.USERCOMP);
                            if (KDSCMSSITE != null)
                            {
                               logger.Debug("Site : "+ KDSCMSSITE.SITESITE);
                                logger.Debug("Site Link Site : " + sitelink.STLNSITE);

                                JProfSiteLink jsitelink = new JProfSiteLink();
                                JSite jsite = new JSite();
                                jsite.siteid = KDSCMSSITE.SITESITE;
                                jsite.sitename = KDSCMSSITE.SITESITENAME;
                                jsite.siteclass = KDSCMSSITE.SITESCLAS;
                                jsite.siteflag = Convert.ToInt16(KDSCMSSITE.SITESITEFLAG);
                                jsite.sitestatus = Convert.ToInt16(KDSCMSSITE.SITESITESTATUS);

                                jsitelink.siteid = sitelink.STLNSITE;
                                jsitelink.siteprofid = sitelink.STLNCOMP;
                                jsitelink.linksdate = sitelink.STLNSDAT;
                                jsitelink.linkedate = sitelink.STLNEDAT;
                                jsitelink.Site = jsite;

                                jprofsitelinks.Add(jsitelink);

                                IEnumerable<KDSCMSSKULINK> skulinks = db.KDSCMSSKULINK.Where(d =>
                                    d.SKULINKSITEID == KDSCMSSITE.SITESITE &&
                                    DbFunctions.TruncateTime(d.SKULINKSDATE) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                    DbFunctions.TruncateTime(d.SKULINKEDATE) >= DbFunctions.TruncateTime(enddate) &&
                                    d.SKULINKCOMP == KDSCMSUSER.USERCOMP
                                );


                                List<JSkuLink> jskulinks = new List<JSkuLink>();

                                foreach (KDSCMSSKULINK skulink in skulinks)
                                {
                                    KDSCMSSKUH skuh = db.KDSCMSSKUH.FirstOrDefault(d =>
                                        d.SKUHSKUID == skulink.SKULINKSKUID &&
                                        DbFunctions.TruncateTime(d.SKUHSDAT) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                        DbFunctions.TruncateTime(d.SKUHEDAT) >= DbFunctions.TruncateTime(skulink.SKULINKEDATE) &&
                                        d.SKUHCOMP == KDSCMSUSER.USERCOMP
                                    );

                                    KDSCMSMSTBRND brand = skulink.KDSCMSMSTBRND;

                                    if (skuh != null)
                                    {
                                        JSkuHeader jskuheader = new JSkuHeader();
                                        jskuheader.skuid = skuh.SKUHSKUID;
                                        jskuheader.skuidx = skuh.SKUHSKUIDX;
                                        jskuheader.skusdesc = skuh.SKUHSDES;
                                        jskuheader.skuldesc = skuh.SKUHLDES;
                                        jskuheader.skusdate = skuh.SKUHSDAT;
                                        jskuheader.skuedate = skuh.SKUHEDAT;

                                        JBrand jbrand = new JBrand();
                                        jbrand.brandid = brand.BRNDBRNDID;
                                        jbrand.branddesc = brand.BRNDDESC;

                                        JSkuLink jskulink = new JSkuLink();
                                        jskulink.siteid = skulink.SKULINKSITEID;
                                        jskulink.skuid = skulink.SKULINKSKUID;
                                        jskulink.brandid = skulink.SKULINKBRNDID;
                                        jskulink.linksdate = skulink.SKULINKSDATE;
                                        jskulink.linkedate = skulink.SKULINKEDATE;
                                        jskulink.SkuHeader = jskuheader;
                                        jskulink.JBrand = jbrand;

                                        /*
                                        IEnumerable<KDSCMSSKUD> skuds = skuh.KDSCMSSKUD;
                                        List<JSkuDetail> jskudetails = new List<JSkuDetail>();

                                        foreach(KDSCMSSKUD skud in skuds)
                                        {
                                            string skudetname = db.KDSCMSPARDTABLE.FirstOrDefault(d => d.PARDTABID == 17 && d.PARDVAC3 == skud.SKUDNM).PARDSDESC;
                                            JSkuDetail skudetail = new JSkuDetail();
                                            skudetail.skuid = skud.SKUDSKUID;
                                            skudetail.skudetid = skud.SKUDSKUIDD;
                                            skudetail.skudetidx = skud.SKUDSKUIDDX;
                                            skudetail.skudetname = skudetname;
                                            skudetail.skudetlvl = skud.SKUDLVL;
                                            skudetail.skudetval = skud.SKUDVAL;
                                            skudetail.skudetpart = skud.SKUDPART;
                                            skudetail.skudetbson = skud.SKUDBSON;
                                            skudetail.skudettype = skud.SKUDTYPE;

                                            jskudetails.Add(skudetail);
                                        }

                                        jskuheader.JSkuDetail = jskudetails;
                                        */
                                        jskulinks.Add(jskulink);
                                    }
                                }
                                jsite.SkuLink = jskulinks;
                            }
                        }

                        siteprof.ProfileSiteLinks = jprofsitelinks;
                        JUser juser = new JUser();
                        juser.userid = KDSCMSUSER.USERUSID;
                        juser.username = KDSCMSUSER.USERUSNM;
                        juser.userdesc = KDSCMSUSER.USERUSDSC;
                        juser.usertype = KDSCMSUSER.USERTYPE;
                        juser.userstatus = KDSCMSUSER.USERSTAT;
                        juser.useraccprofile = KDSCMSUSER.USERACPROF;
                        juser.usermenuprofile = KDSCMSUSER.USERMEPROF;
                        juser.siteprofid = KDSCMSUSER.USERSTPROF;
                        juser.userstartdate = KDSCMSUSER.USERSDAT;
                        juser.userenddate = KDSCMSUSER.USEREDAT;
                        juser.SiteProfile = siteprof;

                        //add IMEI
                        juser.IMEI = KDSCMSUSER.USERIMEI;

                        return Ok(juser);
                    }
                    else
                    {
                        return NotFound();
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

        [Route("getimei")]
        [HttpPost]
        public IHttpActionResult GetImei([FromBody]JObject data)
        {
            try
            {
                string userid = data["userid"].ToString();

                if (!string.IsNullOrWhiteSpace(userid))
                {
                    KDSCMSUSER KDSCMSUSER = db.KDSCMSUSER.Find(userid);
                    if (KDSCMSUSER != null)
                    {
                        //add IMEI
                        string Imei = KDSCMSUSER.USERIMEI;

                        return Ok(Imei);
                    }
                    else
                    {
                        return NotFound();
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

        [Route("getuser")]
        [HttpPost]
        public IHttpActionResult GetUser([FromBody]JObject data)
        {
            try
            {
                string userid = data["userid"].ToString();

                if (!string.IsNullOrWhiteSpace(userid))
                {
                    KDSCMSUSER KDSCMSUSER = db.KDSCMSUSER.Find(userid);
                    if (KDSCMSUSER != null)
                    {
                        //add IMEI
                        JUser juser = new JUser();
                        juser.userid = KDSCMSUSER.USERUSID;
                        juser.username = KDSCMSUSER.USERUSNM;
                        juser.userdesc = KDSCMSUSER.USERUSDSC;
                        juser.usertype = KDSCMSUSER.USERTYPE;
                        juser.userstatus = KDSCMSUSER.USERSTAT;
                        juser.useraccprofile = KDSCMSUSER.USERACPROF;
                        juser.usermenuprofile = KDSCMSUSER.USERMEPROF;
                        juser.siteprofid = KDSCMSUSER.USERSTPROF;
                        juser.userstartdate = KDSCMSUSER.USERSDAT;
                        juser.userenddate = KDSCMSUSER.USEREDAT;


                        return Ok(juser);
                    }
                    else
                    {
                        return NotFound();
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

        [Route("changepassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword([FromBody]JObject data)
        {
            try
            {
                string userid = data["userid"].ToString();
                logger.Debug("User ID = "+ userid);
                string password = data["password"].ToString();
                logger.Debug("Password  = " + password);

                if (!string.IsNullOrWhiteSpace(userid))
                {
                    KDSCMSUSER KDSCMSUSER = db.KDSCMSUSER.Find(userid);
                    if (KDSCMSUSER != null)
                    {
                        
                        KDSCMSUSER.USERPASW = password;
                        db.SaveChanges();

                        return Ok("ok");
                    }
                    else
                    {
                        return NotFound();
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
