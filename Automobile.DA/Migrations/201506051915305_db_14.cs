namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LogId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Logs", t => t.LogId, cascadeDelete: true)
                .Index(t => t.LogId);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Level = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Username = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogDetails", "LogId", "dbo.Logs");
            DropIndex("dbo.LogDetails", new[] { "LogId" });
            DropTable("dbo.Logs");
            DropTable("dbo.LogDetails");
        }
    }
}
