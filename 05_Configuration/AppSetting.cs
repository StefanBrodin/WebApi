using System;

namespace _05_Configuration;

public class AppSetting
{
    public const string AppName = "Cars";
    public const string Version = "0.0.1";

    public static class CarsService
    {
        public const int NrCars = 5;
        public static readonly string[] CarRegistrationNumbers = new string[]
        {
            "ABC123",
            "DEF456",
            "GHI789",
            "JKL012",
            "MNO345"
        };
    }
}
