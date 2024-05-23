using System.Collections.Generic;
using Template.Domain.Entidades;

namespace Template.Domain.Repositorios
{
    public interface IWeatherReadRepository
    {
        List<WeatherForecast> Find();
    }
}