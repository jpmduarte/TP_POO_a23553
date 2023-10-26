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

        public List<ParkingRecord> ParkingHistory { get; set; }

        public ParkingSector(string sectorName, int capacity, Dictionary<string, decimal> hourlyRates, List<string> allowedVehicleTypes)
        {
            SectorName = sectorName;
            Capacity = capacity;
            HourlyRatesByVehicleType = hourlyRates;
            Vehicles = new List<Vehicle>();
            AllowedVehicleTypes = allowedVehicleTypes;
            ParkingHistory = new List<ParkingRecord>();
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
            if (IsFull())
            {
                Console.WriteLine($"Sorry, {SectorName} sector is full.");
                return; // No need to continue if the sector is full
            }

            if (!AllowedVehicleTypes.Contains(vehicle.VehicleType))
            {
                Console.WriteLine($"Sorry, {SectorName} sector does not allow {vehicle.VehicleType} vehicles.");
                return; // No need to continue if the vehicle type is not allowed
            }

            vehicle.EntryTime = DateTime.Now;
            vehicle.ParkingFee = CalculateParkingFee(HourlyRatesByVehicleType[vehicle.VehicleType], vehicle.EntryTime);
            Vehicles.Add(vehicle);

            Console.WriteLine($"Vehicle parked in {SectorName} sector.");
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

                // Create a ParkingRecord before removing the vehicle
                ParkingRecord parkingRecord = new ParkingRecord(vehicle, vehicle.EntryTime, exitTime, vehicle.ParkingFee);
                ParkingHistory.Add(parkingRecord);

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
