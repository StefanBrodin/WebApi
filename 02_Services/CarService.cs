using _04_Models;
using _05_Configuration;

namespace _02_Services;

public class CarService : ICarService
{
    public string? CarRegistrationNumber { get; set; }

    public List<ICar> GetCars()
    {
        List<ICar> cars = new List<ICar>();

        foreach (var registrationNumber in AppSetting.CarsService.CarRegistrationNumbers)
        {
            cars.Add(new Car { CarRegistrationNumber = registrationNumber });
        }

        return cars;
    }
}
