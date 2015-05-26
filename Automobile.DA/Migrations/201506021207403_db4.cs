namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Menu", "Parent_ID", "dbo.Menu");
            DropIndex("dbo.Menu", new[] { "Parent_ID" });
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.AspNetUsers", "DepartmentId", c => c.Int());
            AddColumn("dbo.AspNetRoles", "Title", c => c.String());
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "DepartmentId");
            AddForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Department", "ID");
            DropTable("dbo.Menu");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Role = c.String(),
                        Name = c.String(),
                        Title = c.String(),
                        Link = c.String(),
                        Class = c.String(),
                        Style = c.String(),
                        Parent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Department");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentId" });
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetRoles", "Title");
            DropColumn("dbo.AspNetUsers", "DepartmentId");
            DropTable("dbo.UserGroups");
            CreateIndex("dbo.Menu", "Parent_ID");
            AddForeignKey("dbo.Menu", "Parent_ID", "dbo.Menu", "ID");
        }
    }
}
