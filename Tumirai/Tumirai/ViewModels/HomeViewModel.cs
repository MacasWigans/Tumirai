using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumirai.Pages;
using Tumirai.Services;

namespace Tumirai.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IFirebaseService _firebaseService;
        private string _shipmentNumber;
        private string _sender;
        private string _receiver;
        private string _status;
        private bool _isLoading;
        private string _errorMessage;

        public HomeViewModel(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
            SearchCommand = new AsyncRelayCommand(SearchShipmentAsync);
        }

        public string ShipmentNumber
        {
            get => _shipmentNumber;
            set
            {
                SetProperty(ref _shipmentNumber, value);
                // When search bar is cleared, clear the tracking card
                if (string.IsNullOrWhiteSpace(value))
                {
                    ClearShipmentDetails();
                    ErrorMessage = string.Empty;
                }
            }
        }

        public string Sender
        {
            get => _sender;
            set => SetProperty(ref _sender, value);
        }

        public string Receiver
        {
            get => _receiver;
            set => SetProperty(ref _receiver, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public IAsyncRelayCommand SearchCommand { get; }

        private async Task SearchShipmentAsync()
        {
            if (string.IsNullOrWhiteSpace(ShipmentNumber))
            {
                ErrorMessage = "Please enter a shipment number";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var shipment = await _firebaseService.GetShipmentAsync(ShipmentNumber);

                if (shipment != null)
                {
                    Sender = shipment.Sender;
                    Receiver = shipment.Receiver;
                    Status = shipment.Status;
                }
                else
                {
                    ErrorMessage = "Shipment not found";
                    ClearShipmentDetails();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while searching for the shipment";
                Debug.WriteLine($"Search error: {ex.Message}");
                ClearShipmentDetails();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ClearShipmentDetails()
        {
            Sender = string.Empty;
            Receiver = string.Empty;
            Status = string.Empty;
        }


        

    }

}

