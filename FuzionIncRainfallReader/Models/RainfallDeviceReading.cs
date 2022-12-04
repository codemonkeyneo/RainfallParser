using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzionIncRainfallReader.Models
{
    internal class RainfallDeviceReading
    {
        public int DeviceId { get; set; }
        public DateTime ReadingDateTime { get; set; }
        public Decimal RainfallAmount { get; set; }
    }
}
