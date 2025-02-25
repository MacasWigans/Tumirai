using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Newtonsoft.Json;
namespace Tumirai.Models
{
    public class Shipment
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Status { get; set; }
    }
}


namespace Tumirai.Models
{
    public class OrderModel
    {
        [JsonProperty("pickupAddress")]
        public string PickupAddress { get; set; }

        [JsonProperty("destinationAddress")]
        public string DestinationAddress { get; set; }

        [JsonProperty("payment")]
        public decimal Payment { get; set; }

        [JsonProperty("distance")]
        public int Distance { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        public bool IsActive => Status == "active";
    }
}
