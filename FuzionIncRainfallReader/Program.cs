using CsvHelper;
using CsvHelper.Configuration;
using FuzionIncRainfallReader.Helpers;
using FuzionIncRainfallReader.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FuzionIncRainfallReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Main lists to hold the data in-memory
            List<RainfallDevice> RainfallDeviceList = new List<RainfallDevice>();
            List<RainfallDeviceReading> RainfallDeviceReadingsList = new List<RainfallDeviceReading>();

            // Configuration for the CsvHelper library
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                ShouldSkipRecord = record => record.Row.Parser.Record.All(field => String.IsNullOrWhiteSpace(field))
            };

            using (var reader = new StreamReader(@"..\..\Devices\Devices.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                //Set the mapping context
                csv.Context.RegisterClassMap<RainfallDeviceMap>();

                while (csv.Read())
                {
                    var rainfallDeviceRecord = csv.GetRecord<RainfallDevice>();
                    
                    RainfallDeviceList.Add(rainfallDeviceRecord);
                }
            }


            // Load the data into memory, enumerating the files that are in the data folder
            // and have the extension of csv.
            string[] RainfallFiles = Directory.GetFiles(@"..\..\Data\", "*.csv");

            foreach (string file in RainfallFiles)
            {
                
                using (var reader = new StreamReader($"{file}"))
                using (var csv = new CsvReader(reader, config))
                {
                    //Set the mapping context
                    csv.Context.RegisterClassMap<RainfallDeviceReadingMap>();

                    while (csv.Read())
                    {
                        var rainReadingRecord = csv.GetRecord<RainfallDeviceReading>();

                        

                        RainfallDeviceReadingsList.Add(rainReadingRecord);
                    }
                }
            }

            // Get the DateTime details
            var maxDateTime = RainfallDeviceReadingsList.Max(m => m.ReadingDateTime);
            //var minDateTime = RainfallDeviceReadingsList.Min(m => m.ReadingDateTime);

            Console.WriteLine(String.Format("Current Time: {0}", maxDateTime));
            Console.WriteLine("");
            ConsoleColour.WriteLine(String.Format(@"|{0,10}|{1,15}|{2,30}|{3,15}|{4,15}|",
                "Device Id", 
                "Device Name", 
                "Location", 
                "Avg Lst(24hrs)", 
                "Trend"), 
                ConsoleColor.White);


            // Enumerate the devices and load the data to the readings so its associated correctly
            // with each device
            foreach (var raindevice in RainfallDeviceList)
            {
                raindevice.AddReadings(RainfallDeviceReadingsList.Where(r => r.DeviceId == raindevice.DeviceId).ToList());

                decimal rainDeviceAverage = raindevice.GetAverageRainFall(maxDateTime.AddHours(-4), maxDateTime);
                decimal previousAverage = raindevice.GetAverageRainFall(maxDateTime.AddHours(-8), maxDateTime.AddHours(-4));

                var readingColour = ConsoleColor.White; //default colour

                if (rainDeviceAverage >=15 || raindevice.hasPrev4HourReadingAbove(maxDateTime.AddHours(-4), maxDateTime, 30))
                {
                    readingColour = ConsoleColor.Red;
                }
                else if (rainDeviceAverage < 10)
                {
                    readingColour = ConsoleColor.Green;
                }
                else if (rainDeviceAverage < 15) 
                {
                    readingColour = ConsoleColor.DarkYellow;
                }
                else if (rainDeviceAverage >= 15)
                {
                    readingColour = ConsoleColor.Red;
                }
                
                ConsoleColour.WriteLine(String.Format("|{0,10}|{1,15}|{2,30}|{3,15}|{4,15}|", 
                    raindevice.DeviceId, 
                    raindevice.DeviceName, 
                    raindevice.Location, 
                    rainDeviceAverage.ToString("0.00"), 
                    rainDeviceAverage > previousAverage ? "Increasing" : "Decreasing"), 
                    readingColour);
               
            }

            Console.ReadLine(); // Leave the display showing
        }
    }
}
