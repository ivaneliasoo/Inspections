using Microsoft.AppCenter.Analytics;
using MvvmHelpers;
using ReportsApp.Services.Secutiry;
using ReportsApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ReportsApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthenticationService authService;

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }


        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            authService = new AuthenticationService();

            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(ReportOptions)}");
            try
            {
                await authService.Login(UserName, Password, string.Empty);
                await Application.Current.MainPage.DisplayAlert("Success", "Welcome Mr. Stark", "OK");
                await Shell.Current.GoToAsync($"//{nameof(ReportOptions)}");
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("LOGIN", new Dictionary<string, string> {
                    { nameof(UserName), UserName },
                    { nameof(Password), Password }
                });

                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }

        }
    }
}
