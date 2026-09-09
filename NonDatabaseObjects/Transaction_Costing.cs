using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wbm_common.DataObjects;

namespace wbm_common.NonDatabaseObjects
{
    public class Transaction_Costing
    {
        public TransactionHeader.dbRow thRow { get; set; }

        public Boolean CostedOK { get; set; }   
        public string ErrorMessage { get; set; }

        public class TransactionDetail_Costing
        {
            public TransactionDetail.dbRow tdRow { get; set; }
            public Boolean CostedOK { get; set; }
            public string ErrorMessage { get; set; }
        }
        public List<TransactionDetail_Costing> ListTransactionDetail_Costing { get; set; }

        public Boolean IsValid()
        {
            return true;
        }
        public List<Tuple<string, string>> ValidationErrors { get; set; }


        public Transaction_Costing()
        {
            this.ValidationErrors = new List<Tuple<string, string>>();
        }
    }

    public class Transaction_CostingHolder
    {
        public List<Transaction_Costing> ListTransactionCosting { get; set; }

        public Boolean CostedOK { get; set; }
        public string ErrorMessage { get; set; }

        public Transaction_CostingHolder()
        {
            this.ListTransactionCosting = new List<Transaction_Costing>();
        }

        public Transaction_CostingHolder(Transaction_Costing tc)
        {
            this.ListTransactionCosting = new List<Transaction_Costing>();
            this.ListTransactionCosting.Add(tc);
        }

    }
}
