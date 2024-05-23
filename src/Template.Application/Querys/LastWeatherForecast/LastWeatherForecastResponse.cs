using System.Collections.Generic;
using Template.Domain.Entidades;

namespace Template.Application.Querys.LastWeatherForecast
{
    public class LastWeatherForecastResponse
    {
        public IList<WeatherForecast> WheatherForecasts { get; set;}

        public ApiMetaDataResponse metadata { get; set; }
    }
}