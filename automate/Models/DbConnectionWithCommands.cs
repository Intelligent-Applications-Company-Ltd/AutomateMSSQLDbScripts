using System;
using System.Collections.Generic;
using System.Text;

namespace automate.Models
{
    class DbConnectionWithCommands
    {
        public string DbConnection { get; set; }
        public string Log { get; set; }
        public string[] Commands { get; set; }
    }
}
