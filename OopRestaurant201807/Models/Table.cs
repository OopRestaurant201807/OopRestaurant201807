using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OopRestaurant201807.Models
{
    public class Table
    {
        public Table()
        {
            //null object pattern: az osztály kívülről elérhető 
            //gyüjteményeinek alapértelmezett értéket adunk
            AssignableLocations = new List<SelectListItem>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        //todo: Required
        //Lazy loading kialakítása
        public virtual Location Location { get; set; }

        /// <summary>
        /// ViewModel: Controller és a Nézetek között hozza viszi az adatokat)
        /// </summary>
        #region a ViewModel részei 

        [NotMapped] //csak viewmodel, adatbázisba nem kerül
        public int LocationId { get; set; }

        [NotMapped] //csak viewmodel, adatbázisba nem kerül
        public List<SelectListItem> AssignableLocations { get; set; }

        #endregion a ViewModel részei
    }
}