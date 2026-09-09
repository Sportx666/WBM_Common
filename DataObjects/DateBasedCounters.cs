using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace wbm_common.DataObjects
{
    public class DateBasedCounters
    {
        public class dbRow 
        {
            public int CounterDefnID { get; set; }

            public DateTime Date { get; set; }  

            public int Counter { get; set; }

            public DateTime LastUpdated { get; set; }
        }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
