namespace OopRestaurant201807.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// a visszair�ny� navig�ci�s property nem okozza az adatb�zis strukt�ra v�ltoz�s�t, �gy
    /// ha gy�rtok a v�ltoz�sb�l migr�ci�s l�p�st, az �res lesz.
    /// </summary>
    public partial class addreversenavigationpropertyCategoryMenuItems : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
        }
    }
}
