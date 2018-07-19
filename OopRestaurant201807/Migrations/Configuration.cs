namespace OopRestaurant201807.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OopRestaurant201807.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OopRestaurant201807.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Ez a függvény minden update-database futásakor a migrációs lépések végrehajtása után lefut
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(OopRestaurant201807.Models.ApplicationDbContext context)
        {
            //hozzuk létre a kategóriákat
            //Id Name
            //1   Levesek
            //2   Hideg elõételek
            //4   Meleg elõételek

            var category1 = new Category() { Name = "Levesek" };
            var category2 = new Category() { Name = "Hideg elõételek" };
            var category3 = new Category() { Name = "Meleg elõételek" };


            context.Categories.AddOrUpdate(x => x.Name, category1, category2, category3);
            //context.Categories.AddOrUpdate(x => x.Name, category2);
            //context.Categories.AddOrUpdate(x => x.Name, category3);

            //hozzuk létre az ételeket
            //Id Name    Description Price   Category_Id
            //1   Tengeri hal trió Atlanti lazactatár, pácolt lazacfilé és tonhal lazackaviárral   7500    2
            //3   Borjúesszencia Zöldséges gyöngytyúk galuska    4500    1
            //5   Szarvasgomba cappuccino NULL    4500    1
            //6   Hirtelen sült fogasderék illatos erdei gombákkal    NULL    4500    4
            //8   Gundel Károly gulyáslevese 1910 NULL    3500    1
            //9   Szárított érlelt bélszín carpaccio  Öreg Trappista sajt, keserû levelek 5000    2

            context.MenuItems.AddOrUpdate(x=>x.Name, new MenuItem()
            {
                Name = "Tengeri hal trió",
                Description = "Atlanti lazactatár, pácolt lazacfilé és tonhal lazackaviárral",
                Price = 7500,
                Category = category2
            });

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem()
            {
                Name = "Borjúesszencia",
                Description = "Zöldséges gyöngytyúk galuska (módosítva)",
                Price = 4500,
                Category = category1
            });

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem()
            {
                Name = "Szarvasgomba cappuccino",
                Price = 4500,
                Category = category1
            });

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem()
            {
                Name = "Hirtelen sült fogasderék illatos erdei gombákkal",
                Price = 4500,
                Category = category3
            });

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem()
            {
                Name = "Gundel Károly gulyáslevese 1910",
                Price = 3500,
                Category = category1
            });

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem()
            {
                Name = "Szárított érlelt bélszín carpaccio",
                Description = "Öreg Trappista sajt, keserû levelek",
                Price = 5000,
                Category = category2
            });




            //helyszínek feltöltése
            var loc1 = new Location { Name = "Nemdohányzó terem", IsNonSmoking = true };
            var loc2 = new Location { Name = "Dohányzó terem", IsNonSmoking = false };
            var loc3 = new Location { Name = "Terasz", IsNonSmoking = false };
            context.Locations.AddOrUpdate(x => x.Name, loc1, loc2, loc3);

            context.Tables.AddOrUpdate(x => x.Name,
                new Table { Name = "1. asztal", Location = loc1 },
                new Table { Name = "2. asztal", Location = loc1 },
                new Table { Name = "3. asztal", Location = loc2 },
                new Table { Name = "4. asztal", Location = loc2 },
                new Table { Name = "5. asztal", Location = loc3 },
                new Table { Name = "6. asztal", Location = loc3 }
            );
            
            // felhasználó rögzítése
            // figyelem: nem rögzítünk adatbázisba közvetlenül adatot, 
            //hanem az Identity által kínált szolgáltatás(oka)t használjuk

            var user = new ApplicationUser
            {
                UserName = "gabor.plesz@gmail.com",
                Email = "gabor.plesz@gmail.com",
            };

            // UserStore: ez felel az adatok rögzítéséért
            //UserManager: a programozási felület.

            //context <- UserStore <- UserManager

            var store = new UserStore<ApplicationUser>(context);
            var manager = new ApplicationUserManager(store);

            //ellenõrizni kell, hogy létezik-e már ilyen felhasználó?
            var userExists = manager.FindByEmail(user.Email);
            if (null==userExists)
            { // még nincs ilyen felhasználó, rögzítsük
              //itt megadjuk a felhasználó jelszavát, és 
              //így az Identity generálja az adatbázisba írt HASH kódot
                var result = manager.Create(user, "123456");
                if (!result.Succeeded)
                {

                    ////a legrészletesebb megoldás
                    //var errorMessage = "";
                    //foreach (var error in result.Errors)
                    //{
                    //    if (string.IsNullOrEmpty(errorMessage))
                    //    {
                    //        errorMessage = error;
                    //    }
                    //    else
                    //    {
                    //        errorMessage = errorMessage + ", " + error;
                    //    }
                    //}

                    ////elõzõ megoldás tömörítve
                    //foreach (var error in result.Errors)
                    //{
                    //    //feltételes (ternáris) operátor használata
                    //    errorMessage = errorMessage
                    //        + (string.IsNullOrEmpty(errorMessage) ? "" : ",")
                    //        + error;
                    //}

                    //a legtömörebb megoldás pedig a string osztály beépített Join föggvénye
                    throw new Exception(string.Join(",", result.Errors));
                }
            }
        }
    }
}
