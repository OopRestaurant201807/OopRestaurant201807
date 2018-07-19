using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OopRestaurant201807.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name="Kategória")]
        public string Name { get; set; }

        //újabb navigation property, ide felsoroljuk az összes 
        //adott kategóriába tartozó menuItem-et.
        public List<MenuItem> MenuItems { get; set; }
    }
}