using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OopRestaurant201807.Models;

namespace OopRestaurant201807.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Menu
        public ActionResult Index()
        {
            var model = db.Categories
                          //a navigációs property-t betöltjük a lekérdezéskor
                          .Include(c => c.MenuItems)
                          .OrderBy(c => c.Name)
                          .ToList();

            return View(model);
        }

        // GET: Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
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
