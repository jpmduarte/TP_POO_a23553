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
                            // Add Parking Sector
                            while (true)
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
                                            continue;
                                        }
                                        Dictionary<string, decimal> hourlyRates = GetHourlyRates(allowedVehicleTypes);
                                        if (hourlyRates.Count == 0)
                                        {
                                            Console.WriteLine("At least one hourly rate is required.");
                                            continue;
                                        }

                                        ParkingSector newSector = new ParkingSector(sectorName, capacity, hourlyRates, allowedVehicleTypes);
                                        hospitalParking.AddParkingSector(newSector);
                                        Console.WriteLine("Parking Sector added.");
                                        break;
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
                            break;

                        case 2:
                            // User Section
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
                                        // Park Vehicle
                                        while (true)
                                        {
                                            Console.Write("Enter License Plate: ");
                                            string licensePlate = Console.ReadLine();
                                            if (!string.IsNullOrWhiteSpace(licensePlate))
                                            {
                                                if (hospitalParking.CheckForDuplicateLicensePlate(licensePlate))
                                                {
                                                    Console.WriteLine("License Plate is already in use. Please enter a different one.");
                                                    continue; // Continue the loop to re-enter the license plate
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
                                        break;

                                    case 2:
                                        // Remove Vehicle
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
                                        break;

                                    case 3:
                                        // Return to Main Menu
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
