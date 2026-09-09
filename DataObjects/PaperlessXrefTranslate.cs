using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class PaperlessXrefTranslate
    {
        public class Search : SearchCreatedLast_Base
        {
            public string TableName { get; set; }

            public string ColumnName { get; set; }

            public string FromString { get; set; }

            public new string ToString { get; set; }

            public int FromInt { get; set; }

            public int ToInt { get; set; }

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
                sb.Append("SELECT * FROM PaperlessXrefTranslate WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (!string.IsNullOrEmpty(TableName))
                {
                    sb.Append(" AND TableName like '%' + @TableName + '%'");
                    ListSqlParameter.Add(new SqlParameter("@TableName", TableName));
                }

                if (!string.IsNullOrEmpty(ColumnName))
                {
                    sb.Append(" AND ColumnName like '%' + @ColumnName + '%'");
                    ListSqlParameter.Add(new SqlParameter("@ColumnName", ColumnName));
                }

                if (!string.IsNullOrEmpty(FromString))
                {
                    sb.Append(" AND FromString like '%' + @FromString + '%'");
                    ListSqlParameter.Add(new SqlParameter("@FromString", FromString));
                }

                if (!string.IsNullOrEmpty(ToString))
                {
                    sb.Append(" AND ToString like '%' + @ToString + '%'");
                    ListSqlParameter.Add(new SqlParameter("@ToString", ToString));
                }

                if (FromInt > 0)
                {
                    sb.Append(" AND FromInt = @FromInt");
                    ListSqlParameter.Add(new SqlParameter("@FromInt", FromInt));
                }

                if (ToInt > 0)
                {
                    sb.Append(" AND ToInt = @ToInt");
                    ListSqlParameter.Add(new SqlParameter("@ToInt", ToInt));
                }   

                if (BaseGenerateSqlQuery("PaperlessXrefTranslate", out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                SqlQuery = sb.ToString();
            }

        }

        public class dbRow : dbRowCreatedLast_Base
        {
            public string TableName { get; set; }

            public string ColumnName { get; set; }

            public string FromString { get; set; }

            public new string ToString { get; set; }

            public int FromInt { get; set; }

            public int ToInt { get; set; }

        }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
