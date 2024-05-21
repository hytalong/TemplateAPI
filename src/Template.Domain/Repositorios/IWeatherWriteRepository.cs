using Template.Domain.Entidades;

namespace Template.Domain.Repositorios
{
    public interface IWeatherWriteRepository
    {
        bool Insert(WeatherForecast weatherForecast);
    }
}