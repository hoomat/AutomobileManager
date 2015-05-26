namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Repair", "Wage", c => c.Int());
            AlterColumn("dbo.Repair", "Cost", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Repair", "Cost", c => c.String(nullable: false));
            DropColumn("dbo.Repair", "Wage");
        }
    }
}
