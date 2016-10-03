﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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

namespace cms.api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private CMSContext db = new CMSContext();

        [Route("")]
        [HttpGet]
        public IEnumerable<UserViewModel> GetUsers()
        {
            IEnumerable<UserViewModel> users = from user in db.KDSCMSUSER select new UserViewModel {
                userid = user.USERUSID,
                username = user.USERUSNM,
                userdesc =user.USERUSDSC,
                usertype =user.USERTYPE,
                userstatus =user.USERSTAT,
                useraccprofile =user.USERACPROF,
                usermenuprofile =user.USERMEPROF,
                usersiteprofile =user.USERSTPROF,
                userstartdate =user.USERSDAT,
                userenddate =user.USEREDAT
            }
            ;
            return users;
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult GetUsersData([FromBody]JObject data)
        {
            string userid = data["userid"].ToString();
            DateTime enddate = DateTime.Today.AddDays(-7);

            if (!string.IsNullOrWhiteSpace(userid))
            {
                KDSCMSUSER KDSCMSUSER = db.KDSCMSUSER.Find(userid);
                if (KDSCMSUSER != null)
                {
                    JSiteProfile siteprof = new JSiteProfile();
                    List<JProfSiteLink> jprofsitelinks = new List<JProfSiteLink>();
                    KDSCMSSITEPROF KDSCMSSITEPROF = KDSCMSUSER.KDSCMSSITEPROF;
                    siteprof.siteprofid = KDSCMSSITEPROF.STPRSTPROF;
                    siteprof.siteprofdesc = KDSCMSSITEPROF.STPRSTDESC;

                    IEnumerable<KDSCMSPROFSITELINK> profsitelinks = db.KDSCMSPROFSITELINK.Where(d => 
                                d.PRSTSTPROF == KDSCMSSITEPROF.STPRSTPROF &&
                                DbFunctions.TruncateTime(d.PRSTSDAT) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                DbFunctions.TruncateTime(d.PRSTEDAT) >= DbFunctions.TruncateTime(DateTime.Today)
                    );
                    foreach(KDSCMSPROFSITELINK profsitelink in profsitelinks)
                    {
                        KDSCMSSITE KDSCMSSITE =  db.KDSCMSSITE.FirstOrDefault(d => d.SITESITE == profsitelink.PRSTSITE && d.SITESITEFLAG == 1 && d.SITESITESTATUS == 1);
                        if(KDSCMSSITE != null)
                        {
                            JProfSiteLink jprofsitelink = new JProfSiteLink();
                            JSite jsite = new JSite();
                            jsite.siteid = KDSCMSSITE.SITESITE;
                            jsite.sitename = KDSCMSSITE.SITESITENAME;
                            jsite.siteclass = KDSCMSSITE.SITESCLAS;
                            jsite.siteflag = Convert.ToInt16(KDSCMSSITE.SITESITEFLAG);
                            jsite.sitestatus = Convert.ToInt16(KDSCMSSITE.SITESITESTATUS);

                            jprofsitelink.siteid = profsitelink.PRSTSITE;
                            jprofsitelink.siteprofid = profsitelink.PRSTSTPROF;
                            jprofsitelink.linksdate = profsitelink.PRSTSDAT;
                            jprofsitelink.linkedate = profsitelink.PRSTEDAT;
                            jprofsitelink.Site = jsite;

                            jprofsitelinks.Add(jprofsitelink);

                            IEnumerable<KDSCMSSKULINK> skulinks = db.KDSCMSSKULINK.Where(d =>
                                d.SKULINKSITEID == KDSCMSSITE.SITESITE &&
                                DbFunctions.TruncateTime(d.SKULINKSDATE) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                DbFunctions.TruncateTime(d.SKULINKEDATE) >= DbFunctions.TruncateTime(enddate)
                            );

                            
                            List<JSkuLink> jskulinks = new List<JSkuLink>();

                            foreach (KDSCMSSKULINK skulink in skulinks)
                            {
                                KDSCMSSKUH skuh = db.KDSCMSSKUH.FirstOrDefault(d =>
                                    d.SKUHSKUID == skulink.SKULINKSKUID &&
                                    DbFunctions.TruncateTime(d.SKUHSDAT) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                    DbFunctions.TruncateTime(d.SKUHEDAT) >= DbFunctions.TruncateTime(skulink.SKULINKEDATE)
                                );

                                KDSCMSMSTBRND brand = skulink.KDSCMSMSTBRND;

                                if(skuh != null)
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

        [Route("{id}")]
        [HttpGet]
        [ResponseType(typeof(UserViewModel))]
        public IHttpActionResult GetUsers(string id)
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