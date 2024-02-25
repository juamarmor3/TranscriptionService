using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TranscriptionService.models.validators
{
    public class ValidatorUtils
    {
        private static string SIZE_PATTERN = @"\(\d+(k|K|m|M)\)";
        private static string NUMBER_PATTERN = @"\d+";
        private static string BYTES_PATTERN = @"(k|K|m|M)";
        private static int defaultMinSize = 50000;
        private static int defaultMaxSize = 3000000;
        public static int ParseMinSize(string value)
        {

            MatchCollection coincidences = Regex.Matches(value, SIZE_PATTERN);
            if (coincidences.Count != 1)
            {
                return defaultMinSize;
            }
            string numberOfBytes = coincidences[0].Value;
            int count;
            if (!int.TryParse(Regex.Match(numberOfBytes, NUMBER_PATTERN).Value, out count))
            {
                return defaultMinSize;
            }
            return GetTotalBytes(numberOfBytes, count);

        }

        public static int ParseMaxSize(string value)
        {

            MatchCollection coincidences = Regex.Matches(value, SIZE_PATTERN);
            if (coincidences.Count != 1)
            {
                return defaultMaxSize;
            }
            string numberOfBytes = coincidences[0].Value;

            int count;
            if (!int.TryParse(Regex.Match(numberOfBytes, NUMBER_PATTERN).Value, out count))
            {
                return defaultMaxSize;
            }
            return GetTotalBytes(numberOfBytes, count);

        }

        private static int GetTotalBytes(string numberOfBytes, int count)
        {
            string units = Regex.Match(numberOfBytes, BYTES_PATTERN).Value;
            int coeficient = (units == "M" || units == "m") ? 1000000 : 1000;

            return coeficient * count;
        }
    }
}
