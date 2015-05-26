using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Role : IdentityRole
    {
        public Role()
        { }
        public Role(string rolename, string title)
            : base(rolename)
        {
            Title = title;
        }
        public string Title { get; set; }

    }
}
