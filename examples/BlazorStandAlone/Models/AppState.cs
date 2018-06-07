﻿using System.Collections.Generic;

namespace BlazorStandAlone.Models
{
    public class AppState
    {
        public bool Loading { get; set; } = false;
        public int Count { get; set; }
        public IEnumerable<WeatherForecast> WeatherForecasts { get; set; } = new WeatherForecast[] { };
    }
}
