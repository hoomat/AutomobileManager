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
    public class Incident
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        //تاریخ وقوع حادثه  
        [Required(ErrorMessage = "تاریخ وقوع حادثه")]
        public DateTime IncidentDate { get; set; }

        //نام مقصر
        public virtual Driver Driver { get; set; }
        public int DriverID { get; set; }

        //خودرو
        public virtual Automobile Automobile { get; set; }
        public int? AutomobileID { get; set; }

        public virtual IdentityUser IdentityUser { get; set; }
        //شرح حادثه
        public string Description { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; }

        //  صدمات وارده
        public virtual ICollection<Damage> Damages { get; set; }
    }

    [Table("Damage")]
    public class Damage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Incident Incident { get; set; }

        public int IncidentID { get; set; }
    }
}
