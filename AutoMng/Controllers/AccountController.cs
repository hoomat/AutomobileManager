using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutomobilMng.Models;
using System;
using DAL;
using AutomobilMng.Log;

namespace AutomobilMng.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    var dic = LogAttribute.GetProperties<LoginViewModel>(model, ((int)Subject.UserLogin).ToString(), "success");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ورود به سیستم", null, dic.ToArray());
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    var dic = LogAttribute.GetProperties<LoginViewModel>(model, ((int)Subject.UserLogin).ToString(), "fail");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ورود به سیستم", null, dic.ToArray());
                    ModelState.AddModelError("", @AVAResource.Resource.Invalid_username_or_password);
                }
            }
            return View(model);
        }


        [Authorize(Roles = "User-Register")]
        public ActionResult Register()
        {
            return PartialView("Register",new RegisterViewModel() );
        }


        [HttpPost]
        [Authorize(Roles = "User-Register")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.GetUser();
                var result = await UserManager.CreateAsync(user, model.Password);
                List<string> roles = new List<string>();
                using (var db = new ApplicationDbContext())
                    roles = db.GroupRoles.Where(item => item.GroupId == model.Group).Select(item => item.Role.Name).ToList();
              
                if (result.Succeeded)
                {
                    var dic = LogAttribute.GetProperties<RegisterViewModel>(model, ((int)Subject.UserRegister).ToString(), "success");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "تعریف کاربر", null, dic.ToArray());

                    foreach (var role in roles)
                        UserManager.AddToRole(user.Id, role);
                    var messageModel = new MessageModel { Code = 0, Message = "success" };
                    return PartialView("MessageHandle", messageModel);
                }
                else
                {
                    var dic = LogAttribute.GetProperties<RegisterViewModel>(model, ((int)Subject.UserRegister).ToString(), "fail");
                    Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ورود به سیستم", null, dic.ToArray());
                    var error = ((string[])(result.Errors))[0];
                    var messageModel = new MessageModel { Code = 1, Message = ((string[])(result.Errors))[0] };
                    return PartialView("MessageHandle", messageModel);
                }

            }

            return PartialView("Register", model);
            // If we got this far, something failed, redisplay form
           // return View(model);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            var dic = LogAttribute.GetProperties<System.Security.Principal.IPrincipal>(User, ((int)Subject.UserLogout).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "خروج", null, dic.ToArray());
            return RedirectToAction("Login", "Account");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }


        [Authorize(Roles = "User-Show")]
        public ActionResult Index()
        {
            var dic = LogAttribute.GetProperties<OilChange>(null, ((int)Subject.UserShow).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "نمایش کاربران", null, dic.ToArray());
            ViewBag.Menu = "Account";
            ViewBag.MenuShow = AVAResource.Resource.UserMngMenu;
            return View();
        }

        [Authorize(Roles = "User-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }
        public ActionResult GetUsers(JQueryDataTableParamModel param)
        {
            var Db = new ApplicationDbContext();
            IQueryable<ApplicationUser> users = Db.Users.AsQueryable();
            IEnumerable<ApplicationUser> filteredUsers;
            var usernameSearch = Convert.ToString(Request["usernameSearch"]);
            var firstnameSearch = Convert.ToString(Request["firstnameSearch"]);
            var lastnameSearch = Convert.ToString(Request["lastnameSearch"]);
            if (!string.IsNullOrWhiteSpace(usernameSearch))
                users = users.Where(ivar => ivar.UserName.Contains(usernameSearch));            
            if (!string.IsNullOrWhiteSpace(firstnameSearch))
                users = users.Where(ivar => ivar.FirstName.Contains(firstnameSearch));
            if (!string.IsNullOrWhiteSpace(lastnameSearch) )
                users = users.Where(ivar => ivar.LastName == lastnameSearch);
            filteredUsers = users.ToList();
            var bSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var bSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var bSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var bSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ApplicationUser, string> orderingFunction = (c => sortColumnIndex == 1 && bSortable_1 ? c.UserName :"");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredUsers = filteredUsers.OrderBy(orderingFunction);
            else
                filteredUsers = filteredUsers.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredUsers.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                             c.UserName,
                             c.FirstName,
                             c.LastName,
                             c.Email,
                            (c.Group!=null) ? c.Group.Title:"",
                             c.UserName,
                               c.UserName
            };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = Db.Users.Count(),
                iTotalDisplayRecords = filteredUsers.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowSearchPartial()
        {
            return PartialView("_SearchPartial");
        }

        [Authorize(Roles = "User-Edit")]
        public ActionResult Edit(string id, ManageMessageId? Message = null)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            ViewBag.MessageId = Message;

            return PartialView("Edit", model);
        }
        
        [HttpPost]
        [Authorize(Roles = "User-Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var um = new UserManager<ApplicationUser>(
                 new UserStore<ApplicationUser>(new ApplicationDbContext()));
                
                var user = um.FindByName( model.UserName);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.GroupId = model.Group;
                user.DepartmentId = model.Department;
                var idManager = new IdentityManager();
                idManager.ClearUserRoles(user.Id);
               List<string> roles = new List<string>();
                using (var db = new ApplicationDbContext())
                {
                    roles= db.GroupRoles.Where(item => item.GroupId == model.Group).Select(item=>item.Role.Name).ToList();
                 
                }
                foreach (var role in roles)
                {
                    var idResult = um.AddToRole(user.Id, role);
                    //idManager.AddUserToRole(user.Id, grouprole.Role.Name);
                }
                um.Update(user);
                var dic = LogAttribute.GetProperties<EditUserViewModel>(model, ((int)Subject.UserEdit).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی کاربر", null, dic.ToArray());
                var messageModel = new MessageModel { Code = 0, Message = "success" };
                return PartialView("MessageHandle", messageModel);
                //return RedirectToAction("Index");
            }
            var dicfail = LogAttribute.GetProperties<EditUserViewModel>(model, ((int)Subject.UserEdit).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "بروزرسانی کاربر", null, dicfail.ToArray());
            return PartialView("Edit", model);
        }


        [Authorize(Roles = "User-Delete")]
        public ActionResult Delete(string id = null)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User-Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            Db.Users.Remove(user);
            Db.SaveChanges();
            var dic = LogAttribute.GetProperties<ApplicationUser>(user, ((int)Subject.UserDelete).ToString(), "success");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف کاربر", null, dic.ToArray());
            var messageModel = new MessageModel { Code = 0, Message = "success" };
            return PartialView("MessageHandle", messageModel);

            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            var dicfail = LogAttribute.GetProperties<ApplicationUser>(user, ((int)Subject.UserDelete).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "حذف کاربر", null, dicfail.ToArray());
            return PartialView("Edit", model);
        }


        [Authorize(Roles = "User-Roles")]
        public ActionResult UserRoles(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new SelectUserRolesViewModel(user);
            return PartialView("UserRoles", model);
        }


        [HttpPost]
        [Authorize(Roles = "User-Roles")]
        //[ValidateAntiForgeryToken]
        public ActionResult UserRoles(SelectUserRolesViewModel model )
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var Db = new ApplicationDbContext();
                var user = Db.Users.First(u => u.UserName == model.UserName);
                idManager.ClearUserRoles(user.Id);
                foreach (var role in model.Roles)
                    if (role.Selected)
                        idManager.AddUserToRole(user.Id, role.RoleName);
                var dic = LogAttribute.GetProperties<SelectUserRolesViewModel>(model, ((int)Subject.UserRoles).ToString(), "success");
                Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ثبت مجوز کاربر", null, dic.ToArray());

                var messageModel = new MessageModel { Code = 0, Message = "success" };
                return PartialView("MessageHandle", messageModel);
            }
            var dicfail = LogAttribute.GetProperties<SelectUserRolesViewModel>(model, ((int)Subject.UserRoles).ToString(), "fail");
            Logger.Send(GetType(), Logger.CriticalityLevel.Info, User.Identity.Name, "ثبت مجوز کاربر", null, dicfail.ToArray());
            return PartialView("UserRoles", model);
          
        }


        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }


        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}