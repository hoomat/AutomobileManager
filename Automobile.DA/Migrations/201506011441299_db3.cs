namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Driver", "PersonalNumber", c => c.String());
            AddColumn("dbo.Driver", "Category", c => c.String());
            DropColumn("dbo.Driver", "Degree");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Driver", "Degree", c => c.String());
            DropColumn("dbo.Driver", "Category");
            DropColumn("dbo.Driver", "PersonalNumber");
        }
    }
}
