namespace wbm_common.DataObjects
{
    public class Security_RoleGroup
    {
        public class Search : SearchCreatedLast_Base
        {
            public int ID { get; set; }

            public string PublicID { get; set; }

            public string Description { get; set; }
            public enum SearchModeType
            {
                AllRecords = 2
            }
            public SearchModeType SearchMode { get; set; } = SearchModeType.AllRecords;
        }

        public class dbRow : dbRowCreatedLast_Base
        {
            public string PublicID { get; set; }
            public string Description { get; set; }
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
    }
}
