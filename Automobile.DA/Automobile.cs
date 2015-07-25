using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace DAL
{
    
    [Table("Automobile")]
    public class Automobile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "شماره پلاک الزامی است")]
        [StringLength(250)]
        public string Plaque { get; set; }

        [Required(ErrorMessage = "شماره شاسی الزامی است")]
        [StringLength(250)]
        public string Chassis { get; set; }



        public string ProduceYear { get; set; }



        public string FualType { get; set; }

        public string FualConsume { get; set; }

        public string VolumeTank { get; set; }

        public DateTime DateBuy { get; set; }

        public int Price { get; set; }

        public int Distance { get; set; }

        public DateTime LastService { get; set; }

        public string ImageAddress { get; set; }

        public virtual AutomobileClass AutomobileClass { get; set; }
        public int? AutomobileClassId { get; set; }



        public virtual Color Color { get; set; }
        public int? ColorId { get; set; }

        public virtual Department Department { get; set; }
        public int DepartmentId { get; set; }


        public virtual TrafficCard TrafficCard { get; set; }
        public int? TrafficCardId { get; set; }

        public string FuelCard { get; set; }

         public string  Numberofengines { get; set; }
       

        public virtual IdentityUser IdentityUser { get; set; }

        public virtual ICollection<Transit> Transits { get; set; }

        public virtual AutomobileStatus AutomobileStatus { get; set; }
        public int? AutomobileStatusId { get; set; }


        public int? AllowedDistance { get; set; }
        public int? MaxAllowedDistance { get; set; }
        public DateTime? LastCheckAllowedDistance { get; set; }

        public int? AllowedRepairDistance { get; set; }
        public int? MaxAllowedRepairDistance { get; set; }
        public DateTime? LastCheckAllowedRepairDistance { get; set; }


        public int? ReferralWorkshop { get; set; }
        public int? MaxReferralWorkshop { get; set; }
        public DateTime? LastCheckReferralWorkshop { get; set; }
    }
}