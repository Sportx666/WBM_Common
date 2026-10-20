using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using wbm_common.Utility;

namespace wbm_common.DataObjects
{
    public class Stock : ChangeTracking
    {
        public ToDo ToDo { get; set; }
        public class Search : SearchCreatedLast_Base
        {
            public List<int> ListCompanyID { get; set; }

            public List<int> ListOwnerID { get; set; }
            public List<string> ListProductCode { get; set; }

            public List<int> ListSiteID { get; set; }

            public string ProductCode { get; set; }

            public string Description { get; set; }

            public List<int> ListCategoryID { get; set; }

            public Decimal? Length_min { get; set; }

            public Decimal? Length_max { get; set; }

            public Decimal? Width_min { get; set; }

            public Decimal? Width_max { get; set; }

            public Decimal? Height_min { get; set; }

            public Decimal? Height_max { get; set; }

            public Decimal? Weight_min { get; set; }

            public Decimal? Weight_max { get; set; }

            public List<int> ListPalletStorageTypeID { get; set; }

            public List<int> ListStockRateCategoryID { get; set; }

            public int? ItemsPerPallet_min { get; set; }

            public int? ItemsPerPallet_max { get; set; }

            public string Paperless_KeyID { get; set; }

            public enum SearchModeType
            {
                Undefined = 0,
                SingleID = 1,
                SingleSite = 2,
                SingleOwner = 3,
                Paperless_KeyID = 4,
                AllRecords = 5,
                Owner_ListProductCode = 6,
                ListID = 7
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
                else if (SearchMode == SearchModeType.SingleOwner)
                {
                    if (ListOwnerID == null || ListOwnerID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Owner", "OwnerID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.SingleSite)
                {
                    if (ListSiteID == null || ListSiteID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Site", "SiteID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.Paperless_KeyID)
                {
                    if (string.IsNullOrEmpty(Paperless_KeyID))
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Paperless_KeyID", "Paperless_KeyID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.Owner_ListProductCode)
                {
                    if (ListProductCode == null || ListProductCode.Any(x => string.IsNullOrEmpty(x)))
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ListProductCode", "ListProductCode must be specified."));
                    }
                    if (ListOwnerID == null || ListOwnerID.Count != 1)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("Owner", "OwnerID must be specified."));
                    }
                }
                else if (SearchMode == SearchModeType.ListID)
                {
                    if (ListID == null || ListID.Count == 0)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>("ListID", "ListID must be populated."));
                    }
                }
                else
                {
                    // Validate each field as required

                    if (DataObjectValidator.DecimalMinMax("Length", Length_min, Length_max, out ListLocalValidationErrors) == false)
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }
                    if (DataObjectValidator.DecimalMinMax("Width", Width_min, Width_max, out ListLocalValidationErrors) == false)
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }
                    if (DataObjectValidator.DecimalMinMax("Height", Height_min, Height_max, out ListLocalValidationErrors) == false)
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }
                    if (DataObjectValidator.DecimalMinMax("Weight", Weight_min, Weight_max, out ListLocalValidationErrors) == false)
                    {
                        rtnVal = false;
                        ValidationErrors.AddRange(ListLocalValidationErrors);
                    }
                    if (DataObjectValidator.IntMinMax("ItemsPerPallet", ItemsPerPallet_min, ItemsPerPallet_max, out ListLocalValidationErrors) == false)
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
                sb.Append(@"SELECT Stock.*,
                            Site.PublicID AS Site_PublicID,
                            Company.ShortName AS Company_ShortName,
                            Owner.Name AS Owner_Name,
                            ProcessCategory.PublicID AS CategoryID_PublicID,
                            StockRateCategory.PublicID AS StockRateCategory_PublicID,
                            PalletStorageType.PublicID AS PalletStorageType_PublicID,
                            Security_User.PublicID as Created_UserPublicID
                    FROM Stock 
                        LEFT JOIN Site ON Stock.SiteID = Site.ID
                        LEFT JOIN Company ON Stock.CompanyID = Company.ID
                        LEFT JOIN Owner ON Stock.OwnerID = Owner.ID
                        LEFT JOIN ProcessCategory ON Stock.CategoryID = ProcessCategory.ID
                        LEFT JOIN StockRateCategory ON Stock.StockRateCategoryID = StockRateCategory.ID
                        LEFT JOIN PalletStorageType ON Stock.PalletStorageTypeID = PalletStorageType.ID
                        LEFT JOIN Security_User on Stock.CreatedUserID = Security_User.ID
                    WHERE 1=1");

                string generateSQL = null;
                List<SqlParameter> generateListParamater = null;

                if (DataObjectSQLGenerator.ListCompanyID(ListCompanyID, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListSiteID(ListSiteID, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListOwnerID(ListOwnerID, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (!string.IsNullOrEmpty(ProductCode))
                {
                    sb.Append(" AND Stock.ProductCode LIKE @ProductCode");
                    ListSqlParameter.Add(new SqlParameter("@ProductCode", "%" + ProductCode + "%"));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append(" AND Stock.Description LIKE @Description");
                    ListSqlParameter.Add(new SqlParameter("@Description", "%" + Description + "%"));
                }

                if (DataObjectSQLGenerator.ListCategoryID(ListCategoryID, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListID(ListID, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("Length", Length_min, Length_max, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("Width", Width_min, Width_max, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("Height", Height_min, Height_max, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Decimal_MinMax("Weight", Weight_min, Weight_max, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }   

                if (DataObjectSQLGenerator.ListPalletStorageTypeID(ListPalletStorageTypeID, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.ListStockRateCategoryID(ListStockRateCategoryID, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }

                if (DataObjectSQLGenerator.Int_MinMax("ItemsPerPallet", ItemsPerPallet_min, ItemsPerPallet_max, out generateSQL, out generateListParamater, "Stock."))
                {
                    sb.Append(generateSQL);
                    if (generateListParamater != null)
                        ListSqlParameter.AddRange(generateListParamater);
                }


                if (!string.IsNullOrEmpty(Paperless_KeyID))
                {
                    sb.Append(" AND Stock.Paperless_KeyID = @Paperless_KeyID");
                    ListSqlParameter.Add(new SqlParameter("@Paperless_KeyID", Paperless_KeyID));
                }

                if (BaseGenerateSqlQuery("Stock", out generateSQL, out generateListParamater))
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
            [Range(1, int.MaxValue, ErrorMessage = "CompanyID is required")]
            public int CompanyID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "OwnerID is required")]
            public int OwnerID { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "SiteID is required")]
            public int SiteID { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "ProductCode is required")]
            public string ProductCode { get; set; }
            [Required(AllowEmptyStrings = true, ErrorMessage = "Description is required")]
            public string Description { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "CategoryID is required")]
            public int CategoryID { get; set; }
            [Required(ErrorMessage = "Length is required")]
            public Decimal Length { get; set; }
            [Required(ErrorMessage = "Width is required")]
            public Decimal Width { get; set; }
            [Required(ErrorMessage = "Height is required")]
            public Decimal Height { get; set; }
            [Required(ErrorMessage = "Weight is required")]
            public Decimal Weight { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "PalletStorageTypeID is required")]
            public int PalletStorageTypeID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "StockRateCategoryID is required")]
            public int StockRateCategoryID { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "ItemsPerPallet is required")]
            public int ItemsPerPallet { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "ItemsPerLayer is required")]
            public int ItemsPerLayer { get; set; }
            [Required(ErrorMessage = "LayerPickEligible is required")]
            public bool LayerPickEligible { get; set; }

            public string Paperless_KeyID { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Owner_Name { get; set; }
            public string? Site_PublicID { get; set; }
            public string? CategoryID_PublicID { get; set; }
            public string? StockRateCategory_PublicID { get; set; }
            public string? PalletStorageType_PublicID { get; set; }

            public string? Created_UserPublicID { get; set; }
           // public string? LastAmended_UserPublicID { get; set; }

        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
        [JsonIgnore]
        public static readonly List<string> MyRequiredList = new List<string> { "CompanyID", "SiteID", "OwnerID", "ProductCode"
                , "Description", "CategoryID", "Length", "Width", "Height", "Weight"
                , "PalletStorageTypeID", "StockRateCategoryID", "ItemsPerPallet", "ItemsPerLayer", "LayerPickEligible" };
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
                        new ChangeTrackerData("ProductCode", "Product Code"),
                        new ChangeTrackerData("Description", "Description"),
                        new ChangeTrackerData("CategoryID", "Category ID"),
                        new ChangeTrackerData("Length", "Length", true),
                        new ChangeTrackerData("Width", "Width", true),
                        new ChangeTrackerData("Height", "Height", true),
                        new ChangeTrackerData("Weight", "Weight", true),
                        new ChangeTrackerData("PalletStorageTypeID", "Pallet Storage Type ID"),
                        new ChangeTrackerData("StockRateCategoryID", "Stock Rate Category ID"),
                        new ChangeTrackerData("ItemsPerPallet", "Items Per Pallet"),
                        new ChangeTrackerData("ItemsPerLayer", "Items Per Layer"),
                        new ChangeTrackerData("Paperless_KeyID", "Paperless Key ID")
                    };
                }
                return _ListChangeTrackerData;
            }
        }

    }
}
