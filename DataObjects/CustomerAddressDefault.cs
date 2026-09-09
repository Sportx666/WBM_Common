using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class CustomerAddressDefault : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public string PublicID { get; set; }
            public string Description { get; set; }

            public List<int> ListCompanyID { get; set; }
            public List<int> ListOwnerID { get; set; }
            public List<int> ListSiteID { get; set; }

            public int? PalletHireDelayDays_min { get; set; }
            public int? PalletHireDelayDays_max { get; set; }

            public Boolean? InvoiceEnclosed { get; set; }

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
                    if (!DataObjectValidator.IntMinMax("PalletHireDelayDays", PalletHireDelayDays_min, PalletHireDelayDays_max, out ListLocalValidationErrors))
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
                sb.Append(@"SELECT CustomerAddressDefault.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            Security_User.PublicID as Created_UserPublicID
                    FROM CustomerAddressDefault 
                        LEFT JOIN Site ON CustomerAddressDefault.SiteID = Site.ID
                        LEFT JOIN Company ON CustomerAddressDefault.CompanyID = Company.ID
                        LEFT JOIN Owner ON CustomerAddressDefault.OwnerID = Owner.ID
                        LEFT JOIN Security_User on CustomerAddressDefault.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (!string.IsNullOrEmpty(PublicID))
                {
                    sb.Append(" AND CustomerAddressDefault.PublicID = @PublicID");
                    ListSqlParameter.Add(new SqlParameter("@PublicID", PublicID));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND CustomerAddressDefault.Description like '%' + @Description + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Description", Description));
                }

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "CustomerAddressDefault."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "CustomerAddressDefault."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "CustomerAddressDefault."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Int_MinMax("PalletHireDelayDays", PalletHireDelayDays_min, PalletHireDelayDays_max, out generateSQL, out generateListParamater, "CustomerAddressDefault."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (InvoiceEnclosed.HasValue)
                {
                    sb.Append(" AND CustomerAddressDefault.InvoiceEnclosed = @InvoiceEnclosed");
                    ListSqlParameter.Add(new SqlParameter("@InvoiceEnclosed", InvoiceEnclosed.Value));
                }

                if (BaseGenerateSqlQuery("CustomerAddressDefault", out generateSQL, out generateListParamater))
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
            [Required(AllowEmptyStrings = false, ErrorMessage = "PublicID is required")]
            public string PublicID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "PalletHireDelayDays is required")]
            public int PalletHireDelayDays { get; set; }
            public int SortOrder { get; set; }
            [Required(ErrorMessage = "InvoiceEnclosed is required")]
            public Boolean InvoiceEnclosed { get; set; }
        }

        public dbRow dbRow_instance { get; set; }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }

            public string? Owner_Name { get; set; }

            public string? Site_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }
        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "PublicID", "SiteID", "Description", "CompanyID",
                "OwnerID", "PalletHireDelayDays", "InvoiceEnclosed" };
        private static List<ChangeTrackerData> _ListChangeTrackerData;

        public static List<ChangeTrackerData> ListChangeTrackerData
        {
            get
            {
                if (_ListChangeTrackerData == null)
                {
                    _ListChangeTrackerData = new List<ChangeTrackerData>()
                    {
                        new ChangeTrackerData("PublicID", "Public ID"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("CompanyID", "Company ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("OwnerID", "Owner ID"),
                        new ChangeTrackerData("PalletHireDelayDays", "Pallet Hire Delay Days"),
                        new ChangeTrackerData("InvoiceEnclosed", "Invoice Enclosed"),
                        new ChangeTrackerData("SortOrder", "Sort Order")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
