using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{
    public class SettingModel
    {
         [Required(ErrorMessage = "*")]
        public string TitleLogin { get; set; }

           [Required(ErrorMessage = "*")]
        public string TittleTab { get; set; }

        public string BackgoundName { get; set; }

    }
}