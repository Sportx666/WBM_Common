using System.ComponentModel.DataAnnotations.Schema;

namespace wbm_common.DataObjects
{
    public class Security_Item
    {
        public class Search : SearchCreatedLast_Base
        {
            public int ID { get; set; }
            public List<int> ListItemCategoryID { get; set; }

            public string TagID { get; set; }
            public enum SearchModeType
            {
                AllRecords = 2
            }
            public SearchModeType SearchMode { get; set; } = SearchModeType.AllRecords;
        }

        public class dbRow : dbRowCreatedLast_Base
        {
            public int ItemCategoryID { get; set; }

            public string TagID { get; set; }
        }

        public class dbRowExtended : dbRow
        {
            public string ItemCategory_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }


        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
