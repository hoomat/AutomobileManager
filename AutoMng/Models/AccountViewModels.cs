using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using DAL;

namespace AutomobilMng.Models
{
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage =
            "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage =
            "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class UserTypeViewModel
    {
        public int Type { get; set; }
        public string Title { get; set; }
    }

    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            Groups = new List<System.Web.Mvc.SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var group in db.Groups)
                    Groups.Add(new System.Web.Mvc.SelectListItem { Text = group.Title, Value = group.Id.ToString() });
            }
            Departments = new List<System.Web.Mvc.SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var department in db.Departments)
                    Departments.Add(new System.Web.Mvc.SelectListItem { Text = department.Name, Value = department.ID.ToString() });
            }
        }

        [Required(ErrorMessage = "*")]
        //[Display(Name = "*")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage =
            "حداقل کارکتر 6 است.", MinimumLength = 6)]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [DataType(DataType.Password)]

        [Compare("Password", ErrorMessage =
            "کلمه عبور با تایید کلمه عبور برابر نیست")]
        public string ConfirmPassword { get; set; }

        // New Fields added to extend Application User class:

        [Required(ErrorMessage = "*")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        public int Use { get; set; }

        public int Group { get; set; }
        public List<System.Web.Mvc.SelectListItem> Groups { get; set; }

        public int Department { get; set; }
        public List<System.Web.Mvc.SelectListItem> Departments { get; set; }

        // Return a pre-poulated instance of AppliationUser:
        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                GroupId= this.Group,
                DepartmentId=this.Department
            };
            return user;
        }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        public EditUserViewModel(ApplicationUser user)
        {
            Groups = new List<System.Web.Mvc.SelectListItem>();
            if (user.Group != null)
            {
                Groups.Add(new System.Web.Mvc.SelectListItem { Text = user.Group.Title, Value = user.Group.Id.ToString() });
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    foreach (var group in db.Groups.Where(item => item.Id != user.GroupId))
                        Groups.Add(new System.Web.Mvc.SelectListItem { Text = group.Title, Value = group.Id.ToString() });
                }
                this.Group = user.GroupId.Value;
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    foreach (var group in db.Groups)
                        Groups.Add(new System.Web.Mvc.SelectListItem { Text = group.Title, Value = group.Id.ToString() });
                }
            }
            Departments = new List<System.Web.Mvc.SelectListItem>();
            if (user.Department != null)
            {
                Groups.Add(new System.Web.Mvc.SelectListItem { Text = user.Department.Name, Value = user.Department.ID.ToString() });
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    foreach (var department in db.Departments.Where(item => item.ID != user.DepartmentId))
                        Departments.Add(new System.Web.Mvc.SelectListItem { Text = department.Name, Value = department.ID.ToString() });
                }
                this.Department = user.DepartmentId.Value;
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    foreach (var department in db.Departments)
                        Departments.Add(new System.Web.Mvc.SelectListItem { Text = department.Name, Value = department.ID.ToString() });
                }
            }
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
           
        }

        [Required(ErrorMessage = "*")]

        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        public string Email { get; set; }

        public int Group { get; set; }
        public List<System.Web.Mvc.SelectListItem> Groups { get; set; }

        public int Department { get; set; }
        public List<System.Web.Mvc.SelectListItem> Departments { get; set; }
    }

    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }


        // Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            //this.RootRoles = new List<RootRoleEditorViewModel>();
            var Db = new ApplicationDbContext();
            //var rvm = new RootRoleEditorViewModel {Roles=new List<SelectRoleEditorViewModel>(), RoleRootName = @AVAResource.Resource.UserMngMenu,RoleRootKey="User".ToLower() };
            //this.RootRoles.Add(rvm);
            //rvm = new RootRoleEditorViewModel { Roles = new List<SelectRoleEditorViewModel>(), RoleRootName = @AVAResource.Resource.AutomobileMngMenu, RoleRootKey = "Automobile".ToLower() };
            //this.RootRoles.Add(rvm);

            //rvm = new RootRoleEditorViewModel { Roles = new List<SelectRoleEditorViewModel>(), RoleRootName = @AVAResource.Resource.FuelMngMenu, RoleRootKey = "Fuel".ToLower() };
            //this.RootRoles.Add(rvm);
            //rvm = new RootRoleEditorViewModel { Roles = new List<SelectRoleEditorViewModel>(), RoleRootName = @AVAResource.Resource.OilChangeMngMenu, RoleRootKey = "Oil".ToLower() };
            //this.RootRoles.Add(rvm);

            //rvm = new RootRoleEditorViewModel { Roles = new List<SelectRoleEditorViewModel>(), RoleRootName = @AVAResource.Resource.RepairMngMenu, RoleRootKey = "Repair".ToLower() };
            //this.RootRoles.Add(rvm);
            //rvm = new RootRoleEditorViewModel { Roles = new List<SelectRoleEditorViewModel>(), RoleRootName = @AVAResource.Resource.TrafficCardMngMenu, RoleRootKey = "Traffic".ToLower() };
            //this.RootRoles.Add(rvm);
            //rvm = new RootRoleEditorViewModel { Roles = new List<SelectRoleEditorViewModel>(), RoleRootName = @AVAResource.Resource.TransitMngMenu, RoleRootKey = "Transit".ToLower() };
            //this.RootRoles.Add(rvm);



            var allRoles = Db.Roles;
            foreach (var role in allRoles)
            {
                //var root = RootRoles.FirstOrDefault(item => role.Name.ToLower().Contains(item.RoleRootKey));
                // if (root != null)
                {
                    var selectRoleEditorViewModel = new SelectRoleEditorViewModel { RoleName = role.Name,Title=role.Title };
                    Roles.Add(selectRoleEditorViewModel);
                }
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (var userRole in user.Roles)
            {
                //  foreach (var root in RootRoles)
                {
                    var checkUserRole =
                    Roles.Find(r => r.RoleName == userRole.Role.Name);
                    checkUserRole.Selected = true;
                }
            }
        }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public List<SelectRoleEditorViewModel> Roles { get; set; }


    }

    // Used to display a single role with a checkbox, within a list structure:
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }
        public SelectRoleEditorViewModel(IdentityRole role)
        {
            this.RoleName = role.Name;
        }

        public bool Selected { get; set; }


        [Required]
        public string RoleName { get; set; }

        public string Title { get; set; }
    }

    public class RootRoleEditorViewModel
    {
        public RootRoleEditorViewModel() { }
        public RootRoleEditorViewModel(IdentityRole role)
        {
            this.RoleRootName = role.Name;
        }

        public List<SelectRoleEditorViewModel> Roles { get; set; }
        public string RoleRootName { get; set; }
        public string RoleRootKey { get; set; }
    }
}