using System;

namespace PropertyAgencyDesktopApp.Services
{
    public class LevenshteinWordIndefiniteSearcher : IWordIndefiniteSearcher
    {
        public int Calculate(string firstString, string secondString)
        {
            if (firstString.Length == 0)
            {
                return secondString.Length;
            }
            if (secondString.Length == 0)
            {
                return firstString.Length;
            }
            if (firstString[0] == secondString[0])
            {
                string firstStringSub = firstString.Substring(1);
                string secondStringSub = secondString.Substring(1);
                return Calculate(firstStringSub,
                                 secondStringSub);
            }
            int firstLevenshteinResult = Calculate(firstString.Substring(1),
                                                   secondString);
            int secondLevenshteinResult = Calculate(firstString,
                                                 secondString.Substring(1));
            int thirdLevenshteinResult = Calculate(firstString.Substring(1),
                                                 secondString.Substring(1));
            int additionalMinimalDistance = Math.Min(secondLevenshteinResult,
                                                     thirdLevenshteinResult);
            return 1 + Math.Min(firstLevenshteinResult, additionalMinimalDistance);
        }
    }
}
