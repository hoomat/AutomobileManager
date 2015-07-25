namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Automobile", "TrafficCardId", c => c.Int());
            CreateIndex("dbo.Automobile", "TrafficCardId");
            AddForeignKey("dbo.Automobile", "TrafficCardId", "dbo.TrafficCard", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Automobile", "TrafficCardId", "dbo.TrafficCard");
            DropIndex("dbo.Automobile", new[] { "TrafficCardId" });
            DropColumn("dbo.Automobile", "TrafficCardId");
        }
    }
}
