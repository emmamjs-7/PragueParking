using System;

class Program
{
    // Skapa parkering med 100 platser
    static string[] parkingGarage = new string[100];

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Prague Parking 1.0 ===");
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
        string type = Console.ReadLine().ToUpper();

        if (type != "CAR" && type != "MC")
        {
            Console.WriteLine("Felaktig fordonstyp.");
            Console.ReadKey();
            return;
        }

        Console.Write("Ange registreringsnummer (max 10 tecken): ");
        string regNr = Console.ReadLine().ToUpper();

        if (string.IsNullOrWhiteSpace(regNr) || regNr.Length > 10)
        {
            Console.WriteLine("Ogiltigt registreringsnummer.");
            Console.ReadKey();
            return;
        }

        string vehicle = $"{type}#{regNr}";

        if (type == "MC")
        {
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                if (parkingGarage[i]?.StartsWith("MC#") == true && !parkingGarage[i].Contains("|"))
                {
                    parkingGarage[i] += $"|{vehicle}";
                    Console.WriteLine($"MC dubbelparkerades på plats {i + 1}.");
                    Console.ReadKey();
                    return;
                }
            }
        }
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] == null)
            {
                parkingGarage[i] = vehicle;
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
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] != null)
            {
                Console.WriteLine($"Plats {i + 1}: {parkingGarage[i]}");
            }
            else
            {
                Console.WriteLine($"Plats {i + 1}: Ledig");
            }
        }
        Console.ReadKey();
    }

    static void RemoveVehicle()
    {
        Console.Clear();
        Console.Write("Ange registreringsnummer för att avsluta parkering: ");
        string regNr = Console.ReadLine().ToUpper();
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] != null)
            {
                if (parkingGarage[i].Contains("|"))
                {
                    string[] parts = parkingGarage[i].Split('|');
                    if (parts[0].EndsWith(regNr))
                    {
                        parkingGarage[i] = parts.Length > 1 ? parts[1] : null;
                        Console.WriteLine($"MC {regNr} togs bort från plats {i + 1}.");
                        Console.ReadKey();
                        return;
                    }
                    else if (parts[1].EndsWith(regNr))
                    {
                        parkingGarage[i] = parts[0];
                        Console.WriteLine($"MC {regNr} togs bort från plats {i + 1}.");
                        Console.ReadKey();
                        return;
                    }
                }
                else if (parkingGarage[i].EndsWith(regNr))
                {
                    parkingGarage[i] = null;
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
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] != null && parkingGarage[i].Contains(regNr))
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
        int currentIndex = -1;
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] != null && parkingGarage[i].Contains(regNr))
            {
                currentIndex = i;
                break;
            }
        }

        if (currentIndex == -1)
        {
            Console.WriteLine("Fordonet hittades inte.");
            Console.ReadKey();
            return;
        }

        Console.Write("Ange ny plats (1–100): ");
        if (int.TryParse(Console.ReadLine(), out int newIndex) && newIndex >= 1 && newIndex <= 100)
        {
            newIndex--;
            if (parkingGarage[newIndex] == null)
            {
                parkingGarage[newIndex] = parkingGarage[currentIndex];
                parkingGarage[currentIndex] = null;
                Console.WriteLine($"Fordonet har flyttats till plats {newIndex + 1}.");
            }
            else
            {
                Console.WriteLine("Den platsen är redan upptagen.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt platsnummer.");
        }
        Console.ReadKey();
    }
}
