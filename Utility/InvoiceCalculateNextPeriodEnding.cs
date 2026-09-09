using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wbm_common.DataObjects;

namespace wbm_common.Utility
{
    public static class InvoiceCalculateNextPeriodEnding
    {
        public static DateTime? SetCloseDate(InvoiceCycleCurrent.dbRow ICCRow)
        {
            DateTime? nextCloseDate = null;

            switch (ICCRow.InvoiceCycleID)
            {
                case 1:
                    // Adhoc, manual don't set
                    break;
                case 2:
                    // Daily
                    nextCloseDate = ICCRow.CurrentPeriodEnding.AddDays(1);
                    break;
                case 3:
                    // Weekly
                    nextCloseDate = ICCRow.CurrentPeriodEnding.AddDays(7);
                    break;
                case 4:
                    // Fortnightly
                    nextCloseDate = ICCRow.CurrentPeriodEnding.AddDays(14);
                    break;
                case 5:
                    // 1/2 monthly - either 15th or end of month date
                    if (ICCRow.CurrentPeriodEnding.Day == 15)
                    {
                        // Set to current end of month
                        nextCloseDate = CalculateCurrentEndOfMonth(ICCRow.CurrentPeriodEnding);
                    }
                    else
                    {
                        // Set to 15th of next month
                        nextCloseDate = CalculateCurrentEndOfMonth(ICCRow.CurrentPeriodEnding).AddDays(15);
                    }
                    break;
                case 6:
                    // Monthly
                    nextCloseDate = CalculateNextEndOfMonth(ICCRow.CurrentPeriodEnding);
                    break;
            }

            return nextCloseDate;
        }

        public static DateTime CalculateNextEndOfMonth(DateTime SourceDate)
        {
            return CalculateFirstDayOfMonth(SourceDate.AddMonths(2)).AddDays(-1);
        }

        public static DateTime CalculateCurrentEndOfMonth(DateTime SourceDate)
        {
            return CalculateFirstDayOfMonth(SourceDate.AddMonths(1)).AddDays(-1);
        }

        public static DateTime CalculatePreviousEndOfMonth(DateTime SourceDate)
        {
            return CalculateFirstDayOfMonth(SourceDate).AddDays(-1);
        }

        public static DateTime CalculateFirstDayOfMonth(DateTime SourceDate)
        {
            DateTime WorkingDate = SourceDate.Date;
            WorkingDate = WorkingDate.AddDays(0 - (WorkingDate.Day - 1));

            return WorkingDate;
        }
    }
}
