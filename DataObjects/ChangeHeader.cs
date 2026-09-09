using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class ChangeHeader
    {
        public class Search
        {

        }


        public class dbRow : dbRowCreatedLast_Base
        {
            public int ChangeHeaderTableID { get; set; }

            public int RecordID { get; set; }

            public DateTime ChangeTime { get; set; }

            [NotMapped]
            public string ChangeHeader_TableName { get; set; }

        }

        public class dbRowExtended : dbRow
        {
            public string? Created_UserPublicID { get; set; }
        }
    }
}
