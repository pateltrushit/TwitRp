using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TwitRp.Core.Helpers.RegularExpressions
{
    public class RegexHelper
    {
        /// <summary>
        /// Get sing Match Value from Input string
        /// </summary>
        /// <param name="regexPattern"></param>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string GetMatchValue(string inputString, string regexPattern)
        {
            string match = null;
            if (!string.IsNullOrEmpty(regexPattern) && !string.IsNullOrEmpty(inputString))
            {
                Regex rgx = new Regex(regexPattern);
                var result = rgx.Match(inputString);
                if (result.Success)
                {
                    match = result.Value;
                }
            }
            return match;
        }

        /// <summary>
        /// Gets List Of Matches found in Input String
        /// </summary>
        /// <param name="regexPattern">Regex Pattern</param>
        /// <param name="inputString">Input String</param>
        /// <returns></returns>
        public static List<string> GetListOfMatchValues(string inputString, string regexPattern)
        {
            List<string> listOfMatches = new List<string>();
            if (!string.IsNullOrEmpty(regexPattern) && !string.IsNullOrEmpty(inputString))
            {
                Regex rgx = new Regex(regexPattern);
                foreach (Match match in rgx.Matches(inputString))
                {
                    listOfMatches.Add(match.Value);
                }
            }
            return listOfMatches;
        }
    }
}
