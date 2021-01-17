using ReportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReportsApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportEditPage : ContentPage
    {
        public ReportEditPage()
        {
            InitializeComponent();
            BindingContext = new ReportEditViewModel();
        }

        private void CameraView_MediaCaptured(object sender, Xamarin.CommunityToolkit.UI.Views.MediaCapturedEventArgs e)
        {

        }

        private void CameraView_OnAvailable(object sender, bool e)
        {

        }

        private void CameraView_OnAvailable_1(object sender, bool e)
        {

        }
    }
}