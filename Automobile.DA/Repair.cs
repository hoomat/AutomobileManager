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
    [Table("Repair")]
    public class Repair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        //// نام رده اقدام کننده
        public virtual Department Department { get; set; }
        public int? DepartmentId { get; set; }


        // تاریخ و ساعت تعمیر
        [Required(ErrorMessage = "زمان تعمیر الزامی است")]
        public DateTime DateRepair { get; set; }

        // نام تعمیرگاه
        [Required(ErrorMessage = "نام تعمیرگاه الزامی است")]
        public string Workshop { get; set; }

        // شماره فاکتور تعمیرگاه
        [Required(ErrorMessage = "شماره فاکتور الزامی است")]
        public string InvoiceNo { get; set; }

        // هزینه کرد
        
        public int? Cost { get; set; }

        // هزینه کرد

        public int? Wage { get; set; }
        //نظریه میکانیک
        public string RepairmanDescription { get; set; }

        // شرح اقدامات انجام شده
        public string ActionDescription { get; set; }

        // توضیحات
        public string Description { get; set; }

        // کد کاربر ثبت کننده
        public virtual IdentityUser IdentityUser { get; set; }

        public virtual Automobile Automobile { get; set; }
        public int? AutomobileID { get; set; }

        //  نام راننده
        //[Required(ErrorMessage = "نام راننده الزامی است")]
        public virtual Driver Driver { get; set; }
        public int? DriverID { get; set; }

        //تعمیر 
        public virtual Incident Incident { get; set; }
        public int? IncidentID { get; set; }

        //قطعات مصرفی
        public virtual ICollection<ConsumablePart> ConsumableParts { get; set; }
        
    }

    [Table("ConsumablePart")]
    public class ConsumablePart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int Price { get; set; }

        public virtual Repair Repair { get; set; }

        public int RepairId { get; set; }
    }


    [Table("PartType")]
    public class PartType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
