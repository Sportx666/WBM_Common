using System.ComponentModel.DataAnnotations.Schema;

namespace wbm_common.DataObjects
{
    public class Security_User_Role
    {
        public class Search : SearchCreatedLast_Base
        {
            public int ID { get; set; }
            public int UserID { get; set; }
            public List<int> ListCompanyID { get; set; }
            public int RoleID { get; set; }

            public List<int> ListAccessLevel { get; set; }
            public enum SearchModeType
            {
                AllRecords = 2
            }
            public SearchModeType SearchMode { get; set; } = SearchModeType.AllRecords;
        }

        public class dbRow : dbRowCreatedLast_Base
        {
            public int UserID { get; set; }
            public int CompanyID { get; set; }
            public int RoleID { get; set; }

            public int AccessLevel { get; set; }

        }

        public class dbRowExtended : dbRow
        {
            public string? User_PublicID { get; set; }
            public string? Company_ShortName { get; set; }
            public string? Role_PublicID { get; set; }
            public string? Created_UserPublicID { get; set; }
        }

        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
