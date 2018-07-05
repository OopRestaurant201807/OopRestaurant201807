namespace OopRestaurant201807.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMenuItemPricecolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItems", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuItems", "Price");
        }
    }
}
