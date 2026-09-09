using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class ConnoteFee : ChangeTracking
    {
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }
            public List<int> ListOwnerID { get; set; }
            public List<int> ListSiteID { get; set; }
            public List<int> ListCustomerID { get; set; }
            public List<int> ListCarrierID { get; set; }
            public string RateCollectionRateChargeCode { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleOwnerID = 2,
                AllRecords = 3
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
                sb.Append(@"SELECT ConnoteFee.* ,
                        Site.PublicID AS Site_PublicID,
                        Company.ShortName AS Company_ShortName,
                        Owner.Name AS Owner_Name,
                        Carrier.PublicID AS Carrier_PublicID,
                        CustomerAddress.Name AS Customer_PublicID,
                        RateCode.Description AS RateCode_Code,
                        Security_User.PublicID as Created_UserPublicID
                    FROM ConnoteFee 
                        LEFT JOIN Site ON ConnoteFee.SiteID = Site.ID
                        LEFT JOIN Company ON ConnoteFee.CompanyID = Company.ID
                        LEFT JOIN Owner ON ConnoteFee.OwnerID = Owner.ID
                        LEFT JOIN CustomerAddress ON ConnoteFee.CustomerID = CustomerAddress.ID
                        LEFT JOIN Carrier ON ConnoteFee.CarrierID = Carrier.ID
                        LEFT JOIN RateCode ON ConnoteFee.RateCollectionRateChargeCode = RateCode.RateCode
                        LEFT JOIN Security_User on ConnoteFee.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "ConnoteFee."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "ConnoteFee."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "ConnoteFee."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListCustomerID(ListCustomerID, out generateSQL, out generateListParamater, "ConnoteFee."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListCarrierID(ListCarrierID, out generateSQL, out generateListParamater, "ConnoteFee."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(RateCollectionRateChargeCode))
                {
                    sb.Append(" AND ConnoteFee.RateCollectionRateChargeCode like '%' + @RateCollectionRateChargeCode + '%'");
                    ListSqlParameter.Add(new SqlParameter("@RateCollectionRateChargeCode", RateCollectionRateChargeCode));
                }

                if (BaseGenerateSqlQuery("ConnoteFee", out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "CustomerAddressID is required")]
            public int CustomerAddressID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "CarrierID is required")]
            public int CarrierID { get; set; }
            [Required(ErrorMessage = "RateCollectionRateChargeCode is required")]
            public string RateCollectionRateChargeCode { get; set; }
        }
        public dbRow dbRow_instance { get; set; }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? Carrier_PublicID { get; set; }
            public string? Customer_PublicID { get; set; }
            public string? RateCode_Code { get; set; }
            public string? Created_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "OwnerID", "CustomerAddressID", "CarrierID", "RateCollectionRateChargeCode" };
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
                        new ChangeTrackerData("CustomerAddressID", "Customer Address ID"),
                        new ChangeTrackerData("CarrierID", "Carrier ID"),
                        new ChangeTrackerData("RateCollectionRateChargeCode", "RateCollection Rate Charge Code"),
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
