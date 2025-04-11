using SQLite;
using System.Text.Json.Serialization;

namespace Stargazer.Database.Models
{
    [Table("Planets")]
    public class Planet : ICelestialBody
    {
        [Column("Id"), PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [Column("Name")]
        public string Name
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("mass")]
        [Column("Mass")]
        public double Mass
        {
            get; set
            {
                if (value != 0 && Radius != 0)
                    IsGasGiant = Density < Constants.GasGiantThreshold;
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("radius")]
        [Column("Radius")]
        public double Radius
        {
            get; set
            {
                if (value != 0 && Mass != 0)
                    IsGasGiant = Density < Constants.GasGiantThreshold;
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("period")]
        [Column("Period")]
        public double Period
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("semi_major_axis")]
        [Column("SemiMajorAxis")]
        public double SemiMajorAxis
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("temperature")]
        [Column("Temperature")]
        public double Temperature
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("distance_light_year")]
        [Column("DistanceLightYear")]
        public double DistanceLightYear
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("host_star_mass")]
        [Column("HostStarMass")]
        public double HostStarMass
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("host_star_temperature")]
        [Column("HostStarTemperature")]
        public double HostStarTemperature
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [Column("IsGasGiant")]
        public bool IsGasGiant { get; set; }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Column("LastModified")]
        public DateTime LastModified { get; set; } = DateTime.Now;

        public Planet() { }
        public Planet(string name)
        {
            Name = name;
        }

        public Planet(
            string name,
            double mass,
            double radius,
            double period,
            double semiMajorAxis,
            double temperature,
            double distanceLightYear,
            double hostStarMass,
            double hostStarTemperature)
        {
            Name = name;
            Mass = mass;
            Radius = radius;
            Period = period;
            SemiMajorAxis = semiMajorAxis;
            Temperature = temperature;
            DistanceLightYear = distanceLightYear;
            HostStarMass = hostStarMass;
            HostStarTemperature = hostStarTemperature;
        }

        private double Density => (Mass * Constants.JupiterMass) / ((4 / 3) * Math.PI * Math.Pow(Radius * Constants.JupiterRadius, 3));
    }
}
