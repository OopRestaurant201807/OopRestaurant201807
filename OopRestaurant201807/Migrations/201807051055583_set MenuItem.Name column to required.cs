namespace OopRestaurant201807.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setMenuItemNamecolumntorequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MenuItems", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MenuItems", "Name", c => c.String());
        }
    }
}
