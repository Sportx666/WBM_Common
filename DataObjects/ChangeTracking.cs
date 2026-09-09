using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static wbm_common.DataObjects.ProcessingRule.Search;

namespace wbm_common.DataObjects
{
    public interface IChangeTracking
    {
    }

    public class ChangeTracking
    {

        public enum ChangeHeaderTableID
        {
            Security_User = 1,
            Security_User_Item = 2,
            Security_User_Role = 3,
            Security_User_RoleGroup = 4,
            Security_Item = 5,
            Security_ItemCategory = 6,
            Security_Role = 7,
            Security_Role_Item = 8,
            Security_RoleGroup = 9,
            Security_RoleGroup_Role = 10,
            ActionRateCode = 11,
            ActionType = 12,
            Carrier = 13,
            ChangeDetail = 14,
            ChangeHeader = 15,
            ChangeHeader_TableName = 16,
            Company = 17,
            ConnoteFee = 18,
            ContainerHeavyLift = 19,
            ContainerLift = 20,
            ContainerSize = 21,
            ContainerUnLoadType = 22,
            CustomerAddress = 23,
            CustomerAddressDefault = 24,
            CustomerAddressDefaultDefn = 25,
            DevanLookup = 26,
            InvoiceCycle = 27,
            InvoiceCycleCurrent = 28,
            InvoiceCycleHistory = 29,
            Location = 30,
            Log = 31,
            ManhattenFiles = 32,
            ManhattenFileType = 33,
            NoteType = 34,
            OrderDetail = 35,
            OrderHeader = 36,
            Owner = 37,
            PalletStorageType = 38,
            PaperlessDataType = 39,
            PaperlessTransactionXref = 40,
            PaperlessXrefTranslate = 41,
            PriorityOrder = 42,
            ProcessingRule = 43,
            ProcessingRuleDefn = 44,
            ProcessingRuleProcess = 45,
            ProcessingRuleTriggerCondition = 46,
            PurchaseOrderDetail = 47,
            PurchaseOrderHeader = 48,
            RateCode = 49,
            RateCollection = 50,
            RateCollectionDefn = 51,
            RateFunction = 52,
            Site = 53,
            SiteSystem = 54,
            Stock = 55,
            StockOnHand = 56,
            StockRateCategory = 57,
            StorageRateCodeFrom = 58,
            TableRate = 59,
            ToDo = 60,
            ToDo_Category = 61,
            TransactionDetail = 62,
            TransactionException = 63,
            TransactionExceptionType = 64,
            TransactionHeader = 65,
            TransactionNotes = 66,
            TransactionSource = 67,
            UnitType = 68,
            ZoneProcessing = 69,
            ProcessCategory = 70,
            Process = 71,
            AwaitingPurchOrdDW = 72,
            AwaitingPickHeadDW = 73,
            TransactionType = 74,
            LongRunProcessStatusDescription = 75,
            ToDoStatus_Type = 76,
            InvoiceHeader = 77
        }


        public enum ProcessingType
        {
            None = 0,
            DateTime = 1
        }

        public class ChangeTrackerData
        {
            public string FieldName { get; set; }
            public string PublicName { get; set; }
            public Boolean IsDecimal { get; set; }
            public ProcessingType ExtraProcessing { get; set; }

            public ChangeTrackerData(string fieldName, string publicName, Boolean IsDecimal = false, ProcessingType ExtraProcesisng = ProcessingType.None)
            {
                FieldName = fieldName;
                PublicName = publicName;
                this.IsDecimal = IsDecimal;
                this.ExtraProcessing = ExtraProcessing;
            }
        }

        public DateTime ChangedTime { get; set; }

        public string CalledFrom { get; set; }

        public List<Tuple<string, string>> Save_ValidationErrors { get; set; }


        public System.Boolean Save_IsValid()
        {
            Save_ValidationErrors = new List<Tuple<string, string>>();

            System.Boolean rtnVal = true; // assume valid

            if (string.IsNullOrEmpty(CalledFrom))
            {
                Save_ValidationErrors.Add(new Tuple<string, string>("CalledFrom", "CalledFrom must be specified."));
            }

            if (ChangedTime == DateTime.MinValue)
            {
                Save_ValidationErrors.Add(new Tuple<string, string>("ChangedTime", "ChangedTime must be specified."));
            }

            if (Save_ValidationErrors.Count > 0)
            {
                rtnVal = false;
            }

            return rtnVal;
        }


    }
}
