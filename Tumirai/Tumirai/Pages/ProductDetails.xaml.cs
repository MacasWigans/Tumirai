namespace Tumirai.Pages;

public partial class ProductDetails : ContentPage
{
    public ProductDetails()
    {
        InitializeComponent();

    }
    private Dictionary<string, double> transportRates = new Dictionary<string, double>
{
    { "Motorcar ($0.75/kg)", 0.75 },
    { "Truck ($1.50/kg)", 1.50 },
    { "Motorbike ($0.50/kg)", 0.50 }
};

    private void OnWeightChanged(object sender, TextChangedEventArgs e)
    {
        CalculatePrice();
    }

    private void OnTransportSelected(object sender, EventArgs e)
    {
        CalculatePrice();
    }

    private void OnDeliveryTypeChanged(object sender, CheckedChangedEventArgs e)
    {
        ExpressChargeLabel.IsVisible = ExpressDelivery.IsChecked;
        CalculatePrice();
    }

    private void CalculatePrice()
    {
        if (!double.TryParse(ApproxWeight.Text, out double weight) || weight <= 0 || TransportPicker.SelectedIndex == -1)
        {
            PriceLabel.Text = "$0.00";
            NextButton.IsEnabled = false;
            return;
        }

        string selectedTransport = TransportPicker.SelectedItem.ToString();
        double baseRate = transportRates[selectedTransport];
        double price = baseRate * weight;

        // Apply Express Delivery Charge (10% extra)
        if (ExpressDelivery.IsChecked)
        {
            price *= 1.1;
        }

        PriceLabel.Text = $"${price:F2}";
        NextButton.IsEnabled = true;
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        // Navigate to next page or process order
    }

}