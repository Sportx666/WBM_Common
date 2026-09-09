using System.ComponentModel.DataAnnotations.Schema;

namespace wbm_common.DataObjects
{
    public class Security_Role_Item
    {
        public class Search : SearchCreatedLast_Base
        {
            public int ID { get; set; }
            public List<int> ListRoleID { get; set; }
            public int ItemID { get; set; }

            public int? AccessLevel { get; set; }
            public enum SearchModeType
            {
                AllRecords = 2
            }
            public SearchModeType SearchMode { get; set; } = SearchModeType.AllRecords;
        }

        public class dbRow : dbRowCreatedLast_Base
        {
            public int RoleID { get; set; }

            public int ItemID { get; set; }
            public int AccessLevel { get; set; }

        }

        public class dbRowExtended : dbRow
        {
            public string? Role_PublicID { get; set; }

            public string? Item_TagID { get; set; }
            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }


        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
