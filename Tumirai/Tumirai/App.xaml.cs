using Tumirai.Pages;

namespace Tumirai
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Routing.RegisterRoute("delivery", typeof(DeliveryPage));

            //MainPage = new AppShell();



        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}