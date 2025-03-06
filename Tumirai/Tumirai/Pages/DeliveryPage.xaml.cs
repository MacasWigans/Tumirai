using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Devices.Sensors;

namespace Tumirai.Pages;

public partial class DeliveryPage : ContentPage
{
	public DeliveryPage()
	{
		InitializeComponent();
	}
    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        try
        {
            // Get Current Location
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
                location = await Geolocation.GetLocationAsync();

            if (location == null)
            {
                await DisplayAlert("Error", "Unable to get current location", "OK");
                return;
            }

            // Get Destination Address from the Entry field
            string address = FloorEntry.Text;
            if (string.IsNullOrWhiteSpace(address))
            {
                await DisplayAlert("Error", "Please enter a floor or unit number", "OK");
                return;
            }

            // Convert Address to Coordinates
            var locations = await Geocoding.GetLocationsAsync(address);
            var destination = locations?.FirstOrDefault();

            if (destination == null)
            {
                await DisplayAlert("Error", "Unable to find location for the entered address", "OK");
                return;
            }

            // Clear Previous Elements (Polyline & Pins)
            DeliveryMap.MapElements.Clear();
            DeliveryMap.Pins.Clear();

            // Draw Route (Polyline)
            var polyline = new Polyline
            {
                StrokeWidth = 5,
                StrokeColor = Colors.Blue
            };
            polyline.Geopath.Add(new Location(location.Latitude, location.Longitude)); // Current Location
            polyline.Geopath.Add(new Location(destination.Latitude, destination.Longitude)); // Destination

            // Add Polyline to Map
            DeliveryMap.MapElements.Add(polyline);

            // Add Red Dot (Pin) at Destination
            var destinationPin = new Pin
            {
                Label = "Delivery Address",
                Address = address,
                Location = new Location(destination.Latitude, destination.Longitude),
                Type = PinType.Place
            };

            DeliveryMap.Pins.Add(destinationPin);

            // Center Map on the Route
            var midLatitude = (location.Latitude + destination.Latitude) / 2;
            var midLongitude = (location.Longitude + destination.Longitude) / 2;
            DeliveryMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(midLatitude, midLongitude), Distance.FromKilometers(1)));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        ProceedButton.IsVisible = true;
    }
    private async void OnProceedClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductDetails());
        //await Shell.Current.GoToAsync("//Product");   
    }

}
