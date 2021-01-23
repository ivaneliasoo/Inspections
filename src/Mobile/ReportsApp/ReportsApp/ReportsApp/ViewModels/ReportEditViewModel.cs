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
        public ObservableRangeCollection<GroupedChecks> CheckLists { get; set; }
        public ObservableRangeCollection<PhotoRecord> Photos { get; set; }
        public ObservableRangeCollection<AddressDTO> AddressList { get; set; }
        public ReportEditViewModel()
        {
            _reportsService = DependencyService.Get<IReportsService>();
            GetAddressesCommand = new Command(ExcecuteGetAddresses);
            SelectedAddressChangedCommand = new Command(ExecuteSelectedAddressChangedCommand);
            Signatures = new ObservableRangeCollection<Signature>();
            Photos = new ObservableRangeCollection<PhotoRecord>();
            CheckLists = new ObservableRangeCollection<GroupedChecks>();
            AddressList = new ObservableRangeCollection<AddressDTO>();
            GetAddressesCommand.Execute(null);
        }
        
        private AddressDTO _selectedAddress;
        public AddressDTO SelectedAddress { get => _selectedAddress; set => SetProperty(ref _selectedAddress, value); }

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

        private string _reportTitle;

        public string ReportTitle
        {
            get { return _reportTitle; }
            set { SetProperty(ref _reportTitle, value); }
        }
        private string _formName;

        public string FormName
        {
            get { return _formName; }
            set { SetProperty(ref _formName, value); }
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

        private string _license;

        public string License
        {
            get { return _license; }
            set { SetProperty(ref _license, value); }
        }

        private bool _isClosed;

        public bool IsClosed
        {
            get { return _isClosed; }
            set { SetProperty(ref _isClosed, value); }
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
                FormName = result.FormName;
                ReportTitle = result.Title;
                Address = result.Address;
                Date = result.Date;
                Signatures.Clear();
                Signatures.AddRange(result.Signatures);
                CheckLists.Clear();
                foreach (var ck in result.CheckList)
                    CheckLists.Add(new GroupedChecks(ck.reportId,ck.id,ck.completed,ck.@checked,ck.checks,ck.text));

                Photos.Clear();
                Photos.AddRange(result.PhotoRecords);
                IsClosed = result.IsClosed;
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


        private async void ExcecuteGetAddresses()
        {
            try
            {
                var result = await _reportsService.GetAddressByFilter("");
                AddressList.AddRange(result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        public Command GetAddressesCommand { get; }

        private async void ExecuteSelectedAddressChangedCommand()
        {
            this.Address = SelectedAddress.FormatedAddress;
            License = SelectedAddress.Number;
        }

        public Command SelectedAddressChangedCommand { get; }

    }
}
