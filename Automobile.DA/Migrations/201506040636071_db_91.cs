namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_91 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transit", "TrafficCardID", "dbo.TrafficCard");
            DropIndex("dbo.Transit", new[] { "TrafficCardID" });
            AlterColumn("dbo.Transit", "TrafficCardID", c => c.Int());
            CreateIndex("dbo.Transit", "TrafficCardID");
            AddForeignKey("dbo.Transit", "TrafficCardID", "dbo.TrafficCard", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transit", "TrafficCardID", "dbo.TrafficCard");
            DropIndex("dbo.Transit", new[] { "TrafficCardID" });
            AlterColumn("dbo.Transit", "TrafficCardID", c => c.Int(nullable: false));
            CreateIndex("dbo.Transit", "TrafficCardID");
            AddForeignKey("dbo.Transit", "TrafficCardID", "dbo.TrafficCard", "ID", cascadeDelete: true);
        }
    }
}
