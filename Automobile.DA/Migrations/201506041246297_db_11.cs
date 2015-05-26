namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ConsumablePart", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ConsumablePart", "Price", c => c.String(nullable: false));
        }
    }
}
