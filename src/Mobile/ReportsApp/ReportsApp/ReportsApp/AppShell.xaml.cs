using ReportsApp.ViewModels;
using ReportsApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ReportsApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ReportEdit", typeof(ReportEditPage));
        }
    }
}
