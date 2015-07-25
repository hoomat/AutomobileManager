namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Automobile", "Numberofengines", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Automobile", "Numberofengines");
        }
    }
}
