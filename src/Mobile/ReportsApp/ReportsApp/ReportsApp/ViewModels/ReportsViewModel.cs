using MvvmHelpers;
using ReportsApp.Models;
using ReportsApp.Services;
using ReportsApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ReportsApp.ViewModels
{
    public class ReportsViewModel : BaseViewModel
    {
        private ReportsService _reportsService;
        public ObservableCollection<Report> Reports { get; set; }


        private string searchText;

        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); }
        }

        private Report _selectedReport;

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set { SetProperty(ref _selectedReport, value); }
        }

        public ReportsViewModel()
        {
            Title = "Reports";
            _reportsService = new ReportsService();
            Reports = new ObservableCollection<Report>();
            GetReportsAsyncCommand = new Command(ExcecuteGetReportsCommand);
            SelectedReportCommand = new Command(ExecuteSelectedReportCommand);
            PrintReportCommand = new Command(ExecutePrintReportCommand);
            DeleteReportCommand = new Command(ExecuteDeleteReportCommand);
            CompleteReportCommand = new Command(ExecuteCompleteReportCommand);
            GetReportsAsyncCommand.Execute(null);
        }

        private async void ExecutePrintReportCommand(object obj)
        {
            await Application.Current.MainPage.DisplayActionSheet("The Report xxx has been Printed", "Cancel", "Dismiss", "Undo");
        }

        private async void ExecuteCompleteReportCommand(object obj)
        {
            await Application.Current.MainPage.DisplayActionSheet("The Report xxx has been Completed", "Cancel", "Dismiss", "Undo");
        }

        private async void ExecuteDeleteReportCommand(object obj)
        {
            await Application.Current.MainPage.DisplayActionSheet("The Report xxx has been Deleted", "Cancel", "Dismiss", "Undo");
        }

        private async void ExecuteSelectedReportCommand(object obj)
        {
            await Shell.Current.GoToAsync("ReportEdit", true);
        }

        private async void ExcecuteGetReportsCommand(object obj)
        {
            try
            {
                IsBusy = true;
                var result = await _reportsService.GetReportsByFilter(SearchText, null);
                Reports.Clear();

                foreach (var report in result)
                    Reports.Add(report);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command GetReportsAsyncCommand { get; }
        public Command SelectedReportCommand { get; }
        public Command PrintReportCommand { get; }
        public Command DeleteReportCommand { get; }
        public Command CompleteReportCommand { get; }
    }
}
