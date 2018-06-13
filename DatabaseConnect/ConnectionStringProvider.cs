using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnect
{
    public static class ConnectionStringsProvider
    {
        public static string Get()
        {
            return ConfigurationManager.ConnectionStrings["StdConnection"].ConnectionString;
        }
    }
}
