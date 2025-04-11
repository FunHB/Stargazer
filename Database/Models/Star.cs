using SQLite;
using System.Text.Json.Serialization;

namespace Stargazer.Database.Models
{
    [Table("Stars")]
    public class Star : ICelestialBody
    {
        [Column("Id"), PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [Column("Name"), NotNull]
        public string Name
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("constellation")]
        [Column("Constellation")]
        public string Constellation
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("right_ascension")]
        [Column("RightAscension")]
        public string RightAscension
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("declination")]
        [Column("Declination")]
        public string Declination
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("apparent_magnitude")]
        [Column("ApparentMagnitude")]
        public string ApparentMagnitude
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("absolute_magnitude")]
        [Column("AbsoluteMagnitude")]
        public string AbsoluteMagnitude
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("distance_light_year")]
        [Column("DistanceLightYear")]
        public string DistanceLightYear
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [JsonPropertyName("spectral_class")]
        [Column("SpectralClass")]
        public string SpectralClass
        {
            get; set
            {
                LastModified = DateTime.Now;
                field = value;
            }
        }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Column("LastModified")]
        public DateTime LastModified { get; set; } = DateTime.Now;

        public Star() { }
        public Star(string name)
        {
            Name = name;
        }

        public Star(
            string name,
            string constellation,
            string rightAscension,
            string declination,
            string apparentMagnitude,
            string absoluteMagnitude,
            string distance,
            string spectralclass)
        {
            Name = name;
            Constellation = constellation;
            RightAscension = rightAscension;
            Declination = declination;
            ApparentMagnitude = apparentMagnitude;
            AbsoluteMagnitude = absoluteMagnitude;
            DistanceLightYear = distance;
            SpectralClass = spectralclass;
        }
    }
}
