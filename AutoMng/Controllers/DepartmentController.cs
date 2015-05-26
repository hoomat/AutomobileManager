﻿using AutomobilMng.Models;
using DAL;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Controllers
{
    public class DepartmentController : BaseController
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        [Authorize(Roles = "Department-Show")]
        public ActionResult Index()
        {
            ViewBag.MenuShow = AVAResource.Resource.Menu_Department;
            ViewBag.Menu = "Department";
            return View();
        }

        [Authorize(Roles = "Department-Show")]
        public ActionResult List()
        {
            return PartialView("List");
        }
        public ActionResult GetDepartments(JQueryDataTableParamModel param)
        {
            IQueryable<Department> departments = applicationDbContext.Departments.AsQueryable();
            IEnumerable<Department> filtered;
            var departmentNameSearch = Convert.ToString(Request["departmentNameSearch"]);
            

            if (!string.IsNullOrWhiteSpace(departmentNameSearch))
                departments = departments.Where(ivar => ivar.Name.Contains(departmentNameSearch));
           
            filtered = departments.ToList();
            var bSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var bSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var bSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var bSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Automobile, string> orderingFunction = (c => sortColumnIndex == 1 && bSortable_1 ? c.ID.ToString() : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(item => item.ID);
            else
                filtered = filtered.OrderByDescending(item => item.ID);

            var resultlist = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in resultlist
                         select new[] { 
                             c.Name,
                             c.ID.ToString()
                         };
                        
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = applicationDbContext.Departments.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Department-New")]
        public ActionResult New()
        {
            return PartialView("New", new Department());
        }

        [HttpPost]
        [Authorize(Roles = "Department-New")]
        [ValidateAntiForgeryToken]
        public ActionResult New(Department model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Departments.Add(model);
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        public ActionResult Search()
        {
            return PartialView("Search");
        }

        [Authorize(Roles = "Department-Edit")]
        public ActionResult Edit(int id)
        {
            var driver = applicationDbContext.Departments.First(item => item.ID == id);

            return PartialView("Edit", driver);
        }

        [HttpPost]
        [Authorize(Roles = "Department-Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department model)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                applicationDbContext.SaveChanges();
                return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
        }

        [Authorize(Roles = "Department-Delete")]
        public ActionResult Delete(int id)
        {
            var department = applicationDbContext.Departments.First(u => u.ID == id);
            if (department == null)
                return HttpNotFound();
            return PartialView("Delete", department);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Department-Delete")]
        public ActionResult DeleteConfirmed(Driver model)
        {
            var department = applicationDbContext.Departments.First(u => u.ID == model.ID);
            applicationDbContext.Departments.Remove(department);
            applicationDbContext.SaveChanges();
            return Json(new { success = true, description = @AVAResource.Resource.SuccessMessage });
            if (department == null)
            {
                return HttpNotFound();
            }
            return Json(new { success = false, description = @AVAResource.Resource.WarningMessage });
           
        }
    }
}