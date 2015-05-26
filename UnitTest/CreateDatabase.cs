using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            }
        }

        [TestMethod]
        public void AddDefaultsTest()
        {
            using (var context = new ApplicationDbContext())
            {
                //    context.Database.CreateIfNotExists();
                //   DefineAdministrator();
                //  DefineFualType(context);
                //  DefinePaymentType(context);
                //  DefineTrafficCardType(context);
                DefineDriverRole();
                DefienDepartmentRole();
                DefineTrafficCardRole();
                DefineTrafficCardType(context);
                DefineAutomobileRole();
                DefineTransitRole();
                DefineFuelRole();
                DefineIncidentRole();
                DefineRepairRole();
                DefineOilChangeRole();
                DefineUserRole();
                context.SaveChanges();
            }
        }

        [TestMethod]
        bool DefineAdministrator()
        {
            bool success = false;
            var idManager = new IdentityManager();
            success = idManager.CreateRole("Dashboard-Menu", "نمایش منوی داشبورد");
            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "مدیر",
                LastName = "سیستم",
                Email = "admin@system.com",
                GroupId =1 
            };
            success = idManager.CreateUser(newUser, "password1");
            success = idManager.AddUserToRole(newUser.Id, "Dashboard-Menu");
            return success;
        }

        [TestMethod]
        public void DefineFualType(ApplicationDbContext context)
        {
            context.FualTypes.Add(new FualType { Value = "بنزین" });
            context.FualTypes.Add(new FualType { Value = "نفت و گاز" });
            context.FualTypes.Add(new FualType { Value = "گاز" });
            context.FualTypes.Add(new FualType { Value = "برق" });
        }

        [TestMethod]
        void DefineTrafficCardType(ApplicationDbContext context)
        {
            context.TrafficCardTypes.Add(new TrafficCardType { Value = "معمولی" });
        }

        [TestMethod]
        public void DefinePartType()
        {
            using (ApplicationDbContext context=new ApplicationDbContext())
            {
                context.PartTypes.Add(new PartType { Type = "مصرفی" });
                context.PartTypes.Add(new PartType { Type = "تعویضی" });
                context.SaveChanges();
            }
        }

        [TestMethod]
        void DefinePaymentType(ApplicationDbContext context)
        {
            context.PaymentTypes.Add(new PaymentType { Type = "کارت سوخت" });
            context.PaymentTypes.Add(new PaymentType { Type = "آزاد" });
        }

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

        [TestMethod]
        bool DefineAutomobileRole()
        {
            bool success = false;
            var idManager = new IdentityManager();

            //Automobile roles
            success = idManager.CreateRole("Automobile-Menu", "نمایش منوی خودرو");
            success = idManager.CreateRole("Automobile-Show", "نمایش خودرو");
            success = idManager.CreateRole("Automobile-New", "تعریف خودرو");
            success = idManager.CreateRole("Automobile-Edit", "بروزرسانی خودرو");
            success = idManager.CreateRole("Automobile-Delete", "حذف خودرو");
            success = idManager.CreateRole("Automobile-Report", "گزارش خودرو");

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
            success = idManager.AddUserToRole(newUser.Id, "Automobile-Menu");
            success = idManager.AddUserToRole(newUser.Id, "Automobile-Show");
            success = idManager.AddUserToRole(newUser.Id, "Automobile-New");
            success = idManager.AddUserToRole(newUser.Id, "Automobile-Edit");
            success = idManager.AddUserToRole(newUser.Id, "Automobile-Delete");
            success = idManager.AddUserToRole(newUser.Id, "Automobile-Report");

            return success;
        }

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
             idManager.CreateRole("Transit-UnDelivery-Show", "نمایش  ترددهای بدون برگشت");


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
             idManager.AddUserToRole(newUser.Id, "Transit-UnDelivery-Show");

         
        }

        [TestMethod]
        bool DefineFuelRole()
        {
            bool success = false;
            var idManager = new IdentityManager();

            //Automobile roles

            success = idManager.CreateRole("Fuel-Menu", "نمایش منوی سوخت");
            success = idManager.CreateRole("Fuel-Show", "نمایش سوخت");
            success = idManager.CreateRole("Fuel-New", "تعریف سوخت");
            success = idManager.CreateRole("Fuel-Edit", "بروزرسانی سوخت");
            success = idManager.CreateRole("Fuel-Delete", "حذف سوخت");
            success = idManager.CreateRole("Fuel-Report", "گزارش سوخت");
            success = idManager.CreateRole("Fuel-ReportCompareFuel", "گزارش مقایسه سوخت");


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
            success = idManager.AddUserToRole(newUser.Id, "Fuel-Menu");
            success = idManager.AddUserToRole(newUser.Id, "Fuel-Show");
            success = idManager.AddUserToRole(newUser.Id, "Fuel-New");
            success = idManager.AddUserToRole(newUser.Id, "Fuel-Edit");
            success = idManager.AddUserToRole(newUser.Id, "Fuel-Delete");
            success = idManager.AddUserToRole(newUser.Id, "Fuel-Report");
            success = idManager.AddUserToRole(newUser.Id, "Fuel-ReportCompareFuel");

            return success;
        }

        [TestMethod]
        public void DefineIncidentRole()
        {
            bool success = false;
            var idManager = new IdentityManager();

            success = idManager.CreateRole("Incident-Menu", "نمایش منوی تصادفات");
            success = idManager.CreateRole("Incident-Show", "نمایش تصادفات");
            success = idManager.CreateRole("Incident-New", "تعریف تصادفات");
            success = idManager.CreateRole("Incident-Edit", "بروزرسانی تصادفات");
            success = idManager.CreateRole("Incident-Delete", "حذف تصادفات");
            success = idManager.CreateRole("Incident-Report", "گزارش تصادفات");

            var newUser = new ApplicationUser()
            {
                Id = "f0d11eca-c593-4816-8f2d-a1c8ddb350c1",
                UserName = "admin",
                FirstName = "John",
                LastName = "Atten",
                Email = "jatten@typecastexception.com",
                GroupId = 1
            };
            success = idManager.AddUserToRole(newUser.Id, "Incident-Menu");
            success = idManager.AddUserToRole(newUser.Id, "Incident-Show");
            success = idManager.AddUserToRole(newUser.Id, "Incident-New");
            success = idManager.AddUserToRole(newUser.Id, "Incident-Edit");
            success = idManager.AddUserToRole(newUser.Id, "Incident-Delete");
            success = idManager.AddUserToRole(newUser.Id, "Incident-Report");

        }


        [TestMethod]
        public void DefineRepairRole()
        {
            bool success = false;
            var idManager = new IdentityManager();


            //Repair role
            success = idManager.CreateRole("Repair-Menu", "نمایش منوی تعمیرات");
            success = idManager.CreateRole("Repair-Show", "نمایش تعمیرات");
            success = idManager.CreateRole("Repair-New", "تعریف تعمیرات");
            success = idManager.CreateRole("Repair-Edit", "بروزرسانی تعمیرات");
            success = idManager.CreateRole("Repair-Delete", "حذف تعمیرات");

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
            success = idManager.AddUserToRole(newUser.Id, "Repair-Menu");
            success = idManager.AddUserToRole(newUser.Id, "Repair-Show");
            success = idManager.AddUserToRole(newUser.Id, "Repair-New");
            success = idManager.AddUserToRole(newUser.Id, "Repair-Edit");
            success = idManager.AddUserToRole(newUser.Id, "Repair-Delete");

        }


        [TestMethod]
        public void DefineOilChangeRole()
        {
            bool success = false;
            var idManager = new IdentityManager();


            //OilChange role
            success = idManager.CreateRole("OilChange-Menu", "نمایش منوی تعویض روغن");
            success = idManager.CreateRole("OilChange-Show", "نمایش تعویض روغن");
            success = idManager.CreateRole("OilChange-New", "تعریف تعویض روغن");
            success = idManager.CreateRole("OilChange-Edit", "بروزرسانی تعویض روغن");
            success = idManager.CreateRole("OilChange-Delete", "حذف تعویض روغن");

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
            success = idManager.AddUserToRole(newUser.Id, "OilChange-Menu");
            success = idManager.AddUserToRole(newUser.Id, "OilChange-Show");
            success = idManager.AddUserToRole(newUser.Id, "OilChange-New");
            success = idManager.AddUserToRole(newUser.Id, "OilChange-Edit");
            success = idManager.AddUserToRole(newUser.Id, "OilChange-Delete");

        }

        [TestMethod]
        public void DefineUserRole()
        {
            bool success = false;
            var idManager = new IdentityManager();
            //User role
            success = idManager.CreateRole("User-Menu", "نمایش منوی کاربران");
            success = idManager.CreateRole("User-Register", "تعریف کاربران");
            success = idManager.CreateRole("User-Show", "نمایش کاربران");
            success = idManager.CreateRole("User-Edit", "بروزرسانی کاربران");
            success = idManager.CreateRole("User-Delete", "حذف کاربران");
            success = idManager.CreateRole("User-Roles", "مجوزدهی کاربران");

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
            success = idManager.AddUserToRole(newUser.Id, "User-Menu");
            success = idManager.AddUserToRole(newUser.Id, "User-Register");
            success = idManager.AddUserToRole(newUser.Id, "User-Show");
            success = idManager.AddUserToRole(newUser.Id, "User-Edit");
            success = idManager.AddUserToRole(newUser.Id, "User-Delete");
            success = idManager.AddUserToRole(newUser.Id, "User-Roles");

        }

        [TestMethod]
        public void DefineSettingRole()
        {
            bool success = false;
            var idManager = new IdentityManager();
            //User role
            success = idManager.CreateRole("Show-Setting", "نمایش تنظیمات");
            success = idManager.CreateRole("Save-Setting", "ذخیره تنظیمات");


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
            success = idManager.AddUserToRole(newUser.Id, "Show-Setting");
            success = idManager.AddUserToRole(newUser.Id, "Save-Setting");
 
        }

        [TestMethod]
        public void DefineUserGroup()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Groups.Add(new Group { Id = 1, Name = "DirectorGeneral", Title = "مدیر کل" });
                db.Groups.Add(new Group { Id = 2, Name = "OfficeManager", Title = "مدیر اداره" });
                db.Groups.Add(new Group { Id = 3, Name = "User", Title = "کاربر" });
                db.Groups.Add(new Group { Id = 4, Name = "StuckReport", Title = "گزارش گیر" });

                db.SaveChanges();
            }
        }

        [TestMethod]
        public void DefineAdmineGroupRoles()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var group=db.Groups.FirstOrDefault(item=>item.Id==1);
                foreach (var role in db.Roles)
                {
                    db.GroupRoles.Add(new GroupRole { GroupId = group.Id, RoleId = role.Id });
                }
                db.SaveChanges();
            }
        }
    }
}
