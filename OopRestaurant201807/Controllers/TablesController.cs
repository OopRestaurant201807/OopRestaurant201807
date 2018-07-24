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
    public class TablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tables
        public ActionResult Index()
        {
            return View(db.Tables.ToList());
        }

        // GET: Tables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // GET: Tables/Create
        public ActionResult Create()
        {
            var table = new Table();
            FillAssignablaLocations(table);

            return View(table);
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LocationId")] Table table)
        {
            //todo: ha a Location mező Required lesz, akkor újra kell validálni

            if (ModelState.IsValid)
            {
                var location = db.Locations.Find(table.LocationId);
                table.Location = location;
                db.Tables.Add(table);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(table);
        }

        // GET: Tables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);

            FillAssignablaLocations(table);

            //az aktuálisan kiválasztott helyszín
            table.LocationId = table.Location.Id;

            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        private void FillAssignablaLocations(Table table)
        {
            //lenyíló adatainak a feltöltése
            table.AssignableLocations = db.Locations
                                          .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                                          .ToList()
                                          ;
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LocationId")] Table table)
        {
            if (ModelState.IsValid)
            {
                //betöltjük az adatbázisból a Location példányt
                var location = db.Locations.Find(table.LocationId);

                //betöltjük az asztal aktuális adatait
                var tableToUpdate = db.Tables.Find(table.Id);

                //abban az esetben, ha a property nem virtual, vagyis nincs
                //LazyLoading, így tudunk navigázisó property-t tölteni automatikusan

                //var tableToUpdate = db.Tables
                //                      .Include(x=>x.Location)
                //                      .FirstOrDefault(x => x.Id == table.Id);

                //az aktuális értékeket át kell írni a most kapott értékekkel
                tableToUpdate.Name = table.Name;
                //itt az összes módosítandó property-t fel kell sorolni

                //beállítjuk a helyszín értékét
                tableToUpdate.Location = location;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table);
        }

        // GET: Tables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Table table = db.Tables.Find(id);
            db.Tables.Remove(table);
            db.SaveChanges();
            return RedirectToAction("Index");
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
