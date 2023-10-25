using System;
using System.Collections.Generic;

  namespace Program
{
    public class HospitalParking
    {
        public List<ParkingSector> ParkingSectors { get; set; }

        public HospitalParking()
        {
            ParkingSectors = new List<ParkingSector>();
        }

        public void AddParkingSector(ParkingSector sector)
        {
            ParkingSectors.Add(sector);
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



    }
}