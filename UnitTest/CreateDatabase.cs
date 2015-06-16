using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest
{
    [TestClass]
    public class CreateDatabase
    {
        [TestMethod]
        public void CreateDatabaseTest()
        {
            using (var context = new ApplicationDbContext())
            {
                context.Database.CreateIfNotExists();

                DefineUserGroup();
                DefineAdministrator();
                DefineFualType();
                DefineAutomobileStatus();
                DefineTrafficCardType();
                DefinePartType();
                DefinePaymentType();
                DefineDriverRole();
                DefienDepartmentRole();
                DefineTrafficCardRole();
                DefineAutomobileRole();
                DefineTransitRole();
                DefineFuelRole();
                DefineIncidentRole();
                DefineRepairRole();
                DefineOilChangeRole();
                DefineUserRole();
                DefineSettingRole();
                DefienAutomobileClassRole();
                DefienColorRole();
                DefineAdmineGroupRoles();
                DefineOfficeManagerGroupRoles();
                DefineUserGroupRoles();
                DefineReporterGroupRoles();
            }
        }
        //1
        [TestMethod]
        public void DefineAdministrator()
        {
            var idManager = new IdentityManager();
            idManager.CreateRole("Dashboard-Menu", "نمایش منوی داشبورد");
            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "مدیر",
                LastName = "سیستم",
                Email = "admin@system.com",
                GroupId = 1
            };

            idManager.CreateUser(newUser, "password1");
            idManager.AddUserToRole(newUser.Id, "Dashboard-Menu");
        }
        //2
        [TestMethod]
        public void DefineFualType()
        {
            using (var context = new ApplicationDbContext())
            {
                if (context.FualTypes.FirstOrDefault(item => item.Value == "بنزین") == null)
                    context.FualTypes.Add(new FualType { Value = "بنزین" });
                if (context.FualTypes.FirstOrDefault(item => item.Value == "نفت و گاز") == null)
                    context.FualTypes.Add(new FualType { Value = "نفت و گاز" });
                if (context.FualTypes.FirstOrDefault(item => item.Value == "گاز") == null)
                    context.FualTypes.Add(new FualType { Value = "گاز" });
                if (context.FualTypes.FirstOrDefault(item => item.Value == "برق") == null)
                    context.FualTypes.Add(new FualType { Value = "برق" });

                context.SaveChanges();
            }
        }
        //3
        [TestMethod]
        public void DefineAutomobileStatus()
        {
            using (var context = new ApplicationDbContext())
            {
                if (context.AutomobileStatus.FirstOrDefault(item => item.Status == "موجود") == null)
                    context.AutomobileStatus.Add(new AutomobileStatus { Id = 1, Status = "موجود" });

                if (context.AutomobileStatus.FirstOrDefault(item => item.Status == "غیر عملیاتی") == null)
                    context.AutomobileStatus.Add(new AutomobileStatus { Id = 2, Status = "غیر عملیاتی" });

                if (context.AutomobileStatus.FirstOrDefault(item => item.Status == "در حال تعمیر") == null)
                    context.AutomobileStatus.Add(new AutomobileStatus { Id = 3, Status = "در حال تعمیر" });

                if (context.AutomobileStatus.FirstOrDefault(item => item.Status == "ماموریت") == null)
                    context.AutomobileStatus.Add(new AutomobileStatus { Id = 4, Status = "ماموریت" });

                if (context.AutomobileStatus.FirstOrDefault(item => item.Status == "کارواش") == null)
                    context.AutomobileStatus.Add(new AutomobileStatus { Id = 5, Status = "کارواش" });

                if (context.AutomobileStatus.FirstOrDefault(item => item.Status == "واگذار شده") == null)
                    context.AutomobileStatus.Add(new AutomobileStatus { Id = 6, Status = "واگذار شده" });
                context.SaveChanges();
            }
        }
        //4
        [TestMethod]
        void DefineTrafficCardType()
        {
            using (var context = new ApplicationDbContext())
            {
                if (context.TrafficCardTypes.FirstOrDefault(item => item.Value == "شناور") == null)
                    context.TrafficCardTypes.Add(new TrafficCardType { Value = "شناور" });
                if (context.TrafficCardTypes.FirstOrDefault(item => item.Value == "سالیانه") == null)
                    context.TrafficCardTypes.Add(new TrafficCardType { Value = "سالیانه" });
                context.SaveChanges();
            }
        }
        //5
        [TestMethod]
        public void DefinePartType()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (context.PartTypes.FirstOrDefault(item => item.Type == "مصرفی") == null)
                    context.PartTypes.Add(new PartType { Type = "مصرفی" });
                if (context.PartTypes.FirstOrDefault(item => item.Type == "تعویضی") == null)
                    context.PartTypes.Add(new PartType { Type = "تعویضی" });
                context.SaveChanges();
            }
        }
        //6
        [TestMethod]
        void DefinePaymentType()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (context.PaymentTypes.FirstOrDefault(item => item.Type == "کارت سوخت") == null)
                    context.PaymentTypes.Add(new PaymentType { Type = "کارت سوخت" });
                if (context.PaymentTypes.FirstOrDefault(item => item.Type == "آزاد") == null)
                    context.PaymentTypes.Add(new PaymentType { Type = "آزاد" });
                context.SaveChanges();
            }
        }
        //7
        [TestMethod]
        public bool DefineDriverRole()
        {
            bool success = false;
            var idManager = new IdentityManager();

            //Automobile roles
            success = idManager.CreateRole("Driver-Menu", "نمایش منوی راننده ها");
            success = idManager.CreateRole("Driver-Show", "نمایش راننده ها");
            success = idManager.CreateRole("Driver-New", "تعریف راننده ها");
            success = idManager.CreateRole("Driver-Edit", "بروزرسانی راننده ها");
            success = idManager.CreateRole("Driver-Delete", "حذف راننده ها");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            // success = idManager.CreateUser(newUser, "password1");

            //assign Automobile roles
            success = idManager.AddUserToRole(newUser.Id, "Driver-Menu");
            success = idManager.AddUserToRole(newUser.Id, "Driver-Show");
            success = idManager.AddUserToRole(newUser.Id, "Driver-New");
            success = idManager.AddUserToRole(newUser.Id, "Driver-Edit");
            success = idManager.AddUserToRole(newUser.Id, "Driver-Delete");

            return success;
        }
        //8
        [TestMethod]
        bool DefienDepartmentRole()
        {
            bool success = false;
            var idManager = new IdentityManager();

            //Automobile roles
            success = idManager.CreateRole("Department-Menu", "نمایش منوی اداره");
            success = idManager.CreateRole("Department-Show", "نمایش اداره");
            success = idManager.CreateRole("Department-New", "تعریف اداره");
            success = idManager.CreateRole("Department-Edit", "بروزرسانی اداره");
            success = idManager.CreateRole("Department-Delete", "حذف اداره");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            // success = idManager.CreateUser(newUser, "password1");

            //assign Automobile roles
            success = idManager.AddUserToRole(newUser.Id, "Department-Menu");
            success = idManager.AddUserToRole(newUser.Id, "Department-Show");
            success = idManager.AddUserToRole(newUser.Id, "Department-New");
            success = idManager.AddUserToRole(newUser.Id, "Department-Edit");
            success = idManager.AddUserToRole(newUser.Id, "Department-Delete");

            return success;
        }
        //9
        [TestMethod]
        bool DefineTrafficCardRole()
        {
            bool success = false;
            var idManager = new IdentityManager();

            //Automobile roles
            success = idManager.CreateRole("TrafficCard-Menu", "نمایش منوی کارت ترافیک");
            success = idManager.CreateRole("TrafficCard-Show", "نمایش  کارت ترافیک");
            success = idManager.CreateRole("TrafficCard-New", "تعریف  کارت ترافیک");
            success = idManager.CreateRole("TrafficCard-Edit", "بروزرسانی  کارت ترافیک");
            success = idManager.CreateRole("TrafficCard-Delete", "حذف  کارت ترافیک");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            // success = idManager.CreateUser(newUser, "password1");

            //assign Automobile roles
            success = idManager.AddUserToRole(newUser.Id, "TrafficCard-Menu");
            success = idManager.AddUserToRole(newUser.Id, "TrafficCard-Show");
            success = idManager.AddUserToRole(newUser.Id, "TrafficCard-New");
            success = idManager.AddUserToRole(newUser.Id, "TrafficCard-Edit");
            success = idManager.AddUserToRole(newUser.Id, "TrafficCard-Delete");

            return success;
        }
        //10
        [TestMethod]
        public void DefineAutomobileRole()
        {

            var idManager = new IdentityManager();

            //Automobile roles
            idManager.CreateRole("Automobile-Menu", "نمایش منوی خودرو");
            idManager.CreateRole("Automobile-Show", "نمایش خودرو");
            idManager.CreateRole("Automobile-New", "تعریف خودرو");
            idManager.CreateRole("Automobile-Edit", "بروزرسانی خودرو");
            idManager.CreateRole("Automobile-Delete", "حذف خودرو");
            idManager.CreateRole("Automobile-Report", "گزارش خودرو");
            idManager.CreateRole("Automobile-UnDelivery-Show", "نمایش  خودروهای حاضر");
            idManager.CreateRole("Automobile-Statistics-Analysis", "تحلیل آماری خودروها");
            idManager.CreateRole("Automobile-ChangeStatus", "تغییر وضعیت خودرو");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            // success = idManager.CreateUser(newUser, "password1");

            //assign Automobile roles
            //assign Automobile roles
            idManager.AddUserToRole(newUser.Id, "Automobile-Menu");
            idManager.AddUserToRole(newUser.Id, "Automobile-Show");
            idManager.AddUserToRole(newUser.Id, "Automobile-New");
            idManager.AddUserToRole(newUser.Id, "Automobile-Edit");
            idManager.AddUserToRole(newUser.Id, "Automobile-Delete");
            idManager.AddUserToRole(newUser.Id, "Automobile-Report");
            idManager.AddUserToRole(newUser.Id, "Automobile-UnDelivery-Show");
            idManager.AddUserToRole(newUser.Id, "Automobile-Statistics-Analysis");
            idManager.AddUserToRole(newUser.Id, "Automobile-ChangeStatus");


        }
      // 11
        [TestMethod]
        public void DefineTransitRole()
        {

            var idManager = new IdentityManager();

            //Transit roles
            idManager.CreateRole("Transit-Menu", "نمایش منوی تردد");
            idManager.CreateRole("Transit-Show", " نمایش تردد");
            idManager.CreateRole("Transit-Delivery", " ثبت تحویل خودرو");
            idManager.CreateRole("Transit-Return", " ثبت برگشت خودرو");
            idManager.CreateRole("Transit-Edit", " بروزرسانی تردد");
            idManager.CreateRole("Transit-Delete", " حذف تردد");
            idManager.CreateRole("Transit-Report", " گزارش تردد");
            idManager.CreateRole("Transit-UnReturn-Show", "نمایش  ترددهای بدون برگشت");


            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            // success = idManager.CreateUser(newUser, "password1");

            //assign Transit roles
            idManager.AddUserToRole(newUser.Id, "Transit-Menu");
            idManager.AddUserToRole(newUser.Id, "Transit-Show");
            idManager.AddUserToRole(newUser.Id, "Transit-Delivery");
            idManager.AddUserToRole(newUser.Id, "Transit-Return");
            idManager.AddUserToRole(newUser.Id, "Transit-Edit");
            idManager.AddUserToRole(newUser.Id, "Transit-Delete");
            idManager.AddUserToRole(newUser.Id, "Transit-Report");
            idManager.AddUserToRole(newUser.Id, "Transit-UnReturn-Show");


        }
        //12
        [TestMethod]
        bool DefineFuelRole()
        {
            bool success = false;
            var idManager = new IdentityManager();

            //Automobile roles

            idManager.CreateRole("Fuel-Menu", "نمایش منوی سوخت");
            idManager.CreateRole("Fuel-Show", "نمایش سوخت");
            idManager.CreateRole("Fuel-New", "تعریف سوخت");
            idManager.CreateRole("Fuel-Edit", "بروزرسانی سوخت");
            idManager.CreateRole("Fuel-Delete", "حذف سوخت");
            idManager.CreateRole("Fuel-Report", "گزارش سوخت");
            idManager.CreateRole("Fuel-ReportCompareFuel", "گزارش مقایسه سوخت");


            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };

            //assign Automobile roles
            idManager.AddUserToRole(newUser.Id, "Fuel-Menu");
            idManager.AddUserToRole(newUser.Id, "Fuel-Show");
            idManager.AddUserToRole(newUser.Id, "Fuel-New");
            idManager.AddUserToRole(newUser.Id, "Fuel-Edit");
            idManager.AddUserToRole(newUser.Id, "Fuel-Delete");
            idManager.AddUserToRole(newUser.Id, "Fuel-Report");
            idManager.AddUserToRole(newUser.Id, "Fuel-ReportCompareFuel");

            return success;
        }
        //13
        [TestMethod]
        public void DefineIncidentRole()
        {

            var idManager = new IdentityManager();

            idManager.CreateRole("Incident-Menu", "نمایش منوی تصادفات");
            idManager.CreateRole("Incident-Show", "نمایش تصادفات");
            idManager.CreateRole("Incident-New", "تعریف تصادفات");
            idManager.CreateRole("Incident-Edit", "بروزرسانی تصادفات");
            idManager.CreateRole("Incident-Delete", "حذف تصادفات");
            idManager.CreateRole("Incident-Report", "گزارش تصادفات");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            idManager.AddUserToRole(newUser.Id, "Incident-Menu");
            idManager.AddUserToRole(newUser.Id, "Incident-Show");
            idManager.AddUserToRole(newUser.Id, "Incident-New");
            idManager.AddUserToRole(newUser.Id, "Incident-Edit");
            idManager.AddUserToRole(newUser.Id, "Incident-Delete");
            idManager.AddUserToRole(newUser.Id, "Incident-Report");

        }
        //14
        [TestMethod]
        public void DefineRepairRole()
        {

            var idManager = new IdentityManager();


            //Repair role
            idManager.CreateRole("Repair-Menu", "نمایش منوی تعمیرات");
            idManager.CreateRole("Repair-Show", "نمایش تعمیرات");
            idManager.CreateRole("Repair-New", "تعریف تعمیرات");
            idManager.CreateRole("Repair-Edit", "بروزرسانی تعمیرات");
            idManager.CreateRole("Repair-Delete", "حذف تعمیرات");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            //assign Repair roles
            idManager.AddUserToRole(newUser.Id, "Repair-Menu");
            idManager.AddUserToRole(newUser.Id, "Repair-Show");
            idManager.AddUserToRole(newUser.Id, "Repair-New");
            idManager.AddUserToRole(newUser.Id, "Repair-Edit");
            idManager.AddUserToRole(newUser.Id, "Repair-Delete");

        }
        //15
        [TestMethod]
        public void DefineOilChangeRole()
        {
            bool success = false;
            var idManager = new IdentityManager();


            //OilChange role
            idManager.CreateRole("OilChange-Menu", "نمایش منوی تعویض روغن");
            idManager.CreateRole("OilChange-Show", "نمایش تعویض روغن");
            idManager.CreateRole("OilChange-New", "تعریف تعویض روغن");
            idManager.CreateRole("OilChange-Edit", "بروزرسانی تعویض روغن");
            idManager.CreateRole("OilChange-Delete", "حذف تعویض روغن");
            idManager.CreateRole("OilChange-Report", "گزارش تعویض روغن");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            //assign OilChange roles
            idManager.AddUserToRole(newUser.Id, "OilChange-Menu");
            idManager.AddUserToRole(newUser.Id, "OilChange-Show");
            idManager.AddUserToRole(newUser.Id, "OilChange-New");
            idManager.AddUserToRole(newUser.Id, "OilChange-Edit");
            idManager.AddUserToRole(newUser.Id, "OilChange-Delete");
            idManager.AddUserToRole(newUser.Id, "OilChange-Report");

        }
        //16
        [TestMethod]
        public void DefineUserRole()
        {

            var idManager = new IdentityManager();
            //User role
            idManager.CreateRole("User-Menu", "نمایش منوی کاربران");
            idManager.CreateRole("User-Register", "تعریف کاربران");
            idManager.CreateRole("User-Show", "نمایش کاربران");
            idManager.CreateRole("User-Edit", "بروزرسانی کاربران");
            idManager.CreateRole("User-Delete", "حذف کاربران");
            idManager.CreateRole("User-Roles", "مجوزدهی کاربران");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };

            //assign User roles
            idManager.AddUserToRole(newUser.Id, "User-Menu");
            idManager.AddUserToRole(newUser.Id, "User-Register");
            idManager.AddUserToRole(newUser.Id, "User-Show");
            idManager.AddUserToRole(newUser.Id, "User-Edit");
            idManager.AddUserToRole(newUser.Id, "User-Delete");
            idManager.AddUserToRole(newUser.Id, "User-Roles");

        }
        //17
        [TestMethod]
        public void DefineSettingRole()
        {

            var idManager = new IdentityManager();
            //User role
            idManager.CreateRole("Show-Setting", "نمایش تنظیمات");
            idManager.CreateRole("Save-Setting", "ذخیره تنظیمات");


            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };

            //assign User roles
            idManager.AddUserToRole(newUser.Id, "Show-Setting");
            idManager.AddUserToRole(newUser.Id, "Save-Setting");

        }
        //18
         [TestMethod]
        public void DefienAutomobileClassRole()
        {

            var idManager = new IdentityManager();

            //Automobile roles
            idManager.CreateRole("AutomobileClass-Menu", "نمایش منوی مدل خودرو");
            idManager.CreateRole("AutomobileClass-Show", "نمایش مدل خودرو");
            idManager.CreateRole("AutomobileClass-New", "تعریف مدل خودرو");
            idManager.CreateRole("AutomobileClass-Edit", "بروزرسانی مدل خودرو");
            idManager.CreateRole("AutomobileClass-Delete", "حذف مدل خودرو");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            // success = idManager.CreateUser(newUser, "password1");

            //assign Automobile roles
            idManager.AddUserToRole(newUser.Id, "AutomobileClass-Menu");
            idManager.AddUserToRole(newUser.Id, "AutomobileClass-Show");
            idManager.AddUserToRole(newUser.Id, "AutomobileClass-New");
            idManager.AddUserToRole(newUser.Id, "AutomobileClass-Edit");
            idManager.AddUserToRole(newUser.Id, "AutomobileClass-Delete");
        }
         //19
        [TestMethod]
        public void DefienColorRole()
        {

            var idManager = new IdentityManager();

            //Automobile roles
            idManager.CreateRole("Color-Menu", "نمایش منوی رنگ");
            idManager.CreateRole("Color-Show", "نمایش رنگ");
            idManager.CreateRole("Color-New", "تعریف رنگ");
            idManager.CreateRole("Color-Edit", "بروزرسانی رنگ");
            idManager.CreateRole("Color-Delete", "حذف رنگ");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            // success = idManager.CreateUser(newUser, "password1");

            //assign Automobile roles
            idManager.AddUserToRole(newUser.Id, "Color-Menu");
            idManager.AddUserToRole(newUser.Id, "Color-Show");
            idManager.AddUserToRole(newUser.Id, "Color-New");
            idManager.AddUserToRole(newUser.Id, "Color-Edit");
            idManager.AddUserToRole(newUser.Id, "Color-Delete");
        }
        //20
        [TestMethod]
        public void DefineUserGroup()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Groups.FirstOrDefault(item => item.Id == 1) == null)
                    db.Groups.Add(new Group { Id = 1, Name = "DirectorGeneral", Title = "مدیر کل" });
                if (db.Groups.FirstOrDefault(item => item.Id == 2) == null)
                    db.Groups.Add(new Group { Id = 2, Name = "OfficeManager", Title = "مدیر اداره" });
                if (db.Groups.FirstOrDefault(item => item.Id == 3) == null)
                    db.Groups.Add(new Group { Id = 3, Name = "User", Title = "کاربر" });
                if (db.Groups.FirstOrDefault(item => item.Id == 5) == null)
                    db.Groups.Add(new Group { Id = 4, Name = "StuckReport", Title = "گزارش گیر" });

                db.SaveChanges();
            }
        }
        //21
        [TestMethod]
        public void DefineAdmineGroupRoles()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var group = db.Groups.FirstOrDefault(item => item.Id == 1);
                foreach (var role in db.Roles.ToList())
                {
                    if (db.GroupRoles.FirstOrDefault(item => item.RoleId == role.Id && item.GroupId == group.Id) == null)
                        db.GroupRoles.Add(new GroupRole { GroupId = group.Id, RoleId = role.Id });
                }
                db.SaveChanges();
            }
        }
        //22
        [TestMethod]
        public void DefineOfficeManagerGroupRoles()
        {
            var roles = new List<string>();
            roles.Add("TrafficCard-Menu");
            roles.Add("TrafficCard-Show");
            roles.Add("TrafficCard-New");
            roles.Add("TrafficCard-Edit");
            roles.Add("TrafficCard-Delete");

            roles.Add("Automobile-Menu");
            roles.Add("Automobile-Show");
            roles.Add("Automobile-New");
            roles.Add("Automobile-Edit");
            roles.Add("Automobile-Delete");
            roles.Add("Automobile-Report");
            roles.Add("Automobile-UnDelivery-Show");
            roles.Add("Automobile-Statistics-Analysis");
          

            roles.Add("Transit-Menu");
            roles.Add("Transit-Show");
            roles.Add("Transit-Delivery");
            roles.Add("Transit-Return");
            roles.Add("Transit-Edit");
            roles.Add("Transit-Delete");
            roles.Add("Transit-Report");
            roles.Add("Transit-UnDelivery-Show");

            roles.Add("Fuel-Menu");
            roles.Add("Fuel-Show");
            roles.Add("Fuel-New");
            roles.Add("Fuel-Edit");
            roles.Add("Fuel-Delete");
            roles.Add("Fuel-Report");
            roles.Add("Fuel-ReportCompareFuel");

            roles.Add("Incident-Menu");
            roles.Add("Incident-Show");
            roles.Add("Incident-New");
            roles.Add("Incident-Edit");
            roles.Add("Incident-Delete");
            roles.Add("Incident-Report");

            roles.Add("Repair-Menu");
            roles.Add("Repair-Show");
            roles.Add("Repair-New");
            roles.Add("Repair-Edit");
            roles.Add("Repair-Delete");

            roles.Add("OilChange-Menu");
            roles.Add("OilChange-Show");
            roles.Add("OilChange-New");
            roles.Add("OilChange-Edit");
            roles.Add("OilChange-Delete");

            roles.Add("AutomobileClass-Menu");
            roles.Add("AutomobileClass-Show");
            roles.Add("AutomobileClass-New");
            roles.Add("AutomobileClass-Edit");
            roles.Add("AutomobileClass-Delete");

            roles.Add("Color-Menu");
            roles.Add("Color-Show");
            roles.Add("Color-New");
            roles.Add("Color-Edit");
            roles.Add("Color-Delete");
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var group = db.Groups.FirstOrDefault(item => item.Id == 2);
                foreach (var role in roles)
                {
                    var rolefind = db.Roles.FirstOrDefault(item => item.Name == role);

                    if (rolefind != null && db.GroupRoles.FirstOrDefault(item => item.RoleId == rolefind.Id && item.GroupId == group.Id) == null)
                        db.GroupRoles.Add(new GroupRole { GroupId = group.Id, RoleId = rolefind.Id });
                }
                db.SaveChanges();
            }
        }
        //23
        [TestMethod]
        public void DefineUserGroupRoles()
        {
            var roles = new List<string>();
            roles.Add("TrafficCard-Menu");
            roles.Add("TrafficCard-Show");
            roles.Add("TrafficCard-New");
            roles.Add("TrafficCard-Edit");
            roles.Add("TrafficCard-Delete");

            roles.Add("Automobile-Menu");
            roles.Add("Automobile-Show");
            roles.Add("Automobile-New");
            roles.Add("Automobile-Edit");
            roles.Add("Automobile-Delete");
            roles.Add("Automobile-Report");
            roles.Add("Automobile-UnDelivery-Show");
            roles.Add("Automobile-Statistics-Analysis");

            roles.Add("Transit-Menu");
            roles.Add("Transit-Show");
            roles.Add("Transit-Delivery");
            roles.Add("Transit-Return");
            roles.Add("Transit-Edit");
            roles.Add("Transit-Delete");
            roles.Add("Transit-Report");
            roles.Add("Transit-UnDelivery-Show");

            roles.Add("Fuel-Menu");
            roles.Add("Fuel-Show");
            roles.Add("Fuel-New");
            roles.Add("Fuel-Edit");
            roles.Add("Fuel-Delete");
            roles.Add("Fuel-Report");
            roles.Add("Fuel-ReportCompareFuel");

            roles.Add("Incident-Menu");
            roles.Add("Incident-Show");
            roles.Add("Incident-New");
            roles.Add("Incident-Edit");
            roles.Add("Incident-Delete");
            roles.Add("Incident-Report");

            roles.Add("Repair-Menu");
            roles.Add("Repair-Show");
            roles.Add("Repair-New");
            roles.Add("Repair-Edit");
            roles.Add("Repair-Delete");

            roles.Add("OilChange-Menu");
            roles.Add("OilChange-Show");
            roles.Add("OilChange-New");
            roles.Add("OilChange-Edit");
            roles.Add("OilChange-Delete");

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var group = db.Groups.FirstOrDefault(item => item.Id == 3);
                foreach (var role in roles)
                {
                    var rolefind = db.Roles.FirstOrDefault(item => item.Name == role);

                    if (rolefind != null && db.GroupRoles.FirstOrDefault(item => item.RoleId == rolefind.Id && item.GroupId == group.Id) == null)
                        db.GroupRoles.Add(new GroupRole { GroupId = group.Id, RoleId = rolefind.Id });
                }
                db.SaveChanges();
            }
        }
        //24
        [TestMethod]
        public void DefineReporterGroupRoles()
        {
            var roles = new List<string>();
            roles.Add("TrafficCard-Menu");
            roles.Add("TrafficCard-Show");


            roles.Add("Automobile-Menu");
            roles.Add("Automobile-Show");

            roles.Add("Automobile-Report");
            roles.Add("Automobile-UnDelivery-Show");
            roles.Add("Automobile-Statistics-Analysis");

            roles.Add("Transit-Menu");
            roles.Add("Transit-Show");

            roles.Add("Transit-Report");
            roles.Add("Transit-UnDelivery-Show");

            roles.Add("Fuel-Menu");
            roles.Add("Fuel-Show");

            roles.Add("Fuel-Report");
            roles.Add("Fuel-ReportCompareFuel");

            roles.Add("Incident-Menu");
            roles.Add("Incident-Show");

            roles.Add("Incident-Report");

            roles.Add("Repair-Menu");
            roles.Add("Repair-Show");


            roles.Add("OilChange-Menu");
            roles.Add("OilChange-Show");


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var group = db.Groups.FirstOrDefault(item => item.Id == 4);
                foreach (var role in roles)
                {
                    var rolefind = db.Roles.FirstOrDefault(item => item.Name == role);

                    if (rolefind != null && db.GroupRoles.FirstOrDefault(item => item.RoleId == rolefind.Id && item.GroupId == group.Id) == null)
                        db.GroupRoles.Add(new GroupRole { GroupId = group.Id, RoleId = rolefind.Id });
                }
                db.SaveChanges();
            }
        }
    }
}
