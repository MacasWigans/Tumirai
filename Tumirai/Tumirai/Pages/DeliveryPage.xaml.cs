using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Devices.Sensors;
using System.Text.Json;

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

            // Get Destination Address
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

            // Fetch Route from Google Maps API
            var routeCoordinates = await GetRouteFromGoogleMaps(location, destination);
            if (routeCoordinates == null || routeCoordinates.Count == 0)
            {
                await DisplayAlert("Error", "Failed to fetch route", "OK");
                return;
            }

            // Clear Previous Elements (Polyline & Pins)
            DeliveryMap.MapElements.Clear();
            DeliveryMap.Pins.Clear();

            // Draw Road Path (Polyline)
            var polyline = new Polyline
            {
                StrokeWidth = 5,
                StrokeColor = Colors.Blue
            };

            foreach (var point in routeCoordinates)
            {
                polyline.Geopath.Add(point);
            }

            DeliveryMap.MapElements.Add(polyline);

            // Add Pin at Destination
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
    private async Task<List<Location>> GetRouteFromGoogleMaps(Location start, Location end)
    {
        string apiKey = "AIzaSyDm9oxdmHF_-B5rPg-XNSUTyXO44cV79Uk";
        string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={start.Latitude},{start.Longitude}&destination={end.Latitude},{end.Longitude}&mode=driving&key={apiKey}";

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetStringAsync(url);
            var jsonResponse = JsonDocument.Parse(response);

            var routeCoordinates = new List<Location>();

            if (jsonResponse.RootElement.TryGetProperty("routes", out var routes) && routes.GetArrayLength() > 0)
            {
                var legs = routes[0].GetProperty("legs");
                var steps = legs[0].GetProperty("steps");

                foreach (var step in steps.EnumerateArray())
                {
                    var startLoc = step.GetProperty("start_location");
                    var endLoc = step.GetProperty("end_location");

                    routeCoordinates.Add(new Location(startLoc.GetProperty("lat").GetDouble(), startLoc.GetProperty("lng").GetDouble()));
                    routeCoordinates.Add(new Location(endLoc.GetProperty("lat").GetDouble(), endLoc.GetProperty("lng").GetDouble()));
                }
            }

            return routeCoordinates;
        }
    }

    private async void OnProceedClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductDetails());
        //await Shell.Current.GoToAsync("//Product");   
    }

}
