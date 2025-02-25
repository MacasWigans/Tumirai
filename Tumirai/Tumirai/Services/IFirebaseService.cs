using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumirai.Models;

namespace Tumirai.Services
{
    public interface IFirebaseService
    {
        Task<Shipment> GetShipmentAsync(string shipmentId);
    }
}
