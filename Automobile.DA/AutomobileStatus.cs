using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DAL
{
    public class AutomobileStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Status { get; set; }

        public virtual ICollection<Automobile> Automobiles { get; set; }

    }
}