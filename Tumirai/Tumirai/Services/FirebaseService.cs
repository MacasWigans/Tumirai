using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Tumirai.Models;

namespace Tumirai.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient("https://tumirei-default-rtdb.firebaseio.com/");
        }

        public async Task<Shipment> GetShipmentAsync(string shipmentNumber)
        {
            try
            {
                var shipmentData = await _firebaseClient
                    .Child("shipmentNumber")
                    .Child("shipments")
                    .Child(shipmentNumber)
                    .OnceSingleAsync<Shipment>();

                return shipmentData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching shipment: {ex.Message}");
                return null;
            }
        }
    }
}

