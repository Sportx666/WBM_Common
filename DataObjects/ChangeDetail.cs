using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class ChangeDetail
    {
        public class Search
        {

        }


        public class dbRow
        {
            public int ID { get; set; }

            public int ChangeHeaderID { get; set; }

            public int ChangeHeaderTableID { get; set; }

            public int RecordID { get; set; }

            public string FieldName { get; set; }

            public string OldValue { get; set; }

            public string NewValue { get; set; }

            [NotMapped]
            public string ChangeHeaderTable_TableName { get; set; }

        }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }

    }
}
