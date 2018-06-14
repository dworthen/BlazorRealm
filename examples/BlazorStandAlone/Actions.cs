using Blazor.Realm;
using System.Collections.Generic;

namespace BlazorStandAlone.Actions
{
    namespace Counter
    {
        public class IncrementByOne : IAction { }

        public class IncrementByValue : IAction
        {
            public int Value { get; set; }
            public IncrementByValue(int value)
            {
                Value = value;
            }
        }

        public class DecrementByOne : IAction { }

        public class DecrementByValue : IAction
        {
            public int Value { get; set; }
            public DecrementByValue(int value)
            {
                Value = value;
            }
        }

        public class Set : IAction
        {
            public int Value { get; set; }
            public Set(int value)
            {
                Value = value;
            }
        }

        public class Reset : IAction { }

        public class Dispose : Reset { }
    }

    namespace WeatherForecasts
    {
        public class Clear : IAction { }

        public class Set : IAction
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
        public class Start : IAction { }
        public class Complete : IAction { }
    }
}
