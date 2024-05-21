using System;
using DataAccess.Domain;

namespace Template.Domain.Entidades
{
    public class WeatherForecast
    {
        [Column("Date")] //caso o nome do campo no banco de dados esteja diferente, informe o nome do campo do banco na Column
        public DateTime Date_Time { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
