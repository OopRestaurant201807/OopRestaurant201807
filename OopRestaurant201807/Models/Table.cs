using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OopRestaurant201807.Models
{
    public class Table
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //Lazy loading kialakítása
        public virtual Location Location { get; set; }
    }
}