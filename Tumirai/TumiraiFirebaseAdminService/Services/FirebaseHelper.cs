using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TumiraiFirebaseAdminService.Services
{
    public class FirebaseHelper
    {
        private readonly FirebaseClient firebase = new FirebaseClient("https://tumirei-default-rtdb.firebaseio.com/");

        public async Task<Shipment> GetShipmentDetails(string shipmentNumber)
        {
            var shipment = await firebase
                .Child("shipments")
                .Child(shipmentNumber)
                .OnceSingleAsync<Shipment>();

            return shipment;
        }
    }

    public class Shipment
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Status { get; set; }
    }
}
