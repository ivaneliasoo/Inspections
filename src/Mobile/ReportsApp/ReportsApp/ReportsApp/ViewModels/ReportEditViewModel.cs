using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using ReportsApp.Models;
using ReportsApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace ReportsApp.ViewModels
{
    [QueryProperty("ReportId", "reportId")]
    public class ReportEditViewModel : BaseViewModel
    {
        private readonly IReportsService _reportsService;
        public ObservableRangeCollection<Signature> Signatures { get; set; }
        public ObservableRangeCollection<CheckList> CheckLists { get; set; }
        public ObservableRangeCollection<PhotoRecord> Photos { get; set; }
        public ReportEditViewModel()
        {
            _reportsService = DependencyService.Get<IReportsService>();
        }

        private int _reportId;

        public int ReportId
        {
            get { return _reportId; }
            set { _reportId = value; LoadReport(value); }
        }

        private bool _isInCameraMode;

        public bool IsInCameraMode
        {
            get { return _isInCameraMode; }
            set { SetProperty(ref _isInCameraMode, value); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private LayoutState _currentState;

        public LayoutState CurrentState
        {
            get { return _currentState; }
            set { SetProperty(ref _currentState, value); }
        }

        private async Task<Report> LoadReport(int id)
        {
            try
            {
                IsBusy = true;
                CurrentState = LayoutState.Loading;
                var result = await _reportsService.GetById(id);
                if (result is null) return null;
                Name = result.Name;
                Address = result.Address;
                Date = result.Date;
                Signatures.AddRange(result.Signatures);
                CheckLists.AddRange(result.CheckList);
                Photos.AddRange(result.PhotoRecords);
                return result;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string> { { "ReportId", "id" } });
                throw;
            }
            finally
            {
                IsBusy = false;
                CurrentState = LayoutState.None;
            }
        }
    }
}
