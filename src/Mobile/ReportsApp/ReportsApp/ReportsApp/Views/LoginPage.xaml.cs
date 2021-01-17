using ReportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Extensions;
using ReportsApp.Services.Secutiry;

namespace ReportsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly AuthenticationService _authService;

        public LoginPage()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var result = await _authService.IsLoggedIn();
            if(result)
                await Shell.Current.GoToAsync("//Reports");
        }

        private void LoginViewModel_OnLoginFailed(object sender, LoginFailedEventArgs e)
        {
            this.DisplayToastAsync(e.Message);
        }
    }
}