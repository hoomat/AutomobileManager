namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Automobils", newName: "Automobile");
            RenameColumn(table: "dbo.Attendance", name: "Transit_ID", newName: "TransitID");
            RenameIndex(table: "dbo.Attendance", name: "IX_Transit_ID", newName: "IX_TransitID");
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IncidentDate = c.DateTime(nullable: false),
                        DriverID = c.Int(nullable: false),
                        AutomobileID = c.Int(),
                        Description = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Automobile", t => t.AutomobileID)
                .ForeignKey("dbo.Driver", t => t.DriverID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.IdentityUser_Id)
                .Index(t => t.DriverID)
                .Index(t => t.AutomobileID)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.Damage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IncidentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Incidents", t => t.IncidentID, cascadeDelete: true)
                .Index(t => t.IncidentID);
            
            AddColumn("dbo.Repair", "IncidentID", c => c.Int());
            CreateIndex("dbo.Repair", "IncidentID");
            AddForeignKey("dbo.Repair", "IncidentID", "dbo.Incidents", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Repair", "IncidentID", "dbo.Incidents");
            DropForeignKey("dbo.Incidents", "IdentityUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incidents", "DriverID", "dbo.Driver");
            DropForeignKey("dbo.Damage", "IncidentID", "dbo.Incidents");
            DropForeignKey("dbo.Incidents", "AutomobileID", "dbo.Automobile");
            DropIndex("dbo.Repair", new[] { "IncidentID" });
            DropIndex("dbo.Damage", new[] { "IncidentID" });
            DropIndex("dbo.Incidents", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Incidents", new[] { "AutomobileID" });
            DropIndex("dbo.Incidents", new[] { "DriverID" });
            DropColumn("dbo.Repair", "IncidentID");
            DropTable("dbo.Damage");
            DropTable("dbo.Incidents");
            RenameIndex(table: "dbo.Attendance", name: "IX_TransitID", newName: "IX_Transit_ID");
            RenameColumn(table: "dbo.Attendance", name: "TransitID", newName: "Transit_ID");
            RenameTable(name: "dbo.Automobile", newName: "Automobils");
        }
    }
}
