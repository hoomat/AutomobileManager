using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string  Level { get; set; }

        public DateTime DateTime { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public int? ObjectId { get; set; }

        public virtual ICollection<LogDetail> LogDetails { get; set; }
    }

    public class LogDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Log Log { get; set; }

        public int LogId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
