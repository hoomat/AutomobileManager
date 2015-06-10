namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_19 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Automobile", "FuelCard");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Automobile", "FuelCard", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
