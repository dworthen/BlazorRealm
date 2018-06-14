using Blazor.Realm;
using System.Collections.Generic;

namespace BlazorStandAlone.Actions
{
    namespace Counter
    {
        public class IncrementByOne : IRealmAction { }

        public class IncrementByValue : IRealmAction
        {
            public int Value { get; set; }
            public IncrementByValue(int value)
            {
                Value = value;
            }
        }

        public class DecrementByOne : IRealmAction { }

        public class DecrementByValue : IRealmAction
        {
            public int Value { get; set; }
            public DecrementByValue(int value)
            {
                Value = value;
            }
        }

        public class Set : IRealmAction
        {
            public int Value { get; set; }
            public Set(int value)
            {
                Value = value;
            }
        }

        public class Reset : IRealmAction { }

        public class Dispose : Reset { }
    }

    namespace WeatherForecasts
    {
        public class Clear : IRealmAction { }

        public class Set : IRealmAction
        {
            public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
            public Set(IEnumerable<WeatherForecast> forecasts)
            {
                WeatherForecasts = forecasts;
            }
        }
    }

    namespace Loading
    {
        public class Start : IRealmAction { }
        public class Complete : IRealmAction { }
    }
}
