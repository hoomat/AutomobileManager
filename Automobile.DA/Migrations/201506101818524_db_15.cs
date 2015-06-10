namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutomobileStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Automobile", "AutomobileStatusId", c => c.Int(nullable: true));
            CreateIndex("dbo.Automobile", "AutomobileStatusId");
            AddForeignKey("dbo.Automobile", "AutomobileStatusId", "dbo.AutomobileStatus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Automobile", "AutomobileStatusId", "dbo.AutomobileStatus");
            DropIndex("dbo.Automobile", new[] { "AutomobileStatusId" });
            DropColumn("dbo.Automobile", "AutomobileStatusId");
            DropTable("dbo.AutomobileStatus");
        }
    }
}
