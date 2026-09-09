using Microsoft.Data.SqlClient;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class LongRunProcessStatus
    {
        public class Search 
        {
            public int ID { get; set; }

            public int LongRunProcessHeaderID { get; set; }

            public DateTime StatusDateTime { get; set; }

            public string StatusDescription { get; set; }

            public List<Tuple<string, string>> ValidationErrors { get; set; }
            public List<Tuple<string, string>> ListLocalValidationErrors = new List<Tuple<string, string>>();

            public string SqlQuery { get; set; }

            public List<SqlParameter> ListSqlParameter { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
                LongRunProcessHeaderID = 3
            }

            public SearchModeType SearchMode { get; set; } = SearchModeType.Undefined;

            public System.Boolean IsValid()
            {
                ValidationErrors = new List<Tuple<string, string>>();

                System.Boolean rtnVal = true; // assume valid
                if (SearchMode == SearchModeType.AllRecords)
                {
                    // all records - no mandatory fields
                }
                else if (SearchMode == SearchModeType.SingleID)
                {
                    if (ID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ID", "Single ID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.LongRunProcessHeaderID)
                {
                    if (LongRunProcessHeaderID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("LongRunProcessHeaderID", "LongRunProcessHeaderID must be specified."));
                    }
                }
                else
                {
                    // Validate each field as required

                }

                if (ValidationErrors.Count > 0)
                {
                    rtnVal = false;
                }

                return rtnVal;
            }

            public void GenerateSqlQuery()
            {
                ListSqlParameter = new List<SqlParameter>();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM LongRunProcessStatus WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (LongRunProcessHeaderID > 0)
                {
                    sb.Append(" AND LongRunProcessHeaderID = @LongRunProcessHeaderID");
                    ListSqlParameter.Add(new SqlParameter("@LongRunProcessHeaderID", LongRunProcessHeaderID));
                }

                if (!string.IsNullOrEmpty(StatusDescription))
                {
                    sb.Append(" AND StatusDescription like '%' + @StatusDescription + '%'");
                    ListSqlParameter.Add(new SqlParameter("@StatusDescription", StatusDescription));
                }

                SqlQuery = sb.ToString();
            }

        }

        public class dbRow 
        {
            public int ID { get; set; }

            public int LongRunProcessHeaderID { get; set; }

            public DateTime StatusDateTime { get; set; }

            public string StatusDescription { get; set; }
        }

        public dbRow dbRow_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
