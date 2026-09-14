using _02_Services;
using _04_Models;
using _05_Configuration;


Console.WriteLine($"Welcome to {AppSetting.AppName} version {AppSetting.Version}\n");

ICarService carService = new CarService();

List<ICar> cars = carService.GetCars();

Console.WriteLine("Registreringsnummer:");
foreach (var car in cars)
{
    Console.WriteLine($"- {car.CarRegistrationNumber}");
}