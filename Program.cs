using System;
using System.Collections.Generic;
using System.Linq;

namespace Program
{
    class Program
    {
        static void Main()
        {
            HospitalParking hospitalParking = new HospitalParking();
            List<string> VehicleTypes = new List<string> { "Car", "Motorcycle", "Truck" };

            while (true)
            {
                Console.WriteLine("\nHospital Parking Management System");
                Console.WriteLine("1. Management");
                Console.WriteLine("2. User");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ManageParking(hospitalParking, VehicleTypes);
                            break;

                        case 2:
                            UserSection(hospitalParking, VehicleTypes);
                            break;

                        case 0:
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please select a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
        }

        static void ManageParking(HospitalParking hospitalParking, List<string> VehicleTypes)
        {
            while (true)
            {
                Console.WriteLine("\nManagement Section");
                Console.WriteLine("1. Add Parking Sector");
                Console.WriteLine("2. Remove Parking Sector");
                Console.WriteLine("3. Statistics");
                Console.WriteLine("4. Return to Main Menu");
                Console.Write("Select an option: ");

                if (int.TryParse(Console.ReadLine(), out int managementChoice))
                {
                    switch (managementChoice)
                    {
                        case 1:
                            AddParkingSector(hospitalParking, VehicleTypes);
                            break;

                        case 2:
                            RemoveParkingSector(hospitalParking);
                            break;

                        case 3:
                            ShowStatistics(hospitalParking);
                            break;

                        case 4:
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please select a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
        }

        static void AddParkingSector(HospitalParking hospitalParking, List<string> VehicleTypes)
        {
            Console.Write("Enter Sector Name: ");
            string sectorName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(sectorName))
            {
                Console.Write("Enter Capacity: ");
                if (int.TryParse(Console.ReadLine(), out int capacity))
                {
                    List<string> allowedVehicleTypes = GetAllowedVehicleTypes(VehicleTypes);
                    if (allowedVehicleTypes.Count == 0)
                    {
                        Console.WriteLine("At least one allowed vehicle type is required.");
                        return;
                    }
                    Dictionary<string, decimal> hourlyRates = GetHourlyRates(allowedVehicleTypes);
                    if (hourlyRates.Count == 0)
                    {
                        Console.WriteLine("At least one hourly rate is required.");
                        return;
                    }

                    ParkingSector newSector = new ParkingSector(sectorName, capacity, hourlyRates, allowedVehicleTypes);
                    hospitalParking.AddParkingSector(newSector);
                    Console.WriteLine("Parking Sector added.");
                }
                else
                {
                    Console.WriteLine("Invalid capacity. Please enter a valid number.");
                }
            }
            else
            {
                Console.WriteLine("Sector Name cannot be empty.");
            }
        }

        static void RemoveParkingSector(HospitalParking hospitalParking)
        {
            Console.WriteLine("Available Parking Sectors:");
            for (int i = 0; i < hospitalParking.ParkingSectors.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + hospitalParking.ParkingSectors[i].SectorName);
            }

            Console.Write("Enter the number of the sector to remove: ");
            if (int.TryParse(Console.ReadLine(), out int sectorNumber) && sectorNumber >= 1 && sectorNumber <= hospitalParking.ParkingSectors.Count)
            {
                int sectorIndexToRemove = sectorNumber - 1; // Adjust for 0-based index
                if (sectorIndexToRemove >= 0 && sectorIndexToRemove < hospitalParking.ParkingSectors.Count)
                {
                    hospitalParking.ParkingSectors.RemoveAt(sectorIndexToRemove); // Remove the sector
                    Console.WriteLine($"Sector {sectorNumber} has been removed.");
                }
                else
                {
                    Console.WriteLine("Invalid sector number. Please enter a valid number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

        static void UserSection(HospitalParking hospitalParking, List<string> VehicleTypes)
        {
            while (true)
            {
                Console.WriteLine("\nUser Section");
                Console.WriteLine("1. Park Vehicle");
                Console.WriteLine("2. Remove Vehicle");
                Console.WriteLine("3. Return to Main Menu");
                Console.Write("Select an option: ");

                if (int.TryParse(Console.ReadLine(), out int userChoice))
                {
                    switch (userChoice)
                    {
                        case 1:
                            ParkVehicle(hospitalParking, VehicleTypes);
                            break;

                        case 2:
                            RemoveVehicle(hospitalParking);
                            break;

                        case 3:
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please select a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
        }

        static void ParkVehicle(HospitalParking hospitalParking, List<string> VehicleTypes)
        {
            while (true)
            {
                Console.Write("Enter License Plate: ");
                string licensePlate = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(licensePlate))
                {
                    if (hospitalParking.CheckForDuplicateLicensePlate(licensePlate))
                    {
                        Console.WriteLine("License Plate is already in use. Please enter a different one.");
                        continue;
                    }

                    Console.WriteLine("Select Vehicle Type:");
                    for (int i = 0; i < VehicleTypes.Count; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + VehicleTypes[i]);
                    }
                    Console.Write("Select a vehicle type: ");
                    if (int.TryParse(Console.ReadLine(), out int selectedVehicleType) && selectedVehicleType >= 1 && selectedVehicleType <= VehicleTypes.Count)
                    {
                        string vehicleType = VehicleTypes[selectedVehicleType - 1];

                        Console.WriteLine("Available Parking Sectors:");
                        for (int i = 0; i < hospitalParking.ParkingSectors.Count; i++)
                        {
                            Console.WriteLine((i + 1) + ". " + hospitalParking.ParkingSectors[i].SectorName);
                        }
                        Console.Write("Select a sector to park in: ");
                        if (int.TryParse(Console.ReadLine(), out int selectedSector) && selectedSector >= 1 && selectedSector <= hospitalParking.ParkingSectors.Count)
                        {
                            Vehicle vehicle = new Vehicle
                            {
                                LicensePlate = licensePlate,
                                VehicleType = vehicleType,
                            };
                            hospitalParking.ParkingSectors[selectedSector - 1].ParkVehicle(vehicle);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid sector number.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid vehicle type.");
                    }
                }
                else
                {
                    Console.WriteLine("License Plate cannot be empty.");
                }
            }
        }

        static void RemoveVehicle(HospitalParking hospitalParking)
        {
            while (true)
            {
                Console.Write("Enter License Plate: ");
                string plate = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(plate))
                {
                    foreach (var sector in hospitalParking.ParkingSectors)
                    {
                        sector.RemoveVehicle(plate);
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("License Plate cannot be empty.");
                }
            }
        }

        static void ShowStatistics(HospitalParking hospitalParking)
        {
            while (true)
            {
                Console.WriteLine("\nStatistics");
                Console.WriteLine("1. Sector Statistics");
                Console.WriteLine("2. Vehicle Statistics");
                Console.WriteLine("3. Parking History for License Plate");
                Console.WriteLine("4. Return to Management Menu");
                Console.Write("Select an option: ");

                if (int.TryParse(Console.ReadLine(), out int statsChoice))
                {
                    switch (statsChoice)
                    {
                        case 1:
                            // Sector Statistics
                            Console.WriteLine("\nSector Statistics");

                            if (hospitalParking.ParkingSectors.Count == 0)
                            {
                                Console.WriteLine("No parking sectors available.");
                            }
                            else
                            {
                                foreach (var sector in hospitalParking.ParkingSectors)
                                {
                                    Console.WriteLine($"{sector.SectorName} Sector:");
                                    Console.WriteLine($"Capacity: {sector.Capacity}");
                                    Console.WriteLine($"Number of Vehicles Parked: {sector.Vehicles.Count}");
                                    Console.WriteLine($"Is Full: {sector.IsFull()}");
                                    Console.WriteLine($"Available Spaces: {sector.Capacity - sector.Vehicles.Count}");
                                    Console.WriteLine();
                                }
                            }
                            break;

                        case 2:
                            // Vehicle Statistics
                            Console.WriteLine("\nVehicle Statistics");
                            Console.WriteLine("1. List All Vehicles");
                            Console.WriteLine("2. List Vehicles by Sector");
                            Console.Write("Select an option: ");

                            if (int.TryParse(Console.ReadLine(), out int vehicleStatsChoice))
                            {
                                switch (vehicleStatsChoice)
                                {
                                    case 1:
                                        // List All Vehicles
                                        Console.WriteLine("All Vehicles:");
                                        int totalVehicles = 0;
                                        foreach (var sector in hospitalParking.ParkingSectors)
                                        {
                                            totalVehicles += sector.Vehicles.Count;
                                            foreach (var vehicle in sector.Vehicles)
                                            {
                                                Console.WriteLine($"License Plate: {vehicle.LicensePlate}, Vehicle Type: {vehicle.VehicleType}");
                                            }
                                        }
                                        if (totalVehicles == 0)
                                        {
                                            Console.WriteLine("No vehicles parked in any sector.");
                                        }
                                        break;

                                    case 2:
                                        // List Vehicles by Sector
                                        Console.WriteLine("\nList Vehicles by Sector:");
                                        if (hospitalParking.ParkingSectors.Count == 0)
                                        {
                                            Console.WriteLine("No parking sectors available.");
                                        }
                                        else
                                        {
                                            for (int i = 0; i < hospitalParking.ParkingSectors.Count; i++)
                                            {
                                                Console.WriteLine($"{i + 1}. {hospitalParking.ParkingSectors[i].SectorName}");
                                            }
                                            Console.Write("Select a sector: ");

                                            if (int.TryParse(Console.ReadLine(), out int selectedSector) && selectedSector >= 1 && selectedSector <= hospitalParking.ParkingSectors.Count)
                                            {
                                                var selectedSectorVehicles = hospitalParking.ParkingSectors[selectedSector - 1].Vehicles;

                                                if (selectedSectorVehicles.Count == 0)
                                                {
                                                    Console.WriteLine($"No vehicles parked in {hospitalParking.ParkingSectors[selectedSector - 1].SectorName} Sector.");
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Vehicles in {hospitalParking.ParkingSectors[selectedSector - 1].SectorName} Sector:");
                                                    foreach (var vehicle in selectedSectorVehicles)
                                                    {
                                                        Console.WriteLine($"License Plate: {vehicle.LicensePlate}, Vehicle Type: {vehicle.VehicleType}");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid sector number.");
                                            }
                                        }
                                        break;

                                    default:
                                        Console.WriteLine("Invalid option. Please select a valid option.");
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid option.");
                            }
                            break;

                        case 3:
                            // Parking History for License Plate
                            Console.Write("Enter License Plate: ");
                            string plate = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(plate))
                            {
                                ShowParkingHistoryForLicensePlate(hospitalParking, plate);
                            }
                            else
                            {
                                Console.WriteLine("License Plate cannot be empty.");
                            }
                            break;

                        case 4:
                            // Return to Management Menu
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please select a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
        }

        static void ShowParkingHistoryForLicensePlate(HospitalParking hospitalParking, string licensePlate)
        {
            Console.WriteLine($"Parking History for License Plate: {licensePlate}");
            bool found = false;

            foreach (var sector in hospitalParking.ParkingSectors)
            {
                foreach (var record in sector.ParkingHistory)
                {
                    if (record.Vehicle.LicensePlate == licensePlate)
                    {
                        found = true;
                        Console.WriteLine($"Sector: {sector.SectorName}");
                        Console.WriteLine($"Entry Time: {record.EntryTime}");
                        Console.WriteLine($"Exit Time: {record.ExitTime}");
                        Console.WriteLine($"Parking Fee: ${record.ParkingFee:0.00}");
                        Console.WriteLine();
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("No parking history found for the provided license plate.");
            }
        }

        static Dictionary<string, decimal> GetHourlyRates(List<string> allowedVehicleTypes)
        {
            Dictionary<string, decimal> hourlyRates = new Dictionary<string, decimal>();
            Console.WriteLine("Set Hourly Rates for Allowed Vehicle Types:");

            foreach (var vehicleType in allowedVehicleTypes)
            {
                Console.Write($"Hourly rate for {vehicleType}: $");
                if (decimal.TryParse(Console.ReadLine(), out decimal rate))
                {
                    hourlyRates[vehicleType] = rate;
                }
                else
                {
                    Console.WriteLine($"Invalid hourly rate for {vehicleType}. Please enter a valid number.");
                }
            }

            return hourlyRates;
        }

        static List<string> GetAllowedVehicleTypes(List<string> vehicleTypes)
        {
            List<string> allowedVehicleTypes = new List<string>();
            Console.WriteLine("Select Allowed Vehicle Types (comma-separated by number):");

            for (int i = 0; i < vehicleTypes.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + vehicleTypes[i]);
            }

            Console.Write("Enter the numbers of allowed vehicle types: ");
            var selectedVehicleTypeNumbers = Console.ReadLine().Split(',')
                .Select(str => str.Trim())
                .Select(int.Parse)
                .ToList();

            foreach (var selectedTypeNumber in selectedVehicleTypeNumbers)
            {
                if (selectedTypeNumber >= 1 && selectedTypeNumber <= vehicleTypes.Count)
                {
                    allowedVehicleTypes.Add(vehicleTypes[selectedTypeNumber - 1]);
                }
            }

            return allowedVehicleTypes;
        }
    }
}


