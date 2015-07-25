namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FuelConsume", "FualTypeID", c => c.Int());
            CreateIndex("dbo.FuelConsume", "FualTypeID");
            AddForeignKey("dbo.FuelConsume", "FualTypeID", "dbo.FualTypes", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FuelConsume", "FualTypeID", "dbo.FualTypes");
            DropIndex("dbo.FuelConsume", new[] { "FualTypeID" });
            DropColumn("dbo.FuelConsume", "FualTypeID");
        }
    }
}
