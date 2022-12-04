using CsvHelper.Configuration;

namespace FuzionIncRainfallReader.Models
{
    internal class RainfallDeviceReadingMap : ClassMap<RainfallDeviceReading>
    {
        public RainfallDeviceReadingMap()
        {
            Map(m => m.DeviceId).Name("Device ID");
            Map(m => m.ReadingDateTime).Name("Time");
            Map(m => m.RainfallAmount).Name("Rainfall");
        }

    }
}
