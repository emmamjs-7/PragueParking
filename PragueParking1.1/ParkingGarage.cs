using System;
using System.Linq;
using System.Text.RegularExpressions;

public class ParkingGarage
{
    private string[] parkingSpots = new string[100];

    public void ParkVehicle()
    {
        Console.Clear();
        string vehicleType = GetVehicleType();
        string regNr = GetRegistrationNumber();

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string vehicle = $"{vehicleType}#{regNr}#{timestamp}";

        if (vehicleType == "MC" && TryDoubleParkMC(vehicle)) return;
        if (TryParkVehicle(vehicle)) return;

        Console.WriteLine("Ingen ledig plats hittades.");
        Console.ReadKey();
    }

    private string GetVehicleType()
    {
        while (true)
        {
            Console.Write("Ange fordonstyp (CAR eller MC): ");
            string type = Console.ReadLine().Trim().ToUpper();
            if (type == "CAR" || type == "MC") return type;

            Console.WriteLine("Felaktig fordonstyp. Försök igen.");
        }
    }

    private string GetRegistrationNumber()
    {
        while (true)
        {
            Console.Write("Ange registreringsnummer (6 tecken, inga mellanslag): ");
            string regNr = Console.ReadLine().Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(regNr))
            {
                Console.WriteLine("Registreringsnumret får inte vara tomt.");
                continue;
            }

            if (regNr.Length == 6)
                return regNr;

            Console.WriteLine("Ogiltigt registreringsnummer. Försök igen.");
        }
    }

    private bool TryDoubleParkMC(string vehicle)
    {
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i]?.StartsWith("MC#") == true && !parkingSpots[i].Contains("|"))
            {
                parkingSpots[i] = string.Join("|", parkingSpots[i], vehicle);
                Console.WriteLine($"MC dubbelparkerades på plats {i + 1}.");
                Console.ReadKey();
                return true;
            }
        }
        return false;
    }

    private bool TryParkVehicle(string vehicle)
    {
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] == null)
            {
                parkingSpots[i] = vehicle;
                Console.WriteLine($"Fordonet har parkerats på plats {i + 1}.");
                Console.ReadKey();
                return true;
            }
        }
        return false;
    }

    public void ShowParking()
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
    }

    public void RemoveVehicle()
    {
        Console.Clear();
        Console.Write("Ange registreringsnummer för att ta bort: ");
        string regNr = Console.ReadLine().ToUpper();

        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] == null) continue;

            var vehicles = parkingSpots[i].Split('|');
            int vehicleIndex = Array.FindIndex(vehicles, v =>
            {
                var vehicleInfo = v.Split('#');
                return vehicleInfo.Length >= 3 && vehicleInfo[1] == regNr;
            });

            if (vehicleIndex != -1)
            {
                var vehicleInfo = vehicles[vehicleIndex].Split('#');
                PrintParkingDuration(vehicleInfo[2]);

                vehicles = vehicles.Where((v, index) => index != vehicleIndex).ToArray();
                parkingSpots[i] = vehicles.Length > 0 ? string.Join("|", vehicles) : null;

                Console.WriteLine($"Fordonet {regNr} har tagits bort från plats {i + 1}.");
                Console.ReadKey();
                return;
            }
        }

        Console.WriteLine("Fordonet hittades inte.");
        Console.ReadKey();
    }

    private void PrintParkingDuration(string timeString)
    {
        if (DateTime.TryParse(timeString, out DateTime startTime))
        {
            TimeSpan duration = DateTime.Now - startTime;
            string durationText = duration.TotalDays >= 1
                ? $"{(int)duration.TotalDays} dagar, {duration.Hours}h {duration.Minutes}min"
                : $"{duration.Hours}h {duration.Minutes}min";

            Console.WriteLine($"Fordonet har varit parkerat i: {durationText}");
        }
    }

    public void SearchVehicle()
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

    public void ChangeParking()
    {
        Console.Clear();
        Console.Write("Ange registreringsnummer för att byta plats: ");
        string regNr = Console.ReadLine().ToUpper();
        int currentParkingSpot = -1;
        if (string.IsNullOrWhiteSpace(regNr))
        {
            Console.WriteLine("Ogiltigt registreringsnummer.");
            Console.ReadKey();
            return;
        }

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

    public void OptimizeParking()
    {
        Console.Clear();
        Console.WriteLine("Optimerar MC-platser...");

        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i]?.StartsWith("MC#") == true && !parkingSpots[i].Contains("|"))
            {
                for (int j = i + 1; j < parkingSpots.Length; j++)
                {
                    if (parkingSpots[j]?.StartsWith("MC#") == true && !parkingSpots[j].Contains("|"))
                    {
                        parkingSpots[i] = string.Join("|", parkingSpots[i], parkingSpots[j]);
                        parkingSpots[j] = null;

                        Console.WriteLine($"Flytta MC från plats {j + 1} till plats {i + 1}.");
                        break;
                    }
                }
            }
        }

        Console.WriteLine("Optimering klar.");
        Console.ReadKey();
    }
}
