namespace PropertyAgencyDesktopApp.Services
{
    /// <summary>
    /// Defines a method for the indefinite searching, 
    /// e.g. by the Levenshtein distance.
    /// </summary>
    public interface IWordIndefiniteSearcher
    {
        /// <summary>
        /// Searches the distance between two strings.
        /// </summary>
        /// <param name="firstString">The first string.</param>
        /// <param name="secondString">The second string.</param>
        /// <returns>The calculated distance.</returns>
        int Calculate(string firstString, string secondString);
    }
}
