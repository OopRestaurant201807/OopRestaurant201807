namespace OopRestaurant201807.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// a visszairányú navigációs property nem okozza az adatbázis struktúra változását, így
    /// ha gyártok a változásból migrációs lépést, az üres lesz.
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
