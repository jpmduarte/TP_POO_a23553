using System;
using System.Collections.Generic;
using System.Linq;

namespace Program
{
    public class HospitalParking
    {
        public List<ParkingSector> ParkingSectors { get; private set; }

        public HospitalParking()
        {
            ParkingSectors = new List<ParkingSector>();
        }

        public void AddParkingSector(ParkingSector sector)
        {
            ParkingSectors.Add(sector);
        }

        public bool RemoveParkingSector(string sectorName)
        {
            ParkingSector sectorToRemove = ParkingSectors.Find(sector => sector.SectorName == sectorName);

            if (sectorToRemove != null)
            {
                ParkingSectors.Remove(sectorToRemove);
                return true;
            }

            return false;
        }

        public bool CheckForDuplicateLicensePlate(string licensePlate)
        {
            // Check for duplicates in all parking sectors
            foreach (var sector in ParkingSectors)
            {
                if (sector.Vehicles.Exists(vehicle => vehicle.LicensePlate == licensePlate))
                {
                    return true; // Duplicate found
                }
            }
            return false; // No duplicate found
        }
        public void DisplayParkingSpaceAvailability()
        {
            Console.WriteLine("Parking Space Availability:");
            foreach (var sector in ParkingSectors)
            {
                int availableSpace = sector.Capacity - sector.Vehicles.Count;
                Console.WriteLine($"{sector.SectorName}: {availableSpace} out of {sector.Capacity} spaces available");
            }
        }
    }

}
