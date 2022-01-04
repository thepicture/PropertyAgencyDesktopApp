namespace PropertyAgencyDesktopApp.Services
{
    public class PropertyAgencyShowDeleteResultService
        : IShowDeleteResultService
    {
        public string OnCommonDeleteError(string deletingInstance)
        {
            return $"Can't delete the {deletingInstance}. "
                   + $"Try to go back and to the {deletingInstance} again,"
                   + "or reload the app if it doesn't help";
        }

        public string OnDelete(string deletingInstance)
        {
            return "Do you really want "
                   + $"to delete the {deletingInstance}?";
        }

        public string OnFatalDeleteError(string deletingInstance)
        {
            return $"Can't delete the {deletingInstance}. "
                   + "Fatal error encountered. "
                   + "Reload the app and try again";
        }

        public string OnRelatedObjectsError(string deletingInstance,
                                            params string[] relatedObjects)
        {
            return $"Can't delete the {deletingInstance} with the "
                   + $"following objects: {string.Join(",", relatedObjects)} "
                   + $"related to it";
        }

        public string OnSuccessfulDelete(string deletingInstance)
        {
            return $"The {deletingInstance} was successfully deleted!";
        }
    }
}
