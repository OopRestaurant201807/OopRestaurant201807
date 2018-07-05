using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public string Description { get; set; }
    }
}