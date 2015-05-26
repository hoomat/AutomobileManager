namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "GroupId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "GroupId");
            AddForeignKey("dbo.AspNetUsers", "GroupId", "dbo.Groups", "Id");
            DropColumn("dbo.AspNetUsers", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Type", c => c.Int());
            DropForeignKey("dbo.AspNetUsers", "GroupId", "dbo.Groups");
            DropIndex("dbo.AspNetUsers", new[] { "GroupId" });
            DropColumn("dbo.AspNetUsers", "GroupId");
        }
    }
}
