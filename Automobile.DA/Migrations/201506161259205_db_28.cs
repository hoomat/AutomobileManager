namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Automobile", "AllowedDistance", c => c.Int());
            AddColumn("dbo.Automobile", "MaxAllowedDistance", c => c.Int());
            AddColumn("dbo.Automobile", "LastCheckAllowedDistance", c => c.DateTime());
            AddColumn("dbo.Automobile", "AllowedRepairDistance", c => c.Int());
            AddColumn("dbo.Automobile", "MaxAllowedRepairDistance", c => c.Int());
            AddColumn("dbo.Automobile", "LastCheckAllowedRepairDistance", c => c.DateTime());
            AddColumn("dbo.Automobile", "ReferralWorkshop", c => c.Int());
            AddColumn("dbo.Automobile", "MaxReferralWorkshop", c => c.Int());
            AddColumn("dbo.Automobile", "LastCheckReferralWorkshop", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Automobile", "LastCheckReferralWorkshop");
            DropColumn("dbo.Automobile", "MaxReferralWorkshop");
            DropColumn("dbo.Automobile", "ReferralWorkshop");
            DropColumn("dbo.Automobile", "LastCheckAllowedRepairDistance");
            DropColumn("dbo.Automobile", "MaxAllowedRepairDistance");
            DropColumn("dbo.Automobile", "AllowedRepairDistance");
            DropColumn("dbo.Automobile", "LastCheckAllowedDistance");
            DropColumn("dbo.Automobile", "MaxAllowedDistance");
            DropColumn("dbo.Automobile", "AllowedDistance");
        }
    }
}
