namespace wbm_common.DataObjects
{
    public class Security_User_Password
    { 
        public class Search : SearchCreatedLast_Base
        {
            public int ID { get; set; }
            public int UserID { get; set; }

            public string Password { get; set; }
        }

        public class dbRow : dbRowCreatedLast_Base
        {
            public int UserID { get; set; }
            public string Password { get; set; }
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
