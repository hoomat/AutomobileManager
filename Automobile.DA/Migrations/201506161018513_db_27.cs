namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_27 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "ObjectId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "ObjectId");
        }
    }
}
