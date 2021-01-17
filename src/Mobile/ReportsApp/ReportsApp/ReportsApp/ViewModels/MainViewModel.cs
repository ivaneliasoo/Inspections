using MvvmHelpers;
using MvvmHelpers.Commands;
using ReportsApp.Services.Secutiry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ReportsApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly AuthenticationService authService;

        public MainViewModel()
        {
            authService = new AuthenticationService();
            LogoutCommand = new MvvmHelpers.Commands.Command(ExecuteLogout);
        }

        private void ExecuteLogout(object obj)
        {
            authService.Logout();
            Shell.Current.GoToAsync("//LoginPage");
        }

        public MvvmHelpers.Commands.Command LogoutCommand { get; }


    }
}
