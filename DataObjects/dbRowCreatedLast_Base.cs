using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace wbm_common.DataObjects
{
    public class dbRowCreatedLast_Base
    {
        public int ID { get; set; }

        [NotMapped]
        public int TemporaryID { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string CreatedMethod { get; set; }

        public int CreatedUserID { get; set; }

        public DateTime LastAmendedDateTime { get; set; }

        public string LastAmendedMethod { get; set; }

        public int LastAmendedUserID { get; set; }

        public System.Boolean DeletedFlag { get; set; }



        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj != null && obj is dbRowCreatedLast_Base)
            {
                dbRowCreatedLast_Base CompObj = obj as dbRowCreatedLast_Base;
                return (CompObj.ID == ID);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17 * (23 + ID.GetHashCode());
                return hash;
            }
        }
        #endregion
    }
}
