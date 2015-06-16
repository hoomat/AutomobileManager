namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Automobile", "AutomobileClassId", c => c.Int());
            AddColumn("dbo.Automobile", "ColorId", c => c.Int());
            CreateIndex("dbo.Automobile", "AutomobileClassId");
            CreateIndex("dbo.Automobile", "ColorId");
            AddForeignKey("dbo.Automobile", "AutomobileClassId", "dbo.AutomobileClasses", "ID");
            AddForeignKey("dbo.Automobile", "ColorId", "dbo.Colors", "ID");
            DropColumn("dbo.Automobile", "Model");
            DropColumn("dbo.Automobile", "Color");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Automobile", "Color", c => c.String());
            AddColumn("dbo.Automobile", "Model", c => c.String());
            DropForeignKey("dbo.Automobile", "ColorId", "dbo.Colors");
            DropForeignKey("dbo.Automobile", "AutomobileClassId", "dbo.AutomobileClasses");
            DropIndex("dbo.Automobile", new[] { "ColorId" });
            DropIndex("dbo.Automobile", new[] { "AutomobileClassId" });
            DropColumn("dbo.Automobile", "ColorId");
            DropColumn("dbo.Automobile", "AutomobileClassId");
        }
    }
}
