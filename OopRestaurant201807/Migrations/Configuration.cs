namespace OopRestaurant201807.Migrations
{
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

            context.Categories.AddOrUpdate(category1);
            context.Categories.AddOrUpdate(category2);
            context.Categories.AddOrUpdate(category3);

            //hozzuk létre az ételeket
            //Id Name    Description Price   Category_Id
            //1   Tengeri hal trió Atlanti lazactatár, pácolt lazacfilé és tonhal lazackaviárral   7500    2
            //3   Borjúesszencia Zöldséges gyöngytyúk galuska    4500    1
            //5   Szarvasgomba cappuccino NULL    4500    1
            //6   Hirtelen sült fogasderék illatos erdei gombákkal    NULL    4500    4
            //8   Gundel Károly gulyáslevese 1910 NULL    3500    1
            //9   Szárított érlelt bélszín carpaccio  Öreg Trappista sajt, keserû levelek 5000    2

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Tengeri hal trió",
                Description = "Atlanti lazactatár, pácolt lazacfilé és tonhal lazackaviárral",
                Price = 7500,
                Category = category2
            });

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Borjúesszencia",
                Description = "Zöldséges gyöngytyúk galuska",
                Price = 4500,
                Category = category1
            });

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Szarvasgomba cappuccino",
                Price = 4500,
                Category = category1
            });

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Hirtelen sült fogasderék illatos erdei gombákkal",
                Price = 4500,
                Category = category3
            });

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Gundel Károly gulyáslevese 1910",
                Price = 3500,
                Category = category1
            });

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Szárított érlelt bélszín carpaccio",
                Description = "Öreg Trappista sajt, keserû levelek",
                Price = 5000,
                Category = category2
            });

        }
    }
}
