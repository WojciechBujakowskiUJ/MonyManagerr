using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnect
{
    public class SqlQueryBuilder
    {
        public string Select { get; set; }
        public string From { get; set; }

        private IList<SqlWhere> _where;
        public IList<SqlWhere> Where 
        {
            get 
            {
                if (_where == null)
                {
                    _where = new List<SqlWhere>();
                }
                return _where;
            }
            set 
            {
                _where = value;
            }
        }

        public string GetSql
        {
            get
            {
                return Select + From + ( Where.Any() ? " WHERE " : "" ) + string.Join(" AND ", Where.Select(x => x.Where));
            }
        }

        public SqlParameter[] GetParams 
        {
            get
            {
                return Where.Select(x => x.Param).ToArray();
            }
        }

    }
}
