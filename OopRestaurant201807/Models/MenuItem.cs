using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OopRestaurant201807.Models
{
    /// <summary>
    /// Egy elem adatai a menün
    /// </summary>
    public class MenuItem
    {

        public MenuItem()
        {
            AssignableCategories = new List<SelectListItem>();
        }

        /// <summary>
        /// Ez a korábbi ismereteink szerint kötelező
        /// PK: Primary Key: elsődleges kulcs
        /// 
        /// A névkonvenció alapján, ami int típusú és Id nevű, abból lesz a PK,
        /// hacsak nincs kijelölve más mező a [Key] annotációval
        /// </summary>
        public int Id { get; set; }

        //a nevet kötelező megadni, mert nincs étel név nélkül
        [Required]
        /// <summary>
        /// Ahhoz, hogy később indexelni lehessen ezt a mezőt az SQL szerveren, ahhoz
        /// arra van szükség, hogy ne legyen korlátlan hosszú, ezért a hosszát korlátozzuk.
        /// </summary>
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        /// <summary>
        /// Ez az osztály az adott menüelem kategóriáját 
        /// tartalmazza
        /// Navigation Property: egy másik táblában lévő adatot tölt be
        /// </summary>
        [Required] // ezzel biztosítjuk, hogy a táblában a távoli kulcs mindig legyen kitöltve.
        //[DefaultValue] //todo: ez hogy működik?
        public Category Category { get; set; }

        #region Csak a nézetekre kerülő propertyk
        /// <summary>
        /// ezzel jelzem a CodeFirst-nek, hogy nem akarom adatbázisban látni, 
        /// így nem foglalkozik majd vele
        /// 
        /// Ez a property tartalmazza a lenyílőmező lehetséges választható adatait
        /// </summary>
        [NotMapped]
        public List<SelectListItem> AssignableCategories { get; set; }

        /// <summary>
        /// Itt tároljuk a lenyílómező aktuálisan kiválasztott Category példány azonosítóját
        /// </summary>
        [NotMapped]
        public int CategoryId { get; set; }
        #endregion Csak a nézetekre kerülő propertyk
    }
}