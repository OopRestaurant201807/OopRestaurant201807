namespace OopRestaurant201807.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrequiredtoMenuItemCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories");
            DropIndex("dbo.MenuItems", new[] { "Category_Id" });
            //az oszlopnak megadjuk az alapértelmezett értékét azért, hogy ha véletlenül van null érték a táblában, tudja, hogy mit írjon bele
            AlterColumn("dbo.MenuItems", "Category_Id", c => c.Int(nullable: false, defaultValue: 1, defaultValueSql: "1"));
            CreateIndex("dbo.MenuItems", "Category_Id");
            AddForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories");
            DropIndex("dbo.MenuItems", new[] { "Category_Id" });
            AlterColumn("dbo.MenuItems", "Category_Id", c => c.Int());
            CreateIndex("dbo.MenuItems", "Category_Id");
            AddForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
