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
using NLog;

namespace cms.api.Controllers
{
    public class KDSCMSSITEsController : ApiController
    {
        private CMSContext db = new CMSContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/KDSCMSSITEs
        public IQueryable<KDSCMSSITE> GetKDSCMSSITE()
        {
            return db.KDSCMSSITE;
        }

        // GET: api/KDSCMSSITEs/5
        [ResponseType(typeof(KDSCMSSITE))]
        public IHttpActionResult GetKDSCMSSITE(string id)
        {
            KDSCMSSITE kDSCMSSITE = db.KDSCMSSITE.Find(id);
            if (kDSCMSSITE == null)
            {
                return NotFound();
            }

            return Ok(kDSCMSSITE);
        }

        // PUT: api/KDSCMSSITEs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKDSCMSSITE(string id, KDSCMSSITE kDSCMSSITE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kDSCMSSITE.SITESITE)
            {
                return BadRequest();
            }

            db.Entry(kDSCMSSITE).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KDSCMSSITEExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/KDSCMSSITEs
        [ResponseType(typeof(KDSCMSSITE))]
        public IHttpActionResult PostKDSCMSSITE(KDSCMSSITE kDSCMSSITE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.KDSCMSSITE.Add(kDSCMSSITE);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (KDSCMSSITEExists(kDSCMSSITE.SITESITE))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = kDSCMSSITE.SITESITE }, kDSCMSSITE);
        }

        // DELETE: api/KDSCMSSITEs/5
        [ResponseType(typeof(KDSCMSSITE))]
        public IHttpActionResult DeleteKDSCMSSITE(string id)
        {
            KDSCMSSITE kDSCMSSITE = db.KDSCMSSITE.Find(id);
            if (kDSCMSSITE == null)
            {
                return NotFound();
            }

            db.KDSCMSSITE.Remove(kDSCMSSITE);
            db.SaveChanges();

            return Ok(kDSCMSSITE);
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