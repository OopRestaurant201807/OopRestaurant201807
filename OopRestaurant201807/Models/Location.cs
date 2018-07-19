using System.Collections.Generic;

namespace OopRestaurant201807.Models
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Jelzi, hogy a terem vagy terület nemdohányzó
        /// </summary>
        public bool IsNonSmoking { get; set; }

        /// <summary>
        /// A helyszínhez tartozó asztalok felsorolása
        /// virtual: LazyLoading engedélyezése
        /// </summary>
        public virtual List<Table> Tables { get; set; }
    }
}