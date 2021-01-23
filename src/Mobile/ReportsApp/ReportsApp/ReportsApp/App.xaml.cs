using ReportsApp.Services;
using ReportsApp.Views;
using System;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using ReportsApp.Persistence;

namespace ReportsApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<AuthorizationStore>();
            DependencyService.Register<AuthenticationService>();
            DependencyService.Register<ReportsService>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=36e452df-f91f-4d89-a34d-49964a464b8c;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
