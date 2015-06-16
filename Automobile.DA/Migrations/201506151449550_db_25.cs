namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FuelConsume", "Mileag", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FuelConsume", "Mileag");
        }
    }
}
