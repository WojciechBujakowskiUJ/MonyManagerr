using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnect
{
    public class SqlWhere
    {
        public string Where { get; set; }
        public SqlParameter Param { get; set; }
    }
}
