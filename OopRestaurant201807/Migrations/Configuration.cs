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


            context.Categories.AddOrUpdate(x => x.Name, category1, category2, category3);
            //context.Categories.AddOrUpdate(x => x.Name, category2);
            //context.Categories.AddOrUpdate(x => x.Name, category3);

            //hozzuk l�tre az �teleket
            //Id Name    Description Price   Category_Id
            //1   Tengeri hal tri� Atlanti lazactat�r, p�colt lazacfil� �s tonhal lazackavi�rral   7500    2
            //3   Borj�esszencia Z�lds�ges gy�ngyty�k galuska    4500    1
            //5   Szarvasgomba cappuccino NULL    4500    1
            //6   Hirtelen s�lt fogasder�k illatos erdei gomb�kkal    NULL    4500    4
            //8   Gundel K�roly guly�slevese 1910 NULL    3500    1
            //9   Sz�r�tott �rlelt b�lsz�n carpaccio  �reg Trappista sajt, keser� levelek 5000    2

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem()
            {
                Name = "Tengeri hal tri�",
                Description = "Atlanti lazactat�r, p�colt lazacfil� �s tonhal lazackavi�rral",
                Price = 7500,
                Category = category2
            });

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem()
            {
                Name = "Borj�esszencia",
                Description = "Z�lds�ges gy�ngyty�k galuska (m�dos�tva)",
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
                Name = "Hirtelen s�lt fogasder�k illatos erdei gomb�kkal",
                Price = 4500,
                Category = category3
            });

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem()
            {
                Name = "Gundel K�roly guly�slevese 1910",
                Price = 3500,
                Category = category1
            });

            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem()
            {
                Name = "Sz�r�tott �rlelt b�lsz�n carpaccio",
                Description = "�reg Trappista sajt, keser� levelek",
                Price = 5000,
                Category = category2
            });




            //helysz�nek felt�lt�se
            var loc1 = new Location { Name = "Nemdoh�nyz� terem", IsNonSmoking = true };
            var loc2 = new Location { Name = "Doh�nyz� terem", IsNonSmoking = false };
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

            //cook, waiter, admin
            // jogosults�gcsoport r�gz�t�se
            AddRoleIfNotExists(context, "admin");
            AddRoleIfNotExists(context, "cook");
            AddRoleIfNotExists(context, "waiter");

            //felhaszn�l�k r�gz�t�se
            AddUserIfNotExists(context, "gabor.plesz@gmail.com", "gabor.plesz@gmail.com", "admin,cook,waiter");
            AddUserIfNotExists(context, "pincer@gmail.com", "pincer@gmail.com", "waiter");
            AddUserIfNotExists(context, "szakacs@gmail.com", "szakacs@gmail.com", "cook");
        }


        /// <summary>
        /// Felhaszn�l�csoport r�gz�t�se, ha m�g nem l�tezik
        /// </summary>
        /// <param name="context"></param>
        /// <param name="roleName"></param>
        private void AddRoleIfNotExists(ApplicationDbContext context, string roleName)
        {

            //RoleStore: adatb�zist �r� r�teg
            //RoleManager: az alkalmaz�s fel� az egys�ges fel�let

            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);

            var roleExists= manager.FindByName(roleName);
            if (roleExists==null)
            { //nincs m�g ilyen, l�tre kell hoznunk
                var role = new IdentityRole(roleName);
                var result = manager.Create(role);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(",", result.Errors));
                }
            }
        }

        /// <summary>
        /// felhaszn�l� r�gz�t�se, ha m�g nem l�tezik
        ///   figyelem: nem r�gz�t�nk adatb�zisba k�zvetlen�l adatot, 
        ///   hanem az Identity �ltal k�n�lt szolg�ltat�s(oka)t haszn�ljuk
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        private static void AddUserIfNotExists(ApplicationDbContext context, string userName, string email, string roles)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
            };

            //UserStore: ez felel az adatok r�gz�t�s��rt
            //UserManager: a programoz�si fel�let.
            //context <- UserStore <- UserManager

            var store = new UserStore<ApplicationUser>(context);
            var manager = new ApplicationUserManager(store);

            //ellen�rizni kell, hogy l�tezik-e m�r ilyen felhaszn�l�?
            var userExists = manager.FindByEmail(user.Email);
            if (null == userExists)
            { // m�g nincs ilyen felhaszn�l�, r�gz�ts�k
              //itt megadjuk a felhaszn�l� jelszav�t, �s 
              //�gy az Identity gener�lja az adatb�zisba �rt HASH k�dot
                var result = manager.Create(user, "123456");
                if (!result.Succeeded)
                {

                    ////a legr�szletesebb megold�s
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

                    ////el�z� megold�s t�m�r�tve
                    //foreach (var error in result.Errors)
                    //{
                    //    //felt�teles (tern�ris) oper�tor haszn�lata
                    //    errorMessage = errorMessage
                    //        + (string.IsNullOrEmpty(errorMessage) ? "" : ",")
                    //        + error;
                    //}

                    //a legt�m�rebb megold�s pedig a string oszt�ly be�p�tett Join f�ggv�nye
                    throw new Exception(string.Join(",", result.Errors));
                }

                //hozz�rendel�s jogosults�gcsoportokhoz
                foreach (var role in roles.Split(',')) //a vessz� ment�n sz�tszedj�k a param�tert t�mbre, amin v�gigs�t�lunk
                {
                    manager.AddToRole(user.Id, role);
                }
            }
        }
    }
}
