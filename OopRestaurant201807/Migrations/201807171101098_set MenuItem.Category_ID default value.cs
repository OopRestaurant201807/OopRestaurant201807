namespace OopRestaurant201807.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setMenuItemCategory_IDdefaultvalue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MenuItems", "Category_Id", c => c.Int(defaultValue: 1));
            Sql("update dbo.MenuItems set Category_Id=1 where Category_Id is null");
        }
        
        public override void Down()
        {
            //todo: visszavonási lépés
        }
    }
}
