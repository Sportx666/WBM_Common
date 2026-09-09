using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static wbm_common.DataObjects.Carrier.Search;

namespace wbm_common.DataObjects
{
    public class SearchCreatedLast_Base
    {
        public int ID { get; set; }
        public List<int> ListID { get; set; }

        public DateTime? CreatedDateTime_from { get; set; }
        public DateTime? CreatedDateTime_to { get; set; }

        public string CreatedMethod { get; set; }

        public int CreatedUserID { get; set; }

        public DateTime? LastAmendedDateTime_from { get; set; }
        public DateTime? LastAmendedDateTime_to { get; set; }

        public string LastAmendedMethod { get; set; }

        public int LastAmendedUserID { get; set; }

        public System.Boolean? DeletedFlag { get; set; }

        public Boolean PopulateLookupFields { get; set; } = false;


        public List<Tuple<string, string>> ValidationErrors { get; set; }
        public List<Tuple<string, string>> ListLocalValidationErrors = new List<Tuple<string, string>>();

        public string SqlQuery { get; set; }

        public List<SqlParameter> ListSqlParameter { get; set; }


        public System.Boolean BaseIsValid(out List<Tuple<string, string>> ValidationErrors)
        {
            System.Boolean rtnVal = true; // assume valid
            ValidationErrors = new List<Tuple<string, string>>();

            if (CreatedDateTime_from.HasValue)
            {
                if (CreatedDateTime_to.HasValue)
                {
                    if (CreatedDateTime_from > CreatedDateTime_to)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Created Date", "'From' date must be earlier than 'To' date."));
                    }
                }
            }

            if (LastAmendedDateTime_from.HasValue)
            {
                if (LastAmendedDateTime_to.HasValue)
                {
                    if (LastAmendedDateTime_from > LastAmendedDateTime_to)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Last Amended Date", "'From' date must be earlier than 'To' date."));
                    }
                }
            }

            return rtnVal;
        }

        public System.Boolean BaseGenerateSqlQuery(string tableAlias, out string SqlStatement, out List<SqlParameter> ListSqlParameter)
        {
            System.Boolean rtnVal = true; // assume valid
            SqlStatement = null;
            string prefix = string.IsNullOrEmpty(tableAlias) ? "" : tableAlias + ".";
            ListSqlParameter = new List<SqlParameter>();

            StringBuilder sb = new StringBuilder();

            if (CreatedDateTime_from != null)
            {
                sb.Append($" AND {prefix}CreatedDateTime >= @FromCreatedDateTime");
                ListSqlParameter.Add(new SqlParameter("@FromCreatedDateTime", CreatedDateTime_from.Value));
            }

            if (CreatedDateTime_to != null)
            {
                sb.Append($" AND {prefix}CreatedDateTime <= @ToCreatedDateTime");
                ListSqlParameter.Add(new SqlParameter("@ToCreatedDateTime", CreatedDateTime_to.Value));
            }

            if (LastAmendedDateTime_from != null)
            {
                sb.Append($" AND {prefix}LastAmendedDateTime >= @FromLastAmendedDateTime");
                ListSqlParameter.Add(new SqlParameter("@FromLastAmendedDateTime", LastAmendedDateTime_from));
            }

            if (LastAmendedDateTime_to != null)
            {
                sb.Append($" AND {prefix}LastAmendedDateTime <= @ToLastAmendedDateTime");
                ListSqlParameter.Add(new SqlParameter("@ToLastAmendedDateTime", LastAmendedDateTime_to));
            }

            if (!string.IsNullOrEmpty(CreatedMethod))
            {
                sb.Append($" AND {prefix}CreatedMethod = @CreatedMethod");
                ListSqlParameter.Add(new SqlParameter("@CreatedMethod", CreatedMethod));
            }

            if (CreatedUserID != 0)
            {
                sb.Append($" AND {prefix}CreatedUserID = @CreatedUserID");
                ListSqlParameter.Add(new SqlParameter("@CreatedUserID", CreatedUserID));
            }

            if (!string.IsNullOrEmpty(LastAmendedMethod))
            {
                sb.Append($" AND {prefix}LastAmendedMethod = @LastAmendedMethod");
                ListSqlParameter.Add(new SqlParameter("@LastAmendedMethod", LastAmendedMethod));
            }

            if (LastAmendedUserID != 0)
            {
                sb.Append($" AND {prefix}LastAmendedUserID = @LastAmendedUserID");
                ListSqlParameter.Add(new SqlParameter("@LastAmendedUserID", LastAmendedUserID));
            }

            sb.Append($" AND {prefix}DeletedFlag = '");
            if (DeletedFlag.HasValue)
                sb.Append("True'");
            else
                sb.Append("False'");


            SqlStatement = sb.ToString();

            return rtnVal;

        }


    }
}
