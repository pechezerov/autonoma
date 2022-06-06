using Autonoma.App.ViewModels;
using Xamarin.Forms;

namespace Autonoma.App.Views
{
    public abstract class ContentPageBase : ContentPage
    {
        public ContentPageBase()
        {
            NavigationPage.SetBackButtonTitle(this, string.Empty);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is BaseViewModel vmb)
            {
                if (!vmb.IsInitialized || vmb.MultipleInitialization)
                {
                    await vmb.InitializeAsync(null);
                }
            }
        }
    }
}
