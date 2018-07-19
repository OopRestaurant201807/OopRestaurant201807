namespace OopRestaurant201807.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableIsNonSmoking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "IsNonSmoking", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "IsNonSmoking");
        }
    }
}
