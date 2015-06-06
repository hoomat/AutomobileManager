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
    [Table("Department")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        //نام اداره
        [Required(ErrorMessage = "نام واحد الزامی است")]
        public string Name { get; set; }

        //لیست نام همراهان
        //public virtual ICollection<Transit> Transit { get; set; }

    }
}
