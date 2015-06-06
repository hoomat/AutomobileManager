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
    [Table("TrafficCard")]
    public class TrafficCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        //شماره کارت
        [Required(ErrorMessage = "شماره کارت الزامیست")]
        public string NumberCard { get; set; }

        //نوع کارت 
        public string TrafficCardType { get; set; }

        //تاریخ هزینه
        [Required(ErrorMessage = "تاریخ هزینه الزامیست")]
        public DateTime DateBuy { get; set; }

        //کیلومتر خودرو قبل از تردد
        [Required(ErrorMessage = "خریدار")]
        public string Buyer { get; set; }


        //تاریخ انقضا
        [Required(ErrorMessage = "تاریخ انقضا الزامیست")]
        public DateTime DateExpire { get; set; }

        // هرینه
        [Required(ErrorMessage = "هزینه  الزامیست")]
        public int Cost { get; set; }

        // توضیحات      
        public string  Description { get; set; }

        public virtual IdentityUser IdentityUser { get; set; }
    }
}
