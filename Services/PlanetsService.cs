using Stargazer.Database.Models;
using System.Diagnostics;

namespace Stargazer.Services
{
    public class PlanetsService(IRestService service) : IPlanetsService
    {
        IRestService _restService = service;

        public async Task<Planet> GetPlanet(string name)
        {
            return await _restService.FetchCelestialBody<Planet>(name, "planets");
        }
    }
}
