namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConsumablePart",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        Price = c.String(nullable: false),
                        RepairId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Repair", t => t.RepairId, cascadeDelete: true)
                .Index(t => t.RepairId);
            
            CreateTable(
                "dbo.PartType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConsumablePart", "RepairId", "dbo.Repair");
            DropIndex("dbo.ConsumablePart", new[] { "RepairId" });
            DropTable("dbo.PartType");
            DropTable("dbo.ConsumablePart");
        }
    }
}
