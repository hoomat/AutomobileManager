using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DAL
{

    [Table("Menu")]
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Class { get; set; }

        public string Style { get; set; }

        public virtual Menu Parent { get; set; }
    }
}