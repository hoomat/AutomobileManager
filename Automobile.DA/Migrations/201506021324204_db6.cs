namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GroupRoles", new[] { "Role_Id" });
            DropColumn("dbo.GroupRoles", "RoleId");
            RenameColumn(table: "dbo.GroupRoles", name: "Role_Id", newName: "RoleId");
            AlterColumn("dbo.GroupRoles", "RoleId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.GroupRoles", "RoleId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GroupRoles", new[] { "RoleId" });
            AlterColumn("dbo.GroupRoles", "RoleId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.GroupRoles", name: "RoleId", newName: "Role_Id");
            AddColumn("dbo.GroupRoles", "RoleId", c => c.Int(nullable: false));
            CreateIndex("dbo.GroupRoles", "Role_Id");
        }
    }
}
