namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_18 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Repair", "DateReturnRepair", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Repair", "DateReturnRepair", c => c.DateTime(nullable: false));
        }
    }
}
