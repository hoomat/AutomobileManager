using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    [Table("Driver")]
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        //نام راننده
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        //شماره پرسنلی
        public string PersonalNumber { get; set; }

        //رده
        public string Category { get; set; }
    }
}
