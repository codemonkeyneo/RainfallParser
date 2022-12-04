using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzionIncRainfallReader.Models
{
    internal class RainfallDevice
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Location { get; set; }

        public List<RainfallDeviceReading> Readings { get; set; }

        /// <summary>
        /// List of the readins for the current device
        /// </summary>
        /// <param name="readings"></param>
        public void AddReadings(List<RainfallDeviceReading> readings)
        {
            Readings = readings;
        }

        /// <summary>
        /// Return the average rainfall from the device given the date parameters
        /// </summary>
        /// <param name="fromPeriod"></param>
        /// <param name="toPeriod"></param>
        /// <returns></returns>
        public decimal GetAverageRainFall(DateTime fromPeriod, DateTime toPeriod)
        {
            IEnumerable<RainfallDeviceReading> readings =  this.Readings.Where(r => r.ReadingDateTime >= fromPeriod && r.ReadingDateTime <= toPeriod);
            if (readings.Any())
            {
                return readings.Average(r => r.RainfallAmount);
            } else
            {
                return 0;
            }
        }

        /// <summary>
        /// Return true or false given a date period and warning limit value.
        /// </summary>
        /// <param name="fromPeriod"></param>
        /// <param name="toPeriod"></param>
        /// <param name="warningValue"></param>
        /// <returns></returns>
        public bool hasPrev4HourReadingAbove(DateTime fromPeriod, DateTime toPeriod, decimal warningValue)
        {
            return this.Readings.Where(r => r.ReadingDateTime >= fromPeriod && r.ReadingDateTime <= toPeriod && r.RainfallAmount > warningValue).Any();
        }

    }
}
