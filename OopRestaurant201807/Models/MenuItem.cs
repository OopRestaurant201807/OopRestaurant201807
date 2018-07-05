using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OopRestaurant201807.Models
{
    /// <summary>
    /// Egy elem adatai a menün
    /// </summary>
    public class MenuItem
    {
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
    }
}