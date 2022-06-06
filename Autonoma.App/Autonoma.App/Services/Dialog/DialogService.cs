using System.Threading.Tasks;

namespace Autonoma.App.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return App.Current.MainPage.DisplayAlert(title, message, buttonLabel);
        }
    }
}
