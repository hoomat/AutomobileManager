namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transit", "ReturnDate", c => c.DateTime());
            AlterColumn("dbo.Transit", "MileagAfterTrip", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transit", "MileagAfterTrip", c => c.Int(nullable: false));
            AlterColumn("dbo.Transit", "ReturnDate", c => c.DateTime(nullable: false));
        }
    }
}
