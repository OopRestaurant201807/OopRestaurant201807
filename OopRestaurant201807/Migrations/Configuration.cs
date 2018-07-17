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
        /// Ez a f�ggv�ny minden update-database fut�sakor a migr�ci�s l�p�sek v�grehajt�sa ut�n lefut
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(OopRestaurant201807.Models.ApplicationDbContext context)
        {
            //hozzuk l�tre a kateg�ri�kat
            //Id Name
            //1   Levesek
            //2   Hideg el��telek
            //4   Meleg el��telek

            var category1 = new Category() { Name = "Levesek" };
            var category2 = new Category() { Name = "Hideg el��telek" };
            var category3 = new Category() { Name = "Meleg el��telek" };

            context.Categories.AddOrUpdate(category1);
            context.Categories.AddOrUpdate(category2);
            context.Categories.AddOrUpdate(category3);

            //hozzuk l�tre az �teleket
            //Id Name    Description Price   Category_Id
            //1   Tengeri hal tri� Atlanti lazactat�r, p�colt lazacfil� �s tonhal lazackavi�rral   7500    2
            //3   Borj�esszencia Z�lds�ges gy�ngyty�k galuska    4500    1
            //5   Szarvasgomba cappuccino NULL    4500    1
            //6   Hirtelen s�lt fogasder�k illatos erdei gomb�kkal    NULL    4500    4
            //8   Gundel K�roly guly�slevese 1910 NULL    3500    1
            //9   Sz�r�tott �rlelt b�lsz�n carpaccio  �reg Trappista sajt, keser� levelek 5000    2

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Tengeri hal tri�",
                Description = "Atlanti lazactat�r, p�colt lazacfil� �s tonhal lazackavi�rral",
                Price = 7500,
                Category = category2
            });

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Borj�esszencia",
                Description = "Z�lds�ges gy�ngyty�k galuska",
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
                Name = "Hirtelen s�lt fogasder�k illatos erdei gomb�kkal",
                Price = 4500,
                Category = category3
            });

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Gundel K�roly guly�slevese 1910",
                Price = 3500,
                Category = category1
            });

            context.MenuItems.AddOrUpdate(new MenuItem()
            {
                Name = "Sz�r�tott �rlelt b�lsz�n carpaccio",
                Description = "�reg Trappista sajt, keser� levelek",
                Price = 5000,
                Category = category2
            });

        }
    }
}
