using Stargazer.Database;
using Stargazer.Database.Models;
using Stargazer.Services;
using System.Collections.ObjectModel;

namespace Stargazer.Views
{
    public partial class MainPage : ContentPage
    {
        public string Name { get; set; } = "";

        private CelestialDatabase database;
        private IStarsService starsService;
        private IPlanetsService planetsService;

        public ObservableCollection<Star> Stars { get; set; } = [];
        public ObservableCollection<Planet> Planets { get; set; } = [];

        public MainPage(CelestialDatabase celestialDatabase, IStarsService _starsService, IPlanetsService _planetsService)
        {
            InitializeComponent();
            database = celestialDatabase;
            starsService = _starsService;
            planetsService = _planetsService;
            BindingContext = this;
        }

        private async Task UpdateStarsAsync()
        {
            var stars = await database.GetItemsAsync<Star>();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Stars.Clear();
                foreach (var star in stars)
                    Stars.Add(star);
            });
        }

        private async Task UpdatePlanetsAsync()
        {
            var planets = await database.GetItemsAsync<Planet>();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Planets.Clear();
                foreach (var planet in planets)
                    Planets.Add(planet);
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await UpdateStarsAsync();
            await UpdatePlanetsAsync();
        }

        private async void OnAddPlanetClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await DisplayAlert("Name Required", "Please enter a name for the planet.", "OK");
                return;
            }

            if (Stars.Select(star => star.Name).Contains(Name))
            {
                await DisplayAlert("Already Exists", "There is already star with this name.", "OK");
                return;
            }

            Planet planet = await database.GetItemAsync<Planet>(planet => planet.Name == Name) ?? await planetsService.GetPlanet(Name);
            await database.SaveItemAsync(planet);
            await UpdatePlanetsAsync();
        }

        private async void OnAddStarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await DisplayAlert("Name Required", "Please enter a name for the star.", "OK");
                return;
            }

            if (Planets.Select(planet => planet.Name).Contains(Name))
            {
                await DisplayAlert("Already Exists", "There is already planet with this name.", "OK");
                return;
            }

            Star star = await database.GetItemAsync<Star>(star => star.Name == Name) ?? await starsService.GetStar(Name);
            await database.SaveItemAsync(star);
            await UpdateStarsAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await DisplayAlert("Name Required", "Please enter a name of the object.", "OK");
                return;
            }

            Star star = Stars.Where(star => star.Name == Name).FirstOrDefault();
            if (star is not null)
            {
                await database.DeleteItemAsync(star);
                await UpdateStarsAsync();
            }

            Planet planet = Planets.Where(planet => planet.Name == Name).FirstOrDefault();
            if (planet is not null)
            {
                await database.DeleteItemAsync(planet);
                await UpdatePlanetsAsync();
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Star star)
            {
                await Navigation.PushAsync(new StarPage(database)
                {
                    BindingContext = star
                });
            }

            if (e.CurrentSelection.FirstOrDefault() is Planet planet)
            {
                await Navigation.PushAsync(new PlanetPage(database)
                {
                    BindingContext = planet
                });
            }
        }
    }

}
