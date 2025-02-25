namespace Tumirai.Pages
{
    public partial class ShipmentPage : ContentPage
    {
        public ShipmentPage()
        {
            InitializeComponent();
        }

        private void OnNotificationTapped(object sender, EventArgs e)
        {
            // Handle notification tap event
            DisplayAlert("Notification", "You tapped on the notification icon.", "OK");
        }
    }
}
