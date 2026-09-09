using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class TransactionNotes : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListTransactionHeaderID { get; set; }

            public List<int> ListTransactionDetailID { get; set; }

            public List<int> ListNoteTypeID { get; set; }

            public string NoteText { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleTransactionHeader = 2,
                SingleTransactionDetail = 3,
                AllRecords = 4
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
                else if (SearchMode == SearchModeType.SingleTransactionHeader)
                {
                    if (ListTransactionHeaderID == null || ListTransactionHeaderID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("TransactionHeader", "TransactionHeaderID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.SingleTransactionDetail)
                {
                    if (ListTransactionDetailID == null || ListTransactionDetailID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("TransactionDetail", "TransactionDetailID must be specified."));
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
                sb.Append(@"SELECT TransactionNotes.*,
                                Security_User.PublicID as Created_UserPublicID
                            FROM TransactionNotes
                                LEFT JOIN Security_User on TransactionNotes.CreatedUserID = Security_User.ID
                            WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListTransactionHeaderID(ListTransactionHeaderID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListTransactionDetailID(ListTransactionDetailID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListNoteTypeID(ListNoteTypeID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(NoteText))
                {
                    sb.Append(" AND NoteText like '%' + @NoteText + '%'");
                    ListSqlParameter.Add(new SqlParameter("@NoteText", NoteText));
                }

                if (BaseGenerateSqlQuery("TransactionNotes", out generateSQL, out generateListParamater))
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
            [Range(-int.MaxValue, int.MaxValue, ErrorMessage = "TransactionHeaderID is required")]
            public int TransactionHeaderID { get; set; }
            [Range(-int.MaxValue, int.MaxValue, ErrorMessage = "TransactionDetailID is required")]
            public int TransactionDetailID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "NoteTypeID is required")]
            public int NoteTypeID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "NoteText is required")]
            public string NoteText { get; set; }

            [NotMapped]
            public string NoteType_PublicID { get; set; }
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
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "TransactionHeaderID", "TransactionDetailID", "NoteTypeID"
                , "NoteText" };

        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("NoteTypeID", "Note Type"),
                        new ChangeTrackerData("NoteText", "Note")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
