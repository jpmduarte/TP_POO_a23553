using System;
using System.Collections.Generic;

namespace Program
{
    public class ParkingSector
    {
        public string SectorName { get; set; }
        public int Capacity { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public Dictionary<string, decimal> HourlyRatesByVehicleType { get; set; }
        public List<string> AllowedVehicleTypes { get; set; }

        public ParkingSector(string sectorName, int capacity, Dictionary<string, decimal> hourlyRates, List<string> allowedVehicleTypes)
        {
            SectorName = sectorName;
            Capacity = capacity;
            HourlyRatesByVehicleType = hourlyRates;
            Vehicles = new List<Vehicle>();
            AllowedVehicleTypes = allowedVehicleTypes;
        }
        public decimal CalculateParkingFee(decimal hourlyRate, DateTime entryTime)
        {
            DateTime exitTime = DateTime.Now;
            TimeSpan parkedTime = exitTime - entryTime;
            decimal hoursParked = (decimal)parkedTime.TotalHours;
            return hoursParked * hourlyRate;
        }


        public bool IsFull()
        {
            return Vehicles.Count >= Capacity;
        }

        public void ParkVehicle(Vehicle vehicle)
        {
            if (!IsFull())
            {
                if (AllowedVehicleTypes.Contains(vehicle.VehicleType))
                {
                    vehicle.EntryTime = DateTime.Now;
                    vehicle.ParkingFee = CalculateParkingFee(HourlyRatesByVehicleType[vehicle.VehicleType], vehicle.EntryTime);
                    Vehicles.Add(vehicle);
                    Console.WriteLine($"Vehicle parked in {SectorName} sector.");
                }
                else
                {
                    Console.WriteLine($"Sorry, {SectorName} sector does not allow {vehicle.VehicleType} vehicles.");
                }
            }
            else
            {
                Console.WriteLine($"Sorry, {SectorName} sector is full.");
            }
        }

        public void RemoveVehicle(string licensePlate)
        {
            Vehicle vehicle = Vehicles.Find(v => v.LicensePlate == licensePlate);
            if (vehicle != null)
            {
                DateTime exitTime = DateTime.Now;
                TimeSpan parkedTime = exitTime - vehicle.EntryTime;
                decimal hoursParked = (decimal)parkedTime.TotalHours;
                vehicle.ParkingFee = hoursParked * HourlyRatesByVehicleType[vehicle.VehicleType];

                Vehicles.Remove(vehicle);
                Console.WriteLine($"Vehicle removed from {SectorName} sector.");
                Console.WriteLine($"Parking Fee: ${vehicle.ParkingFee:0.00}");
            }
            else
            {
                Console.WriteLine($"Vehicle not found in {SectorName} sector.");
            }
        }
    }
}
