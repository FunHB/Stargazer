
namespace Stargazer.Database.Models
{
    public interface ICelestialBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
    }
}
