namespace wbm_common.Utility
{
    public static class DataObjectValidator
    {
        public static System.Boolean DateMinMax(string DataName, DateTime? date_min, DateTime? date_max, out List<Tuple<string, string>> ValidationErrors)
        {
            System.Boolean rtnVal = true; // assume valid
            ValidationErrors = new List<Tuple<string, string>>();

            if (date_min.HasValue)
            {
                if (date_max.HasValue)
                {
                    if (date_min > date_max)
                    {
                        rtnVal = false;
                        ValidationErrors.Add(new Tuple<string, string>(DataName, "'Min' value must be less than 'Max' value."));
                    }
                }
            }
            return rtnVal;
        }

        public static System.Boolean DecimalMinMax(string DataName, Decimal? dec_min, Decimal? dec_max, out List<Tuple<string, string>> ValidationErrors)
        {
            System.Boolean rtnVal = true; // assume valid
            ValidationErrors = new List<Tuple<string, string>>();

            if (dec_min.HasValue && dec_max.HasValue)
            {
                if (dec_min > dec_max)
                {
                    rtnVal = false;
                    ValidationErrors.Add(new Tuple<string, string>(DataName, "'Min' value must be less than 'Max' value."));
                }
            }
            return rtnVal;
        }

        public static System.Boolean IntMinMax(string DataName, int? int_min, int? int_max, out List<Tuple<string, string>> ValidationErrors)
        {
            System.Boolean rtnVal = true; // assume valid
            ValidationErrors = new List<Tuple<string, string>>();

            if (int_min.HasValue && int_max.HasValue)
            {
                if (int_min > int_max)
                {
                    rtnVal = false;
                    ValidationErrors.Add(new Tuple<string, string>(DataName, "'Min' value must be less than 'Max' value."));
                }

            }
            return rtnVal;
        }


        public static System.Boolean IntGreaterZero(string DataName, int? int_value, out List<Tuple<string, string>> ValidationErrors)
        {
            System.Boolean rtnVal = true; // assume valid
            ValidationErrors = new List<Tuple<string, string>>();

            if (int_value.HasValue)
            {
                if (int_value <= 0)
                {
                    rtnVal = false;
                    ValidationErrors.Add(new Tuple<string, string>(DataName, "Must be greater than zero"));
                }
            }
            return rtnVal;
        }


    }
}
