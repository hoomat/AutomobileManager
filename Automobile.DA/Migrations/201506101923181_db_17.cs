namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Repair", "DateReturnRepair", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Repair", "DateReturnRepair");
        }
    }
}
