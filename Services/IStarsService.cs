using Stargazer.Database.Models;

namespace Stargazer.Services
{
    public interface IStarsService
    {
        public Task<Star> GetStar(string name);
    }
}
