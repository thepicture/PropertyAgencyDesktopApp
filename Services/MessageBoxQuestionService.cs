using System.Windows;

namespace PropertyAgencyDesktopApp.Services
{
    public class MessageBoxQuestionService : IQuestionService
    {
        public bool Ask(string message)
        {
            return MessageBox.Show(
                message,
                "Question",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
