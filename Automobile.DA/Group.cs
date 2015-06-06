using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        //نام گروه
        public string Name { get; set; }

        //عنوان گروه
        public string Title { get; set; }
    }
}
