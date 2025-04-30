using System;

namespace Template.Application.Querys.LastWeatherForecast
{
    public class ApiErrorResponse
    {
        public string error { get; set; }

        public string message { get; set; }

        public string detail { get; set; }

    }
}