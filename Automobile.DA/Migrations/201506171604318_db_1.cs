namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendance",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        TransitID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Transit", t => t.TransitID, cascadeDelete: true)
                .Index(t => t.TransitID);
            
            CreateTable(
                "dbo.Transit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeliveryDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(),
                        Description = c.String(),
                        Distance = c.Int(),
                        MileagAfterTrip = c.Int(),
                        AutomobileID = c.Int(nullable: false),
                        DriverID = c.Int(nullable: false),
                        TrafficCardID = c.Int(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Automobile", t => t.AutomobileID, cascadeDelete: true)
                .ForeignKey("dbo.Driver", t => t.DriverID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.IdentityUser_Id)
                .ForeignKey("dbo.TrafficCard", t => t.TrafficCardID)
                .Index(t => t.AutomobileID)
                .Index(t => t.DriverID)
                .Index(t => t.TrafficCardID)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.Automobile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Plaque = c.String(nullable: false, maxLength: 250),
                        Chassis = c.String(nullable: false, maxLength: 250),
                        ProduceYear = c.String(),
                        FualType = c.String(),
                        FualConsume = c.String(),
                        VolumeTank = c.String(),
                        DateBuy = c.DateTime(nullable: false),
                        Price = c.Int(nullable: false),
                        Distance = c.Int(nullable: false),
                        LastService = c.DateTime(nullable: false),
                        ImageAddress = c.String(),
                        AutomobileClassId = c.Int(),
                        ColorId = c.Int(),
                        DepartmentId = c.Int(nullable: false),
                        FuelCard = c.String(),
                        AutomobileStatusId = c.Int(),
                        AllowedDistance = c.Int(),
                        MaxAllowedDistance = c.Int(),
                        LastCheckAllowedDistance = c.DateTime(),
                        AllowedRepairDistance = c.Int(),
                        MaxAllowedRepairDistance = c.Int(),
                        LastCheckAllowedRepairDistance = c.DateTime(),
                        ReferralWorkshop = c.Int(),
                        MaxReferralWorkshop = c.Int(),
                        LastCheckReferralWorkshop = c.DateTime(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AutomobileClasses", t => t.AutomobileClassId)
                .ForeignKey("dbo.AutomobileStatus", t => t.AutomobileStatusId)
                .ForeignKey("dbo.Colors", t => t.ColorId)
                .ForeignKey("dbo.Department", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.IdentityUser_Id)
                .Index(t => t.AutomobileClassId)
                .Index(t => t.ColorId)
                .Index(t => t.DepartmentId)
                .Index(t => t.AutomobileStatusId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AutomobileClasses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Class = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AutomobileStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        GroupId = c.Int(),
                        DepartmentId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .Index(t => t.GroupId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Title = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Driver",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PersonalNumber = c.String(),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TrafficCard",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NumberCard = c.String(nullable: false),
                        TrafficCardType = c.String(),
                        DateBuy = c.DateTime(nullable: false),
                        Buyer = c.String(nullable: false),
                        DateExpire = c.DateTime(nullable: false),
                        Cost = c.Int(nullable: false),
                        Description = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.ConsumablePart",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                        RepairId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Repair", t => t.RepairId, cascadeDelete: true)
                .Index(t => t.RepairId);
            
            CreateTable(
                "dbo.Repair",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(),
                        DateRepair = c.DateTime(nullable: false),
                        DateReturnRepair = c.DateTime(),
                        Workshop = c.String(),
                        InvoiceNo = c.String(),
                        Cost = c.Int(),
                        Wage = c.Int(),
                        RepairmanDescription = c.String(),
                        ActionDescription = c.String(),
                        Description = c.String(),
                        AutomobileID = c.Int(),
                        DriverID = c.Int(),
                        IncidentID = c.Int(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Automobile", t => t.AutomobileID)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.Driver", t => t.DriverID)
                .ForeignKey("dbo.AspNetUsers", t => t.IdentityUser_Id)
                .ForeignKey("dbo.Incidents", t => t.IncidentID)
                .Index(t => t.DepartmentId)
                .Index(t => t.AutomobileID)
                .Index(t => t.DriverID)
                .Index(t => t.IncidentID)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IncidentDate = c.DateTime(nullable: false),
                        DriverID = c.Int(nullable: false),
                        AutomobileID = c.Int(),
                        Description = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Automobile", t => t.AutomobileID)
                .ForeignKey("dbo.Driver", t => t.DriverID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.IdentityUser_Id)
                .Index(t => t.DriverID)
                .Index(t => t.AutomobileID)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.Damage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IncidentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Incidents", t => t.IncidentID, cascadeDelete: true)
                .Index(t => t.IncidentID);
            
            CreateTable(
                "dbo.FualTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FuelCards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FuelConsume",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RefuelDate = c.DateTime(nullable: false),
                        VolumeFuel = c.Int(nullable: false),
                        AmountPaid = c.Int(nullable: false),
                        Distance = c.Int(nullable: false),
                        FualStation = c.String(nullable: false),
                        Description = c.String(),
                        PaymentTypeID = c.Int(),
                        AutomobileID = c.Int(),
                        DriverID = c.Int(),
                        DepartmentID = c.Int(nullable: false),
                        FuelCardID = c.Int(),
                        Mileag = c.Int(nullable: false),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Automobile", t => t.AutomobileID)
                .ForeignKey("dbo.Department", t => t.DepartmentID, cascadeDelete: true)
                .ForeignKey("dbo.Driver", t => t.DriverID)
                .ForeignKey("dbo.FuelCards", t => t.FuelCardID)
                .ForeignKey("dbo.AspNetUsers", t => t.IdentityUser_Id)
                .ForeignKey("dbo.PaymentType", t => t.PaymentTypeID)
                .Index(t => t.PaymentTypeID)
                .Index(t => t.AutomobileID)
                .Index(t => t.DriverID)
                .Index(t => t.DepartmentID)
                .Index(t => t.FuelCardID)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.PaymentType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GroupRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.LogDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LogId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Logs", t => t.LogId, cascadeDelete: true)
                .Index(t => t.LogId);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Level = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Username = c.String(),
                        Message = c.String(),
                        ObjectId = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OilChange",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ChangeDate = c.DateTime(nullable: false),
                        NextChangeDest = c.String(nullable: false),
                        Workshop = c.String(nullable: false),
                        TypeOil = c.String(nullable: false),
                        OilFilterChanged = c.Boolean(nullable: false),
                        AirFilterChanged = c.Boolean(nullable: false),
                        AutomobileID = c.Int(),
                        DriverID = c.Int(),
                        DepartmentID = c.Int(),
                        Description = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Automobile", t => t.AutomobileID)
                .ForeignKey("dbo.Department", t => t.DepartmentID)
                .ForeignKey("dbo.Driver", t => t.DriverID)
                .ForeignKey("dbo.AspNetUsers", t => t.IdentityUser_Id)
                .Index(t => t.AutomobileID)
                .Index(t => t.DriverID)
                .Index(t => t.DepartmentID)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.PartType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TrafficCardType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OilChange", "IdentityUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OilChange", "DriverID", "dbo.Driver");
            DropForeignKey("dbo.OilChange", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.OilChange", "AutomobileID", "dbo.Automobile");
            DropForeignKey("dbo.LogDetails", "LogId", "dbo.Logs");
            DropForeignKey("dbo.GroupRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.GroupRoles", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.FuelConsume", "PaymentTypeID", "dbo.PaymentType");
            DropForeignKey("dbo.FuelConsume", "IdentityUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FuelConsume", "FuelCardID", "dbo.FuelCards");
            DropForeignKey("dbo.FuelConsume", "DriverID", "dbo.Driver");
            DropForeignKey("dbo.FuelConsume", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.FuelConsume", "AutomobileID", "dbo.Automobile");
            DropForeignKey("dbo.Repair", "IncidentID", "dbo.Incidents");
            DropForeignKey("dbo.Incidents", "IdentityUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incidents", "DriverID", "dbo.Driver");
            DropForeignKey("dbo.Damage", "IncidentID", "dbo.Incidents");
            DropForeignKey("dbo.Incidents", "AutomobileID", "dbo.Automobile");
            DropForeignKey("dbo.Repair", "IdentityUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Repair", "DriverID", "dbo.Driver");
            DropForeignKey("dbo.Repair", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.ConsumablePart", "RepairId", "dbo.Repair");
            DropForeignKey("dbo.Repair", "AutomobileID", "dbo.Automobile");
            DropForeignKey("dbo.Attendance", "TransitID", "dbo.Transit");
            DropForeignKey("dbo.Transit", "TrafficCardID", "dbo.TrafficCard");
            DropForeignKey("dbo.TrafficCard", "IdentityUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transit", "IdentityUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transit", "DriverID", "dbo.Driver");
            DropForeignKey("dbo.Transit", "AutomobileID", "dbo.Automobile");
            DropForeignKey("dbo.Automobile", "IdentityUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Automobile", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.Automobile", "ColorId", "dbo.Colors");
            DropForeignKey("dbo.Automobile", "AutomobileStatusId", "dbo.AutomobileStatus");
            DropForeignKey("dbo.Automobile", "AutomobileClassId", "dbo.AutomobileClasses");
            DropIndex("dbo.OilChange", new[] { "IdentityUser_Id" });
            DropIndex("dbo.OilChange", new[] { "DepartmentID" });
            DropIndex("dbo.OilChange", new[] { "DriverID" });
            DropIndex("dbo.OilChange", new[] { "AutomobileID" });
            DropIndex("dbo.LogDetails", new[] { "LogId" });
            DropIndex("dbo.GroupRoles", new[] { "RoleId" });
            DropIndex("dbo.GroupRoles", new[] { "GroupId" });
            DropIndex("dbo.FuelConsume", new[] { "IdentityUser_Id" });
            DropIndex("dbo.FuelConsume", new[] { "FuelCardID" });
            DropIndex("dbo.FuelConsume", new[] { "DepartmentID" });
            DropIndex("dbo.FuelConsume", new[] { "DriverID" });
            DropIndex("dbo.FuelConsume", new[] { "AutomobileID" });
            DropIndex("dbo.FuelConsume", new[] { "PaymentTypeID" });
            DropIndex("dbo.Damage", new[] { "IncidentID" });
            DropIndex("dbo.Incidents", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Incidents", new[] { "AutomobileID" });
            DropIndex("dbo.Incidents", new[] { "DriverID" });
            DropIndex("dbo.Repair", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Repair", new[] { "IncidentID" });
            DropIndex("dbo.Repair", new[] { "DriverID" });
            DropIndex("dbo.Repair", new[] { "AutomobileID" });
            DropIndex("dbo.Repair", new[] { "DepartmentId" });
            DropIndex("dbo.ConsumablePart", new[] { "RepairId" });
            DropIndex("dbo.TrafficCard", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentId" });
            DropIndex("dbo.AspNetUsers", new[] { "GroupId" });
            DropIndex("dbo.Automobile", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Automobile", new[] { "AutomobileStatusId" });
            DropIndex("dbo.Automobile", new[] { "DepartmentId" });
            DropIndex("dbo.Automobile", new[] { "ColorId" });
            DropIndex("dbo.Automobile", new[] { "AutomobileClassId" });
            DropIndex("dbo.Transit", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Transit", new[] { "TrafficCardID" });
            DropIndex("dbo.Transit", new[] { "DriverID" });
            DropIndex("dbo.Transit", new[] { "AutomobileID" });
            DropIndex("dbo.Attendance", new[] { "TransitID" });
            DropTable("dbo.TrafficCardType");
            DropTable("dbo.PartType");
            DropTable("dbo.OilChange");
            DropTable("dbo.Logs");
            DropTable("dbo.LogDetails");
            DropTable("dbo.GroupRoles");
            DropTable("dbo.PaymentType");
            DropTable("dbo.FuelConsume");
            DropTable("dbo.FuelCards");
            DropTable("dbo.FualTypes");
            DropTable("dbo.Damage");
            DropTable("dbo.Incidents");
            DropTable("dbo.Repair");
            DropTable("dbo.ConsumablePart");
            DropTable("dbo.TrafficCard");
            DropTable("dbo.Driver");
            DropTable("dbo.Groups");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Department");
            DropTable("dbo.Colors");
            DropTable("dbo.AutomobileStatus");
            DropTable("dbo.AutomobileClasses");
            DropTable("dbo.Automobile");
            DropTable("dbo.Transit");
            DropTable("dbo.Attendance");
        }
    }
}
