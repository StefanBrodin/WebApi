using Models;
using Configuration;

namespace Services;

public class CarsService : ICarsService 
{
    public List<Car> GetCars() 
    {
        List<Car> cars = new List<Car>();

        foreach (var registrationNumber in AppSetting.Cars.CarRegistrationNumbers)
        {
            cars.Add(new Car { CarRegistrationNumber = registrationNumber });
        }

        return cars;
    }
}
