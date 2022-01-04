namespace PropertyAgencyDesktopApp.Services
{
    /// <summary>
    /// Defines methods to get a string template about errors 
    /// during saving an instance of a class.
    /// </summary>
    public interface IShowSaveResultService
    {
        /// <summary>
        /// Returns a template describing a reasonable thrown exception.
        /// </summary>
        /// <param name="malformedInstance">The instance to save.</param>
        /// <returns>A template describing the error.</returns>
        string GetOnCommonErrorTemplate(string malformedInstance);
        /// <summary>
        /// Returns a template describing a generic thrown exception.
        /// </summary>
        /// <param name="malformedInstance">The instance to save.</param>
        /// <returns>A template describing the error.</returns>
        string GetOnFatalErrorTemplate(string malformedInstance);
        /// <summary>
        /// Returns a template describing a successful saving.
        /// </summary>
        /// <param name="malformedInstance">The saved instance.</param>
        /// <returns>A template describing a successful saving</returns>
        string GetOnSavedTemplate(string malformedInstance);
    }
}
