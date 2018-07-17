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
    public class MenuItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Nyilvános Action-ök
        // GET: MenuItems
        public ActionResult Index()
        {
            return View(db.MenuItems.ToList());
        }

        // GET: MenuItems/Details/5
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
            GetMenuItemEntryAndLoadCategory(menuItem);
            return View(menuItem);
        }
        #endregion Nyilvános Action-ök

        #region Csak bejelentkezett felhasználók által használható actionök
        // GET: MenuItems/Create
        /// <summary>
        /// Kikényszerítjük, hogy csak bejelentkezett 
        /// felhasználók használhassák ezt az Action-t
        /// </summary>
        [Authorize]
        public ActionResult Create()
        {
            var menuItem = new MenuItem();
            //a lenyílómező választható adatainak a feltöltése

            FillAssignableCategories(menuItem);

            return View(menuItem);
        }

        /// <summary>
        /// A Category választó lenyílómező adattartalmát betölti a menuItem.AssignableCategories property-be
        /// </summary>
        /// <param name="menuItem">az adatmodell, ahova a lenyíló adattartalmát be kell tölteni</param>
        private void FillAssignableCategories(MenuItem menuItem)
        {
            foreach (var category in db.Categories.ToList())
            {
                menuItem.AssignableCategories.Add(new SelectListItem() { Text = category.Name, Value = category.Id.ToString() });
            }
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem)
        {
            //a menuItem.Category kitöltése
            //keressük ki a megfelelő kategóriát az adatbázisból
            var category = db.Categories.Find(menuItem.CategoryId);

            //töltsük ki a modellünket ezzel a kategóriával
            menuItem.Category = category;

            //a validáció a modell átvételekor megtörtént, automatikusan nem frissül
            //ezért nekünk kell újra validálnunk

            //a TryValidateModel nem törli az előző validálás hibáit, így 
            //a teljes újravalidáláshoz először törölni kell ezeket
            ModelState.Clear();

            //immár tiszta lappal indulva validálunk
            var isValid = TryValidateModel(menuItem);

            //todo: az előző sort ide integrálhatjuk
            if (ModelState.IsValid)
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            FillAssignableCategories(menuItem);
            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
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

            //lenyílő mező adatainak kezelése
            //ha létezik az id azonosítójú menuItem, csak akkor tudjuk kitölteni a lenyíló adatait
            //menuItem.AssignableCategories feltöltése
            FillAssignableCategories(menuItem);

            //menuItem.CategoryId feltöltése
            menuItem.CategoryId = menuItem.Category.Id;

            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem)
        {
            //a menuItem.Category kitöltése
            //keressük ki a megfelelő kategóriát az adatbázisból
            var category = db.Categories.Find(menuItem.CategoryId);

            var menuItemEntry = GetMenuItemEntryAndLoadCategory(menuItem);

            //beállítjuk a megfelelő értéket 
            menuItem.Category = category;

            //a validáció a modell átvételekor megtörtént, automatikusan nem frissül
            //ezért nekünk kell újra validálnunk

            //a TryValidateModel nem törli az előző validálás hibáit, így 
            //a teljes újravalidáláshoz először törölni kell ezeket
            ModelState.Clear();

            //immár tiszta lappal indulva validálunk
            var isValid = TryValidateModel(menuItem);

            if (ModelState.IsValid)
            {
                //ez jelzi az EF-nek, hogy módosítottuk a menuItem-et
                menuItemEntry.State = EntityState.Modified;

                //ezért ez elmenti az adatokat
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuItem);
        }

        /// <summary>
        /// Elkéri a dbEntry kapcsolatot az EntityFramework-től és betölti a Category navigációs property-t
        /// </summary>
        /// <param name="menuItem">a betöltendő menuItem</param>
        /// <returns></returns>
        private System.Data.Entity.Infrastructure.DbEntityEntry<MenuItem> GetMenuItemEntryAndLoadCategory(MenuItem menuItem)
        {
            //ezzel bemutatjuk a menuItem-et az EntityFramework-nak
            //innen fogja tudni majd betölteni a navigációs property-t is
            db.MenuItems.Attach(menuItem);

            //ezzel az adatok mentését készítjük elő
            var menuItemEntry = db.Entry(menuItem);

            //betöltjük a navigációs property-t, ezzel egyben
            //az EntityFramework tudomást szerez a létezéséről
            //és a változását már el is menti
            //figyelem: ha korábban kitöltenénk a Category property-t, akkor ez NEM CSINÁL SEMMIT!!!
            menuItemEntry.Reference(x => x.Category)
                         .Load();
            return menuItemEntry;
        }

        // GET: MenuItems/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
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

            //ha nem akarom nem veszem át a visszatérési értéket
            GetMenuItemEntryAndLoadCategory(menuItem);

            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = db.MenuItems.Find(id);
            db.MenuItems.Remove(menuItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion Csak bejelentkezett felhasználók által használható actionök

        #region takarítás magunk után Dispose-zal
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion takarítás magunk után Dispose-zal
    }
}
