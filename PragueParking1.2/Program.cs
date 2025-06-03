using System;

class Program
{
    static void Main()
    {
        ParkingGarage garage = new ParkingGarage();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Prague Parking 1.1 ===");
            Console.WriteLine("1. Parkera bil/mc");
            Console.WriteLine("2. Visa parkering");
            Console.WriteLine("3. Avsluta parkering");
            Console.WriteLine("4. Sök efter fordon");
            Console.WriteLine("5. Byta plats");
            Console.WriteLine("6. Optimera MC-platser");
            Console.WriteLine("7. Avsluta");
            Console.Write("Välj ett alternativ: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    garage.ParkVehicle();
                    break;
                case "2":
                    garage.ShowParking();
                    break;
                case "3":
                    garage.RemoveVehicle();
                    break;
                case "4":
                    garage.SearchVehicle();
                    break;
                case "5":
                    garage.ChangeParking();
                    break;
                case "6":
                    garage.OptimizeParking();
                    break;
                case "7":
                    Console.WriteLine("Avslutar...");
                    return;
                default:
                    Console.WriteLine("Ogiltigt val. Tryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
