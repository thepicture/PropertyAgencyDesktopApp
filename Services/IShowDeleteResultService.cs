namespace PropertyAgencyDesktopApp.Services
{
    /// <summary>
    /// Defines methods to get a template when an instance of a class 
    /// is deleting.
    /// </summary>
    public interface IShowDeleteResultService
    {
        /// <summary>
        /// Gets a template when related objects of a deleting instance 
        /// were found and it's impossible to delete in the current state.
        /// </summary>
        /// <param name="deletingInstance">The instance to delete.</param>
        /// <param name="relatedObjects">The related objects.</param>
        /// <returns>The template which describes a reason.</returns>
        string OnRelatedObjectsError(string deletingInstance,
                                     params string[] relatedObjects);
        /// <summary>
        /// Gets a template when deleting is starting 
        /// but not have finished yet.
        /// </summary>
        /// <param name="deletingInstance">The instance to delete.</param>
        /// <returns>The template which describes a reason.</returns>
        string OnDelete(string deletingInstance);
        /// <summary>
        /// Gets a template when delete has successfully finished.
        /// </summary>
        /// <param name="deletingInstance">The instance to delete.</param>
        /// <returns>The template which describes a reason.</returns>
        string OnSuccessfulDelete(string deletingInstance);
        /// <summary>
        /// Gets a template descibing a reasonable delete error.
        /// </summary>
        /// <param name="deletingInstance">The instance to delete.</param>
        /// <returns>The template which describes a reason.</returns>
        string OnCommonDeleteError(string deletingInstance);
        /// <summary>
        /// Gets a template describing a generic delete error.
        /// </summary>
        /// <param name="deletingInstance">The instance to delete.</param>
        /// <returns>The template which describes a reason.</returns>
        string OnFatalDeleteError(string deletingInstance);
    }
}
