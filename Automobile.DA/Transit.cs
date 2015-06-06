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
    [Table("Transit")]
    public class Transit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        //تاریخ و ساعت تحویل خودرو
        [Required(ErrorMessage = "تاریخ و ساعت تحویل خودرو الزامی است")]
        public DateTime DeliveryDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        //توضیحات
        public string Description { get; set; }

        //مسافت طی شده    
     
        public int? Distance { get; set; }


        //کیلومتر خودرو بعد از تردد
          //[Required(ErrorMessage = "کیلومتر خودرو")]
          //[RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "*")]
        public int? MileagAfterTrip { get; set; }

        //لیست نام همراهان
        public virtual ICollection<Attendance> Attendances { get; set; }

        //کد کاربر ثبت کننده
        public virtual IdentityUser IdentityUser { get; set; }

         //[Required(ErrorMessage = "خودرو الزامی است")]
        public virtual Automobile Automobile { get; set; }
        public int AutomobileID { get; set; }

        // نام راننده
        public virtual Driver Driver { get; set; }
        public int DriverID { get; set; }

        //کارت ترافیک
        public virtual TrafficCard TrafficCard { get; set; }
        public int? TrafficCardID { get; set; }
    }

    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Transit Transit { get; set; }

        public int TransitID { get; set; }
    }


}
