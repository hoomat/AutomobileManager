namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AutomobileClasses", "Class", c => c.String(nullable: false));
            DropColumn("dbo.AutomobileClasses", "Model");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AutomobileClasses", "Model", c => c.String(nullable: false));
            DropColumn("dbo.AutomobileClasses", "Class");
        }
    }
}
