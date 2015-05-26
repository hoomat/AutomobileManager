using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Models
{
    public enum GroupModel
    {
        //مدیر کل
        DirectorGeneral = 1,
        //مدیر اداره
        OfficeManager = 2,
        //کاربر
        User = 3,
        //گزارش گیر
        StuckReport = 4
    }
}