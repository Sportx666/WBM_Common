using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace wbm_common.DataObjects
{
    public class APICallStats
    {
        public class dbRow 
        {
            public string APIEndPoint { get; set; }

            public DateTime Date { get; set; }  

            public int NumberOfCalls { get; set; }

            public int TotalCallLength { get; set; }
        }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
