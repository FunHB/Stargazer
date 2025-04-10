using Stargazer.Database.Models;

namespace Stargazer.Services
{
    public interface IPlanetsService
    {
        public Task<Planet> GetPlanet(string name);
    }
}
