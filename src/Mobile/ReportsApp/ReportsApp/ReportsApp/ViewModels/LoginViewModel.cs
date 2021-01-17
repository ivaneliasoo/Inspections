using Microsoft.AppCenter.Analytics;
using MvvmHelpers;
using ReportsApp.Services.Secutiry;
using ReportsApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace ReportsApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService authService;

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


        private LayoutState _currentState;

        public LayoutState CurrentState
        {
            get { return _currentState; }
            set { SetProperty(ref _currentState, value); }
        }


        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            authService = DependencyService.Get<IAuthenticationService>();
            CurrentState = LayoutState.None;
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                IsBusy = true;
                CurrentState = LayoutState.Loading;
                await authService.Login(UserName, Password, string.Empty);
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync("//Reports");
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("LOGIN", new Dictionary<string, string> {
                    { nameof(UserName), UserName },
                    { nameof(Password), Password }
                });

                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                OnLoginFailed?.Invoke(this, new LoginFailedEventArgs(ex.Message));
            }
            finally
            {
                CurrentState = LayoutState.None;
                IsBusy = false;
            }

        }
        public event EventHandler<LoginFailedEventArgs> OnLoginFailed;
    }

    public class LoginFailedEventArgs : EventArgs
    {
        public string Message { get; }
        public LoginFailedEventArgs(string message)
        {
            Message = message;
        }
    }
}
