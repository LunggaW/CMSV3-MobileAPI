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

namespace cms.api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Sites")]
    public class SitesController : ApiController
    {
        private CMSContext db = new CMSContext();

        [Route("")]
        [HttpGet]
        public IEnumerable<SiteViewModel> GetSites()
        {
            IEnumerable<SiteViewModel> sites = from site in db.KDSCMSSITE
                                               select new SiteViewModel
                                               {
                                                    siteid = site.SITESITE,
                                                    siteclass = site.SITESCLAS,
                                                    sitename = site.SITESITENAME,
                                                    siteflag = site.SITESITEFLAG,
                                                    sitestatus = site.SITESITESTATUS
                                               }
            ;
            return sites;
        }

        [Route("profile/{id}")]
        [HttpGet]
        public IEnumerable<SiteViewModel> GetProfSites(string id)
        {
            IEnumerable<SiteViewModel> sites = from site in db.KDSCMSSITE
                                               join siteprof in db.KDSCMSPROFSITELINK on site.SITESITE equals siteprof.PRSTSITE
                                               where siteprof.PRSTSTPROF == id &&
                                               site.SITESITEFLAG == 1 &&
                                               site.SITESITESTATUS == 1 &&
                                               DbFunctions.TruncateTime(siteprof.PRSTSDAT) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                               DbFunctions.TruncateTime(siteprof.PRSTEDAT) >= DbFunctions.TruncateTime(DateTime.Today)
                                               select new SiteViewModel
                                                {
                                                   siteid = site.SITESITE,
                                                   siteclass = site.SITESCLAS,
                                                   sitename = site.SITESITENAME,
                                                   siteflag = site.SITESITEFLAG,
                                                   sitestatus = site.SITESITESTATUS
                                                }
            ;
            return sites;
        }

        [Route("user/{id}")]
        [HttpGet]
        public IEnumerable<SiteViewModel> GetUserSites(string id)
        {
            IEnumerable<SiteViewModel> sites = from site in db.KDSCMSSITE
                                                from siteprof in db.KDSCMSPROFSITELINK
                                                from user in db.KDSCMSUSER
                                                where user.USERUSID == id &&
                                                site.SITESITE == siteprof.PRSTSITE &&
                                                siteprof.PRSTSTPROF == user.USERSTPROF &&
                                                site.SITESITESTATUS == 1 &&
                                                site.SITESCLAS == 1 &&
                                                site.SITESITEFLAG == 1 &&
                                                DbFunctions.TruncateTime(siteprof.PRSTSDAT) <= DbFunctions.TruncateTime(DateTime.Today) &&
                                                DbFunctions.TruncateTime(siteprof.PRSTEDAT) >= DbFunctions.TruncateTime(DateTime.Today) &&
                                                user.USERSTAT == 1
                                                select new SiteViewModel
                                                {
                                                   siteid = site.SITESITE,
                                                   siteclass = site.SITESCLAS,
                                                   sitename = site.SITESITENAME,
                                                   siteflag = site.SITESITEFLAG,
                                                   sitestatus = site.SITESITESTATUS
                                                }
            ;
            return sites;
        }

        [Route("{id}")]
        [HttpGet]
        [ResponseType(typeof(SiteViewModel))]
        public IHttpActionResult GetSites(string id)
        {
            KDSCMSSITE KDSCMSSITE = db.KDSCMSSITE.Find(id);
            if (KDSCMSSITE == null)
            {
                return NotFound();
            }
            SiteViewModel site = new SiteViewModel();
            site.siteid = KDSCMSSITE.SITESITE;
            site.siteclass = KDSCMSSITE.SITESCLAS;
            site.sitename = KDSCMSSITE.SITESITENAME;
            site.siteflag = KDSCMSSITE.SITESITEFLAG;
            site.sitestatus = KDSCMSSITE.SITESITESTATUS;
            return Ok(site);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KDSCMSSITEExists(string id)
        {
            return db.KDSCMSSITE.Count(e => e.SITESITE == id) > 0;
        }
    }
}