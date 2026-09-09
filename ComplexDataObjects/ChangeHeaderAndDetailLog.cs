using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wbm_common.DataObjects;

namespace wbm_common.ComplexDataObjects
{
    public class ChangeHeaderAndDetailLog
    {
        public int RecordID { get; set; }

        public ChangeTracking.ChangeHeaderTableID ChangeHeaderTableID { get; set; }

        public class ChangeEvent
        {
            public ChangeHeader.dbRowExtended ChangeHeader { get; set; }

            public List<ChangeDetail.dbRow> ListChangeDetail { get; set; }
        }

        public List<ChangeEvent> ListChangeEvent { get; set; }
    }
}
