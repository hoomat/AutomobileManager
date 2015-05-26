using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomobilMng.Models
{
    public class ConsumablePartModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

         [Required(ErrorMessage = "*")]
        public string Type { get; set; }

         [Required(ErrorMessage = "*")]
         [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "به صورت عدد وارد فرمایید")]
         public int Price { get; set; }

        public List<SelectListItem> Types { get; set; }

        public ConsumablePartModel()
        {
            Types = new List<SelectListItem>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var type in db.PartTypes)
                    Types.Add(new SelectListItem { Text = type.Type.ToString(), Value = type.ID.ToString() });
            }
        }
    }
}