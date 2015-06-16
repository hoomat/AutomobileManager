namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_24 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Repair", "Workshop", c => c.String());
            AlterColumn("dbo.Repair", "InvoiceNo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Repair", "InvoiceNo", c => c.String(nullable: true));
            AlterColumn("dbo.Repair", "Workshop", c => c.String(nullable: true));
        }
    }
}
