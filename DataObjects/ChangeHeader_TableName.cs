using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace wbm_common.DataObjects
{
    public class ChangeHeader_TableName
    {
        public class Search : SearchCreatedLast_Base
        {
            public string TableName{ get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
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
                else
                {
                    // Validate each field as required

                    if (!BaseIsValid(out ListLocalValidationErrors))
                    {
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }
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
                sb.Append(@"SELECT * FROM ChangeHeaderTableName.*,
                                Security_User.PublicID as Created_UserPublicID
                            FROM ChangeHeaderTableName WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (!string.IsNullOrEmpty(TableName))
                {
                    sb.Append(" AND TableName like '%' + @TableName+ '%'");
                    ListSqlParameter.Add(new SqlParameter("@TableName", TableName));
                }

                if (BaseGenerateSqlQuery("DateBasedCounterDefn", out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                SqlQuery = sb.ToString();
            }

        }

        public dbRow dbRow_instance { get; set; }

        public class dbRow : dbRowCreatedLast_Base
        {
            public string TableName { get; set; }
            public int SortOrder { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }


        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
