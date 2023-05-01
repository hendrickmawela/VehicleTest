using System;
using System.Collections.Generic;
using System.IO;

class Vehicle
{
    public int PositionId { get; set; }
    public string VehicleRegistration { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public ulong RecordedTimeUTC { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // Define the list of positions to find the nearest vehicles to
        List<Tuple<float, float>> positions = new List<Tuple<float, float>> {
            Tuple.Create(34.544909f, -102.100843f),
            Tuple.Create(32.345544f, -99.123124f),
            Tuple.Create(33.234235f, -100.214124f),
            Tuple.Create(35.195739f, -95.348899f),
            Tuple.Create(31.895839f, -97.789573f),
            Tuple.Create(32.895839f, -101.789573f),
            Tuple.Create(34.115839f, -100.225732f),
            Tuple.Create(32.335839f, -99.992232f),
            Tuple.Create(33.535339f, -94.792232f),
            Tuple.Create(32.234235f, -100.222222f)
        };

        // Read the binary data file containing the vehicle data
        string filePath = "vehicles.txt";
        List<Vehicle> vehicles = new List<Vehicle>();
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                Vehicle vehicle = new Vehicle();
                vehicle.PositionId = reader.ReadInt32();
                vehicle.VehicleRegistration = reader.ReadString();
                vehicle.Latitude = reader.ReadSingle();
                vehicle.Longitude = reader.ReadSingle();
                vehicle.RecordedTimeUTC = reader.ReadUInt64();
                vehicles.Add(vehicle);
            }
        }

        // Find the nearest vehicle to each position
        foreach (Tuple<float, float> position in positions)
        {
            double nearestDistance = double.MaxValue;
            Vehicle nearestVehicle = null;
            foreach (Vehicle vehicle in vehicles)
            {
                double distance = Math.Sqrt(Math.Pow(vehicle.Latitude - position.Item1, 2) + Math.Pow(vehicle.Longitude - position.Item2, 2));
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestVehicle = vehicle;
                }
            }

            Console.WriteLine("Nearest vehicle to position ({0}, {1}): PositionId = {2}, VehicleRegistration = {3}", position.Item1, position.Item2, nearestVehicle.PositionId, nearestVehicle.VehicleRegistration);
        }
    }
}
