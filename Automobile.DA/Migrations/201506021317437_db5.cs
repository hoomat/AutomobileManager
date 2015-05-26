namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        Role_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.UserGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.GroupRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.GroupRoles", "GroupId", "dbo.Groups");
            DropIndex("dbo.GroupRoles", new[] { "Role_Id" });
            DropIndex("dbo.GroupRoles", new[] { "GroupId" });
            DropTable("dbo.Groups");
            DropTable("dbo.GroupRoles");
        }
    }
}
