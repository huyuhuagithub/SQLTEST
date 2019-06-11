using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLTEST
{
    class users:BaseModel
    {
        //public int id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public bool Permission { get; set; }

    }
}
