using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class CustomerAddress : ChangeTracking
    {
        public ToDo ToDo { get; set; }
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListOwnerID { get; set; }
            public List<int> ListSiteID { get; set; }

            public string CustomerID { get; set; }

            public string Name { get; set; }
            public string Address { get; set; }
            public string Suburb { get; set; }
            public string State { get; set; }
            public string PostCode { get; set; }

            public int? PalletHireDelayDays_min { get; set; }
            public int? PalletHireDelayDays_max { get; set; }

            public Boolean? InvoiceEnclosed { get; set; }
            public string Paperless_KeyID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                Paperless_KeyID = 2,
                AllRecords = 3,
                OwnerCustomerID = 4
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
                else if (SearchMode == SearchModeType.OwnerCustomerID)
                {
                    if (string.IsNullOrEmpty(CustomerID))
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Customer", "CustomerID must be specified."));
                    }
                    if (ListOwnerID == null || ListOwnerID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Owner", "OwnerID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.Paperless_KeyID)
                {
                    if (string.IsNullOrEmpty(Paperless_KeyID))
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Paperless_KeyID", "Paperless KeyID must be specified."));
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
                sb.Append(@"SELECT CustomerAddress.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            Security_User.PublicID as Created_UserPublicID

                    FROM CustomerAddress 
                        LEFT JOIN Site ON CustomerAddress.SiteID = Site.ID
                        LEFT JOIN Company ON CustomerAddress.CompanyID = Company.ID
                        LEFT JOIN Owner ON CustomerAddress.OwnerID = Owner.ID
                        LEFT JOIN Security_User on CustomerAddress.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "CustomerAddress."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "CustomerAddress."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "CustomerAddress."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }


                if (!string.IsNullOrEmpty(CustomerID))
                {
                    sb.Append(" AND CustomerAddress.CustomerID like '%' + @CustomerID + '%'");
                    ListSqlParameter.Add(new SqlParameter("@CustomerID", CustomerID));
                }

                if (!string.IsNullOrEmpty(Name))
                {
                    sb.Append(" AND CustomerAddress.Name like '%' + @Name + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Name", Name));
                }

                if (!string.IsNullOrEmpty(Address))
                {
                    sb.Append(" AND (CustomerAddress.AddressLine1 like '%' + @Address + '%'");
                    sb.Append(" OR CustomerAddress.AddressLine2 like '%' + @Address + '%'");
                    sb.Append(" OR CustomerAddress.AddressLine3 like '%' + @Address + '%'");
                    sb.Append(" OR CustomerAddress.AddressLine4 like '%' + @Address + '%')");

                    ListSqlParameter.Add(new SqlParameter("@Address", Address));
                }

                if (!string.IsNullOrEmpty(Suburb))
                {
                    sb.Append(" AND CustomerAddress.Suburb like '%' + @Suburb + '%'");
                    ListSqlParameter.Add(new SqlParameter("@Suburb", Suburb));
                }

                if (!string.IsNullOrEmpty(State))
                {
                    sb.Append(" AND CustomerAddress.State like '%' + @State + '%'");
                    ListSqlParameter.Add(new SqlParameter("@State", State));
                }

                if (!string.IsNullOrEmpty(PostCode))
                {
                    sb.Append(" AND CustomerAddress.PostCode like '%' + @PostCode + '%'");
                    ListSqlParameter.Add(new SqlParameter("@PostCode", PostCode));
                }

                if (DataObjectSQLGenerator.Int_MinMax("PalletHireDelayDays", PalletHireDelayDays_min, PalletHireDelayDays_max, out generateSQL, out generateListParamater, "CustomerAddress."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (InvoiceEnclosed.HasValue)
                {
                    sb.Append(" AND CustomerAddress.InvoiceEnclosed = @InvoiceEnclosed");
                    ListSqlParameter.Add(new SqlParameter("@InvoiceEnclosed", InvoiceEnclosed.Value));
                }

                if (BaseGenerateSqlQuery("CustomerAddress",out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "CustomerID is required")]
            public string CustomerID { get; set; }

            public string Name { get; set; }

            public string AddressLine1 { get; set; }

            public string AddressLine2 { get; set; }

            public string AddressLine3 { get; set; }

            public string AddressLine4 { get; set; }

            public string Suburb { get; set; }

            public string State { get; set; }

            public string PostCode { get; set; }
            public int? PalletHireDelayDays { get; set; }
            public Boolean? InvoiceEnclosed { get; set; }

            public string Paperless_KeyID { get; set; }

            public string Connote_RateCollectionRateChargeCode { get; set; }
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
        public static readonly List<string> MyRequiredList = new List<string> { "CustomerID", "SiteID", "CompanyID",
                "OwnerID" };
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
                        new ChangeTrackerData("OwnerID", "Owner ID"),
                        new ChangeTrackerData("SiteID", "Site ID"),
                        new ChangeTrackerData("CustomerID", "Customer ID"),
                        new ChangeTrackerData("Name", "Name"),
                        new ChangeTrackerData("AddressLine1", "Address Line 1"),
                        new ChangeTrackerData("AddressLine2", "Address Line 2"),
                        new ChangeTrackerData("AddressLine3", "Address Line 3"),
                        new ChangeTrackerData("AddressLine4", "Address Line 4"),
                        new ChangeTrackerData("Suburb", "Suburb"),
                        new ChangeTrackerData("State", "State"),
                        new ChangeTrackerData("PostCode", "Post Code"),
                        new ChangeTrackerData("PalletHireDelayDays", "Pallet Hire Delay Days"),
                        new ChangeTrackerData("InvoiceEnclosed", "Invoice Enclosed"),
                        new ChangeTrackerData("Paperless_KeyID", "Paperless Key ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
