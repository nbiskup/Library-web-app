using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RwaLib.DAL
{
    public static class ConnectionString
    {
        public static string GetConnectionString() => ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
    }
}
