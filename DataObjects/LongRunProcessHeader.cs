using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class LongRunProcessHeader
    {
        public class Search 
        {
            public int ID { get; set; }

            public int UserID { get; set; }

            public int ProcessNameID { get; set; }

            public DateTime? StartedDateTime_from { get; set; }

            public DateTime? StartedDateTime_to { get; set; }

            public DateTime? FinishedDateTime_from { get; set; }

            public DateTime? FinishedDateTime_to { get; set; }

            public int ProcessStatusID { get; set; }

            public Boolean? CanBeDeleted { get; set; }


            public List<Tuple<string, string>> ValidationErrors { get; set; }
            public List<Tuple<string, string>> ListLocalValidationErrors = new List<Tuple<string, string>>();

            public string SqlQuery { get; set; }

            public List<SqlParameter> ListSqlParameter { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                AllRecords = 2,
                UserID = 3
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
                else if (SearchMode == SearchModeType.UserID)
                {
                    if (UserID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("UserID", "UserID must be specified."));
                    }
                }
                else
                {
                    // Validate each field as required

                    if (DataObjectValidator.DateMinMax("StartedDateTime",StartedDateTime_from, StartedDateTime_to, out ListLocalValidationErrors))
                    {
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }

                    if (DataObjectValidator.DateMinMax("FinishedDateTime", FinishedDateTime_from, FinishedDateTime_to, out ListLocalValidationErrors))
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
                sb.Append("SELECT * FROM LongRunProcessHeader WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (UserID > 0)
                {
                    sb.Append(" AND UserID = @UserID");
                    ListSqlParameter.Add(new SqlParameter("@UserID", UserID));
                }

                if (ProcessNameID > 0)
                {
                    sb.Append(" AND ProcessNameID = @ProcessNameID");
                    ListSqlParameter.Add(new SqlParameter("@ProcessNameID", ProcessNameID));
                }

                if (DataObjectSQLGenerator.Date_FromTo("StartedDateTime", StartedDateTime_from, StartedDateTime_to, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("FinishedDateTime", FinishedDateTime_from, FinishedDateTime_to, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (ProcessStatusID > 0)
                {
                    sb.Append(" AND ProcessStatusID = @ProcessStatusID");
                    ListSqlParameter.Add(new SqlParameter("@ProcessStatusID", ProcessStatusID));
                }

                if (CanBeDeleted.HasValue)
                {
                    sb.Append(" AND CanBeDeleted = @CanBeDeleted");
                    ListSqlParameter.Add(new SqlParameter("@CanBeDeleted", CanBeDeleted.Value));
                }

                SqlQuery = sb.ToString();
            }

        }

        public class dbRow 
        {
            public int ID { get; set; }

            public int UserID { get; set; }

            public string ProcessName { get; set; }

            public int ProcessNameID { get; set; }

            public DateTime StartedDateTime { get; set; }

            public DateTime? FinishedDateTime { get; set; }

            public int ProcessStatusID { get; set; }

            public int MaxCounter { get; set; }

            public int CurrentCounter { get; set; }

            public DateTime StatusDateTime { get; set; }

            public string FilenamePath { get; set; }

            public string DataToReturn { get; set; }

            public Boolean CanBeDeleted { get; set; }

            [NotMapped]
            public string ProcessNameID_PublicID { get; set; }  

            [NotMapped]
            public string Security_User_PublicID { get; set; }

            [NotMapped]
            public string LongRunProcessStatusDescription_Description { get; set; }


        }

        public dbRow dbRow_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
