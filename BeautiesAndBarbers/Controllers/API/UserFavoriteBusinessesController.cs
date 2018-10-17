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
using BeautiesAndBarbers.Models;

namespace BeautiesAndBarbers.Controllers.API
{
    public class UserFavoriteBusinessesController : ApiController
    {
        private BnBContext db = new BnBContext();

        // GET: api/UserFavoriteBusinesses
        public IQueryable<UserFavoriteBusiness> GetUserFavoriteBusinesses()
        {
            return db.UserFavoriteBusinesses;
        }

        // GET: api/UserFavoriteBusinesses/5
        [ResponseType(typeof(UserFavoriteBusiness))]
        public IHttpActionResult GetUserFavoriteBusiness(int id)
        {
            UserFavoriteBusiness userFavoriteBusiness = db.UserFavoriteBusinesses.Find(id);
            if (userFavoriteBusiness == null)
            {
                return NotFound();
            }

            return Ok(userFavoriteBusiness);
        }

        // PUT: api/UserFavoriteBusinesses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserFavoriteBusiness(int id, [FromBody] UserFavoriteBusiness userFavoriteBusiness)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userFavoriteBusiness.UserFavoriteBusinessId)
            {
                return BadRequest();
            }

            db.Entry(userFavoriteBusiness).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFavoriteBusinessExists(id))
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

        // POST: api/UserFavoriteBusinesses
        [ResponseType(typeof(UserFavoriteBusiness))]
        public IHttpActionResult PostUserFavoriteBusiness(UserFavoriteBusiness userFavoriteBusiness)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserFavoriteBusinesses.Add(userFavoriteBusiness);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userFavoriteBusiness.UserFavoriteBusinessId }, userFavoriteBusiness);
        }

        // DELETE: api/UserFavoriteBusinesses/5
        [ResponseType(typeof(UserFavoriteBusiness))]
        public IHttpActionResult DeleteUserFavoriteBusiness(int id)
        {
            UserFavoriteBusiness userFavoriteBusiness = db.UserFavoriteBusinesses.Find(id);
            if (userFavoriteBusiness == null)
            {
                return NotFound();
            }

            db.UserFavoriteBusinesses.Remove(userFavoriteBusiness);
            db.SaveChanges();

            return Ok(userFavoriteBusiness);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserFavoriteBusinessExists(int id)
        {
            return db.UserFavoriteBusinesses.Count(e => e.UserFavoriteBusinessId == id) > 0;
        }
    }
}