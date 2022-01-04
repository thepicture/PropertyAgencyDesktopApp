namespace PropertyAgencyDesktopApp.Services
{
    public class PropertyAgencyShowSaveResultService : IShowSaveResultService
    {
        public string GetOnCommonErrorTemplate(string malformedInstance)
        {
            return $"Can't save the {malformedInstance}. "
                   + $"Try to go back and to the {malformedInstance} "
                   + "again, "
                   + "or reload the app if it doesn't help";
        }

        public string GetOnFatalErrorTemplate(string malformedInstance)
        {
            return $"Can't save the {malformedInstance}. "
                   + "Fatal error encountered. "
                   + "Reload the app and try again";
        }

        public string GetOnSavedTemplate(string malformedInstance)
        {
            return $"The {malformedInstance} was successfully saved!";
        }
    }
}
