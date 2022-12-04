using CsvHelper.Configuration;

namespace FuzionIncRainfallReader.Models
{
    internal class RainfallDeviceMap : ClassMap<RainfallDevice>
    {
        public RainfallDeviceMap()
        {
            Map(m => m.DeviceId).Name("Device ID");
            Map(m => m.DeviceName).Name("Device Name");
            Map(m => m.Location).Name("Location");
        }
    }
}
