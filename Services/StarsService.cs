using Stargazer.Database.Models;

namespace Stargazer.Services
{
    public class StarsService(IRestService service) : IStarsService
    {
        IRestService _restService = service;

        public async Task<Star> GetStar(string name)
        {
            return await _restService.FetchCelestialBody<Star>(name, "stars");
        }
    }
}
