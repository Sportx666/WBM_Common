using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wbm_common.DataObjects
{
    public class Log : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public int ID { get; set; }

            public DateTime? CallTime_from { get; set; }
            public DateTime? CalllTime_to { get; set; }

            public string APIEndPoint { get; set; }

            public Boolean? Authenticatd { get; set; }

            public int UserID { get; set; }
            public string UserName { get; set; }
            public Boolean? CallSuccess { get; set; }

            public string CallingData { get; set; }

            public string ErrorText { get; set; }

            public string ReturnData { get; set; }

            public int CallLength { get; set; }

            public string APICallReference { get; set; }

            public string APITransactionReference { get; set; }
            public enum SearchModeType
            {
                AllRecords = 2
            }
            public SearchModeType SearchMode { get; set; } = SearchModeType.AllRecords;
        }

        public class dbRow
        {
            [Key]
            public int ID { get; set; }
            public DateTime CallTime { get; set; }

            public string APIEndPoint { get; set; }

            public Boolean Authenticated { get; set; }

            public int UserID { get; set; }
            public string UserName { get; set; }
            public Boolean CallSuccess { get; set; }

            public string CallingData { get; set; }

            public string ErrorText { get; set; }

            public string ReturnData { get; set; }

            public int CallLength { get; set; }

            public string APICallReference { get; set; }

            public string APITransactionReference { get; set; }

        }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
