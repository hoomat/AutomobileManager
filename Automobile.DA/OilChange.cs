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
    [Table("OilChange")]
    public class OilChange
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        //تاریخ تعویض روغن
        [Required(ErrorMessage = "زمان تعویض الزامی است")]
        public DateTime ChangeDate { get; set; }

        //کیلومتر بعدی تعویض
        [Required(ErrorMessage = "کیلومتر بعدی تعویض الزامی است")]
        public string NextChangeDest { get; set; }

        //تعمیرگاه
          [Required(ErrorMessage = "تعمیرگاه تعویض الزامی است ")]
        public string Workshop { get; set; }

        //نوع روغن
        [Required(ErrorMessage = "نوع روغن الزامی است")]
        public string TypeOil { get; set; }

        //تعویض فیلتر روغن
        [Required]
        public bool OilFilterChanged { get; set; }

        //تعویض فیلتر هوا
        [Required]
        public bool AirFilterChanged { get; set; }

        //کاربر ثبت کننده
        public virtual IdentityUser IdentityUser { get; set; }

        //خودرو
        public virtual Automobile Automobile { get; set; }
        public int? AutomobileID { get; set; }

        // نام راننده
        public virtual Driver Driver { get; set; }
        public int? DriverID { get; set; }

        ////واحد اقدام کننده
        public virtual Department Department { get; set; }
        public int? DepartmentID { get; set; }

        //توضیحات
        public string Description { get; set; }

    }
}
