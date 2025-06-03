using System;

class Program
{
    static string[] parkingSpots = new string[100];

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Prague Parking ===");
            Console.WriteLine("1. Parkera bil/mc");
            Console.WriteLine("2. Visa parkering");
            Console.WriteLine("3. Avsluta parkering");
            Console.WriteLine("4. Sök efter fordon");
            Console.WriteLine("5. Byta plats");
            Console.WriteLine("6. Avsluta");
            Console.Write("Välj ett alternativ: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ParkVehicle();
                    break;
                case "2":
                    ShowParking();
                    break;
                case "3":
                    RemoveVehicle();
                    break;
                case "4":
                    SearchVehicle();
                    break;
                case "5":
                    ChangeParking();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Ogiltigt val. Tryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ParkVehicle()
    {
        Console.Clear();
        Console.Write("Ange fordonstyp (CAR eller MC): ");
        string vehicleType = Console.ReadLine().ToUpper();

        if (vehicleType != "CAR" && vehicleType != "MC")
        {
            Console.WriteLine("Felaktig fordonstyp.");
            Console.ReadKey();
            return;
        }

        Console.Write("Ange registreringsnummer (6 tecken): ");
        string regNr = Console.ReadLine().ToUpper();

        if (string.IsNullOrWhiteSpace(regNr) || regNr.Length != 6)
        {
            Console.WriteLine("Ogiltigt registreringsnummer.");
            Console.ReadKey();
            return;
        }

        string vehicle = $"{vehicleType}#{regNr}";

        if (vehicleType == "MC")
        {
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                if (parkingSpots[i]?.StartsWith("MC#") == true && !parkingSpots[i].Contains("|"))
                {
                    parkingSpots[i] += $"|{vehicle}";
                    Console.WriteLine($"MC dubbelparkerades på plats {i + 1}.");
                    Console.ReadKey();
                    return;
                }
            }
        }
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] == null)
            {
                parkingSpots[i] = vehicle;
                Console.WriteLine($"Fordonet har parkerats på plats {i + 1}.");
                Console.ReadKey();
                return;
            }
        }

        Console.WriteLine("Ingen ledig plats hittades.");
        Console.ReadKey();
    }

    static void ShowParking()
    {
        Console.Clear();
        Console.WriteLine("Parkeringsstatus:");
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] != null)
            {
                Console.WriteLine($"Plats {i + 1}: {parkingSpots[i]}");
            }
            else
            {
                Console.WriteLine($"Plats {i + 1}: Ledig");
            }
        }

        Console.ReadKey();
        Console.Clear();
    }

    static void RemoveVehicle()
    {
        Console.Clear();
        Console.Write("Ange registreringsnummer för att avsluta parkering: ");
        string regNr = Console.ReadLine().ToUpper();
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] != null)
            {
                if (parkingSpots[i].Contains("|"))
                {
                    string[] parts = parkingSpots[i].Split('|');
                    if (parts[0].EndsWith(regNr))
                    {
                        parkingSpots[i] = parts.Length > 1 ? parts[1] : null;
                        Console.WriteLine($"MC {regNr} togs bort från plats {i + 1}.");
                        Console.ReadKey();
                        return;
                    }
                    else if (parts[1].EndsWith(regNr))
                    {
                        parkingSpots[i] = parts[0];
                        Console.WriteLine($"MC {regNr} togs bort från plats {i + 1}.");
                        Console.ReadKey();
                        return;
                    }
                }
                else if (parkingSpots[i].EndsWith(regNr))
                {
                    parkingSpots[i] = null;
                    Console.WriteLine($"Fordon {regNr} togs bort från plats {i + 1}.");
                    Console.ReadKey();
                    return;
                }
            }
        }
        Console.WriteLine("Fordonet hittades inte.");
        Console.ReadKey();
    }

    static void SearchVehicle()
    {
        Console.Clear();
        Console.Write("Ange registreringsnummer för att söka: ");
        string regNr = Console.ReadLine().ToUpper();
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] != null && parkingSpots[i].Contains(regNr))
            {
                Console.WriteLine($"Fordon {regNr} hittades på plats {i + 1}.");
                Console.ReadKey();
                return;
            }
        }
        Console.WriteLine("Fordonet hittades inte.");
        Console.ReadKey();
    }

    static void ChangeParking()
    {
        Console.Clear();
        Console.Write("Ange registreringsnummer för att byta plats: ");
        string regNr = Console.ReadLine().ToUpper();
        int currentParkingSpot = -1;

        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] != null && parkingSpots[i].Contains(regNr))
            {
                currentParkingSpot = i;
                break;
            }
        }

        if (currentParkingSpot == -1)
        {
            Console.WriteLine("Fordonet hittades inte.");
            Console.ReadKey();
            return;
        }

        Console.Write("Ange ny plats (1–100): ");
        if (int.TryParse(Console.ReadLine(), out int newParkingSpot) && newParkingSpot >= 1 && newParkingSpot <= 100)
        {
            newParkingSpot--; 

            if (parkingSpots[newParkingSpot] == null)
            {
                string[] vehicles = parkingSpots[currentParkingSpot].Split('|');

                string vehicleToMove = vehicles.FirstOrDefault(v => v.Contains(regNr));

                if (vehicleToMove != null)
                {
                    parkingSpots[newParkingSpot] = vehicleToMove;

                    vehicles = vehicles.Where(v => v != vehicleToMove).ToArray();
                    parkingSpots[currentParkingSpot] = vehicles.Length > 0 ? string.Join("|", vehicles) : null;

                    Console.WriteLine($"Fordonet {regNr} har flyttats från plats {currentParkingSpot + 1} till plats {newParkingSpot + 1}.");
                }
                else
                {
                    Console.WriteLine("Fordonet hittades inte.");
                }
            }
            else
            {
                Console.WriteLine("Den nya platsen är redan upptagen.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt platsnummer.");
        }
        Console.ReadKey();
    }

}
