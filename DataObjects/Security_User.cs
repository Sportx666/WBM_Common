using System.ComponentModel.DataAnnotations.Schema;

namespace wbm_common.DataObjects
{
    public class Security_User
    {
        public class Search : SearchCreatedLast_Base
        {
            public int ID { get; set; }

            public string PublicID { get; set; }

            public string EmailAddress { get; set; }

            public string Firstname { get; set; }

            public string Surname { get; set; }
            public List<int> ListStartCompany { get; set; }

            public Boolean? Active { get; set; }
            public enum SearchModeType
            {
                AllRecords = 2
            }
            public SearchModeType SearchMode { get; set; } = SearchModeType.AllRecords;
        }

        public class dbRow : dbRowCreatedLast_Base
        {
            public string PublicID { get; set; }

            public string EmailAddress { get; set; }

            public string Firstname { get; set; }

            public string Surname { get; set; }
            public int StartCompany { get; set; }

            public Boolean Active { get; set; }

        }

        public class dbRowExtended : dbRow
        {
            public string? Company_ShortName { get; set; }
            public string? Created_UserPublicID { get; set; }

        }
        public dbRowExtended dbRowExtended_instance { get; set; }

        public class SearchResult
        {
            public List<dbRow> ListResult { get; set; }
        }
    }
}
