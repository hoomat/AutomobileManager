namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Automobile", "FuelCard", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Automobile", "FuelCard");
        }
    }
}
