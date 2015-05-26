namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Repair", "DepartmentId", c => c.Int());
            CreateIndex("dbo.Repair", "DepartmentId");
            AddForeignKey("dbo.Repair", "DepartmentId", "dbo.Department", "ID");
            DropColumn("dbo.Repair", "CommanderPerson");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Repair", "CommanderPerson", c => c.String(nullable: false));
            DropForeignKey("dbo.Repair", "DepartmentId", "dbo.Department");
            DropIndex("dbo.Repair", new[] { "DepartmentId" });
            DropColumn("dbo.Repair", "DepartmentId");
        }
    }
}
