using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wbm_common.DataObjects;

namespace wbm_common.NonDatabaseObjects
{
    public class PreInvoiceGenerateStats
    {
        public int CompanyID { get; set; }

        public class DollarCount
        {
            public int Count { get; set; }
            public Decimal Amount { get; set; }

            public DollarCount()
            {
                Count = 0;
                Amount = 0;
            }

            public DollarCount(int count, Decimal amount)
            {
                Count = count;
                Amount = amount;
            }
        }

        public List<InvoiceCycleCurrent.dbRow> ListInvoiceCycleCurrent { get; set; }

        public string ErrorMessage { get; set; }

        public class OwnerStat
        {
            public int OwnerID { get; set; }

            public string SourceOwnerID { get; set; }

            public string OwnerName { get; set; }

            public DateTime? LastInvoiceDate { get; set; }

            public int InvoiceCycleID { get; set; }

            public string InvoiceCycleName { get; set; }

            public Dictionary<string, DollarCount> Stats { get; set; }

            public void AddStat(string statName, int count, Decimal amount)
            {
                if (Stats == null)
                {
                    Stats = new Dictionary<string, DollarCount>();
                }
                if (Stats.ContainsKey(statName))
                {
                    Stats[statName].Count += count;
                    Stats[statName].Amount += amount;
                }
                else
                {
                    Stats[statName] = new DollarCount(count, amount);
                }
            }
        }

        public List<OwnerStat> ListOwnerStats { get; set; }


        public PreInvoiceGenerateStats()
        {
            this.ListOwnerStats = new List<OwnerStat>();
        }
    }
}
