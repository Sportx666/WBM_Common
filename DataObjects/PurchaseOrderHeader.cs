using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.NonDatabaseObjects;
using wbm_common.Utility;
using static wbm_common.DataObjects.ChangeTracking;

namespace wbm_common.DataObjects
{
    public class PurchaseOrderHeader
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListSiteID { get; set; }
            public List<int> ListOwnerID { get; set; }

            public DateTime? PurchaseOrderDateTime_from { get; set; }
            public DateTime? PurchaseOrderDateTime_to { get; set; }

            public List<int> ListTransactionTypeID { get; set; }

            public string TransactionReference { get; set; }
            public string Description { get; set; }

            public string SupplierName { get; set; }

            public string ContainerNumber { get; set; }

            public List<int> ListContainerSizeID { get; set; }

            public List<int> ListContainerUnloadTypeID { get; set; }

            public Boolean? EDIReceipt { get; set; }

            public Boolean? Priority { get; set; }

            public int TransactionHeaderID { get; set; }

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

                    if (DataObjectValidator.DateMinMax("PurchaseOrderDateTime", PurchaseOrderDateTime_from, PurchaseOrderDateTime_to, out ListLocalValidationErrors) == false)
                    {
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
                sb.Append(@"SELECT PurchaseOrderHeader.*,
                                 Security_User.PublicID as Created_UserPublicID
                            FROM PurchaseOrderHeader
                                LEFT JOIN Security_User on PurchaseOrderHeader.CreatedUserID = Security_User.ID
                            WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Date_FromTo("PurchaseOrderDateTime", PurchaseOrderDateTime_from, PurchaseOrderDateTime_to, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListTransactionTypeID(ListTransactionTypeID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(TransactionReference))
                {
                    sb.Append(" AND TransactionReference like '%' + @TransactionReference + '%'");
                    ListSqlParameter.Add(new SqlParameter("@TransactionReference", TransactionReference));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (!string.IsNullOrEmpty(SupplierName))
                {
                    sb.Append(" AND SupplierName like '%' + @SupplierName + '%'");
                    ListSqlParameter.Add(new SqlParameter("@SupplierName", SupplierName));
                }

                if (!string.IsNullOrEmpty(ContainerNumber))
                {
                    sb.Append(" AND ContainerNumber like '%' + @ContainerNumber + '%'");
                    ListSqlParameter.Add(new SqlParameter("@ContainerNumber", ContainerNumber));
                }

                if (DataObjectSQLGenerator.ListContainerSizeID(ListContainerSizeID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListContainerUnloadTypeID(ListContainerUnloadTypeID, out generateSQL, out generateListParamater))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (EDIReceipt.HasValue)
                {
                    sb.Append(" AND EDIReceipt = @EDIReceipt");
                    ListSqlParameter.Add(new SqlParameter("@EDIReceipt", EDIReceipt.Value));
                }

                if (Priority.HasValue)
                {
                    sb.Append(" AND Priority = @Priority");
                    ListSqlParameter.Add(new SqlParameter("@Priority", Priority.Value));
                }

                if (TransactionHeaderID > 0)
                {
                    sb.Append(" AND TransactionHeaderID = @TransactionHeaderID");
                    ListSqlParameter.Add(new SqlParameter("@TransactionHeaderID", TransactionHeaderID));
                }

                if (BaseGenerateSqlQuery("PurchaseOrderHeader",out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Required(ErrorMessage = "PurchaseOrderDateTime is required")]
            public DateTime PurchaseOrderDateTime { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "TransactionTypeID is required")]
            public int TransactionTypeID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "TransactionReference is required")]
            public string TransactionReference { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "SupplierName is required")]
            public string SupplierName { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "ContainerNumber is required")]
            public string ContainerNumber { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "ContainerSizeID is required")]
            public int ContainerSizeID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "ContainerUnloadTypeID is required")]
            public int ContainerUnloadTypeID { get; set; }
            [Required(ErrorMessage = "EDIReceipt is required")]
            public Boolean EDIReceipt { get; set; }
            [Required(ErrorMessage = "Priority is required")]
            public Boolean Priority { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "TransactionHeaderID is required")]
            public int TransactionHeaderID { get; set; }
        }
        public class dbRowExtended : dbRow
        {
            public string Company_ShortName { get; set; }
            public string Owner_Name { get; set; }
            public string Site_PublicID { get; set; }
            public string TransactionType_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }
        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "OwnerID"
                , "PurchaseOrderDateTime", "TransactionTypeID", "TransactionReference", "Description", "SupplierName", "ContainerNumber"
            , "ContainerSizeID", "ContainerUnloadTypeID", "EDIReceipt", "Priority", "TransactionHeaderID" };

        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("CompanyID", "Company ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("OwnerID", "Owner ID"),
                        new ChangeTrackerData("PurchaseOrderDateTime", "Purchase Order DateTime"),
                        new ChangeTrackerData("TransactionTypeID", "Transaction Type ID"),
                        new ChangeTrackerData("TransactionReference", "Transaction Reference"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("SupplierName", "Supplier Name"),
                        new ChangeTrackerData("ContainerNumber", "Container Number"),
                        new ChangeTrackerData("ContainerSizeID", "Container Size ID"),
                        new ChangeTrackerData("ContainerUnloadTypeID", "Container Unload Type ID"),
                        new ChangeTrackerData("EDIReceipt", "EDI Receipt"),
                        new ChangeTrackerData("Priority", "Priority"),
                        new ChangeTrackerData("TransactionHeaderID", "Transaction Header ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }
    }
}
