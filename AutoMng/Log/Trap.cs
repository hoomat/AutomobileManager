using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomobilMng.Log
{
    public class Trap
    {
        public DateTime TimeSpan { get; set; }
        public string Logger { get; set; }
        public string Level { get; set; }
        public int Thread { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public List<TrapDetail> Properties { get; set; }
        public Trap()
        {
            Properties = new List<TrapDetail>();
        }
    }
    public class TrapDetail
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}