using System.Windows.Input;
using Tumirai.Pages;

namespace Tumirai
{
    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();
        public ICommand HelpCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            BindingContext = this;
        }
        void RegisterRoutes()
        {
            Routes.Add("homepage", typeof(HomePage));
            Routes.Add("profilepage", typeof(ProfilePage));
            Routes.Add("shipmentpage", typeof(ShipmentPage));
           

            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}
