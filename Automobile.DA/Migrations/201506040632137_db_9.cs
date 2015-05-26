namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transit", "Distance", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transit", "Distance", c => c.Int(nullable: false));
        }
    }
}
