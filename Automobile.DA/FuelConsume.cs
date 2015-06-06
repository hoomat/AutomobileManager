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
    [Table("FuelConsume")]
    public class FuelConsume
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }



        //-3 تاریخ و ساعت سوخت گیری
        [Required(ErrorMessage = "زمان سوخت گیری الزامی می باشد")]
        public DateTime RefuelDate { get; set; }

        //-4 میزان سوخت گیری )لیتر(
        [Required(ErrorMessage = "حجم سوخت گیری الزامی می باشد")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "*")]
        public int VolumeFuel { get; set; }

        //-5 مبلغ پرداخت شده
        [Required(ErrorMessage = "مبلغ سوخت گیری لازمی می باشد")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "*")]
        public int AmountPaid { get; set; }

        //-6 میزان مسافت طی شده قبل از سوخت گیری )کیلومتر(
       
        public int Distance { get; set; }

        //-9 نام جایگاه سوخت
        [Required]
        public string FualStation { get; set; }

        //-11 توضیحات
        public string Description { get; set; }

        //-11 کد کاربر ثبت کننده
        public virtual IdentityUser IdentityUser { get; set; }

        //-7 نحوه پرداخت )کارت سوخت / آزاد(
        public virtual PaymentType PaymentType { get; set; }
        public int? PaymentTypeID { get; set; }

        public virtual Automobile Automobile { get; set; }
        public int? AutomobileID { get; set; }
        // نام راننده
        public virtual Driver Driver { get; set; }
        public int? DriverID { get; set; }

        //-2 نام اداره تحویل گیرنده
        public virtual Department Department { get; set; }
        public int DepartmentID { get; set; }

        //-8 شماره کارت سوخت مصرفی
        public virtual FuelCard FuelCard { get; set; }
        public int? FuelCardID { get; set; }

        [Required(ErrorMessage = "مسافت قبل از سوخت گیری لازمی می باشد")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "*")]
        public int Mileag { get; set; }
    }

      [Table("PaymentType")]
    public class PaymentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
