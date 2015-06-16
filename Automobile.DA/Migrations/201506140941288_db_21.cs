namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_21 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutomobileClasses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Model = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Colors");
            DropTable("dbo.AutomobileClasses");
        }
    }
}
