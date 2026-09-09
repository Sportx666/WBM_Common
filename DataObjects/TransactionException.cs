using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class TransactionException : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListTransactionHeaderID { get; set; }
            public List<int> ListTransactionDetailID { get; set; }

            public List<int> ListTransactionTypeID { get; set; }

            public List<int> ListSiteID { get; set; }

            public string Description { get; set; }

            public Boolean? ClearedFlag { get; set; }

            public DateTime? ClearedDateTime_from { get; set; }

            public DateTime? ClearedDateTime_to { get; set; }

            public string ClearedMethod { get; set; }

            public int ClearedUserID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                TransactionHeaderID = 2
            }

            public SearchModeType SearchMode { get; set; } = SearchModeType.Undefined;

            public System.Boolean IsValid()
            {
                ValidationErrors = new List<Tuple<string, string>>();

                System.Boolean rtnVal = true; // assume valid

                if (SearchMode == SearchModeType.SingleID)
                {
                    if (ID <= 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ID", "Single ID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.TransactionHeaderID)
                {
                    if (ListTransactionHeaderID == null || ListTransactionHeaderID.Count < 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("TransactionHeader", "TransactionHeaderID must be specified."));
                    }
                }
                else
                {
                    // Validate each field as required

                    if (!DataObjectValidator.DateMinMax("ClearedDateTime", ClearedDateTime_from, ClearedDateTime_to, out ListLocalValidationErrors))
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }   

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
                sb.Append(@"SELECT TransactionException.*,
                            TransactionHeader.Description AS TransactionHeader_Description,
                            TransactionDetail.FullDescription AS TransactionDetail_FullDescription,
                            TransactionExceptionType.PublicID AS TransactionExceptionType_PublicID,            
                            Security_User.PublicID as Created_UserPublicID
                    FROM TransactionException 
                        LEFT JOIN TransactionHeader ON TransactionException.TransactionHeaderID = TransactionHeader.ID
                        LEFT JOIN TransactionDetail ON TransactionException.TransactionDetailID = TransactionDetail.ID
                        LEFT JOIN TransactionExceptionType ON TransactionException.TransactionExceptionTypeID = TransactionExceptionType.ID
                        LEFT JOIN Security_User on TransactionException.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListTransactionHeaderID(ListTransactionHeaderID, out generateSQL, out generateListParamater, "TransactionException."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListTransactionDetailID(ListTransactionDetailID, out generateSQL, out generateListParamater, "TransactionException."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListTransactionExceptionTypeID(ListTransactionTypeID, out generateSQL, out generateListParamater, "TransactionException."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND TransactionException.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (ClearedFlag.HasValue)
                {
                    sb.Append(" AND TransactionException.ClearedFlag = @ClearedFlag");
                    ListSqlParameter.Add(new SqlParameter("@ClearedFlag", ClearedFlag.Value));
                }

                if (DataObjectSQLGenerator.Date_FromTo("ClearedDateTime", ClearedDateTime_from, ClearedDateTime_to, out generateSQL, out generateListParamater, "TransactionException."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (BaseGenerateSqlQuery("TransactionException",out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                SqlQuery = sb.ToString();
            }


        }

        public dbRow dbRow_instance { get; set; }
        public class dbRow :dbRowCreatedLast_Base
        {
            [Range(-int.MaxValue, int.MaxValue, ErrorMessage = "TransactionHeaderID is required")]
            public int TransactionHeaderID { get; set; }
            [Range(-int.MaxValue, int.MaxValue, ErrorMessage = "TransactionDetailID is required")]
            public int TransactionDetailID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "TransactionExceptionTypeID is required")]
            public int TransactionExceptionTypeID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Required(ErrorMessage = "ClearedFlag is required")]
            public Boolean ClearedFlag { get; set; }
            [Required(ErrorMessage = "ClearedDateTime is required")]
            public DateTime ClearedDateTime { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "ClearedMethod is required")]
            public string ClearedMethod { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "ClearedUserID is required")]
            public int ClearedUserID { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? TransactionExceptionType_PublicID { get; set; }
            public string? TransactionHeader_Description { get; set; }
            public string? TransactionDetail_FullDescription { get; set; }
            public string? ClearedUser_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "TransactionHeaderID", "TransactionDetailID", "TransactionExceptionTypeID"
                , "Description" , "ClearedFlag" , "ClearedDateTime" , "ClearedMethod" , "ClearedUserID" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("TransactionHeaderID", "Transaction Header ID"),
                        new ChangeTrackerData("TransactionDetailID", "Transaction Detail ID"),
                        new ChangeTrackerData("TransactionExceptionTypeID", "Transaction Exception Type ID"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("ClearedFlag", "Cleared Flag"),
                        new ChangeTrackerData("ClearedDateTime", "Cleared DateTime"),
                        new ChangeTrackerData("ClearedMethod", "Cleared Method"),
                        new ChangeTrackerData("ClearedUserID", "Cleared User ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
