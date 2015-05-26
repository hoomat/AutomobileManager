namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ConsumablePart", "Price", c => c.Int(nullable: false));
            AlterColumn("dbo.Repair", "Cost", c => c.Int());
            AlterColumn("dbo.Repair", "Wage", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Repair", "Wage", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Repair", "Cost", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ConsumablePart", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
