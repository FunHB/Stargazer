using Stargazer.Database.Models;

namespace Stargazer.Services
{
    public interface IRestService
    {
        public Task<T> FetchCelestialBody<T>(string name, string endpoint) where T : ICelestialBody, new();
    }
}
