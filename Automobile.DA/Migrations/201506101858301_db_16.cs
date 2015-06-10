namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Automobile", "AutomobileStatusId", "dbo.AutomobileStatus");
            DropIndex("dbo.Automobile", new[] { "AutomobileStatusId" });
            AlterColumn("dbo.Automobile", "AutomobileStatusId", c => c.Int());
            CreateIndex("dbo.Automobile", "AutomobileStatusId");
            AddForeignKey("dbo.Automobile", "AutomobileStatusId", "dbo.AutomobileStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Automobile", "AutomobileStatusId", "dbo.AutomobileStatus");
            DropIndex("dbo.Automobile", new[] { "AutomobileStatusId" });
            AlterColumn("dbo.Automobile", "AutomobileStatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.Automobile", "AutomobileStatusId");
            AddForeignKey("dbo.Automobile", "AutomobileStatusId", "dbo.AutomobileStatus", "Id", cascadeDelete: true);
        }
    }
}
