using Stargazer.Database;
using Stargazer.Database.Models;
using Stargazer.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Stargazer.Views
{
    public partial class MainPage : ContentPage
    {
        public string Name { get; set; } = string.Empty;

        // Planets
        public ObservableCollection<Planet> Planets { get; set; } = [];
        public int PlanetsCount { get; set; } = 0;

        public ObservableCollection<int> PlanetsPages { get; set; } = [];
        public int PlanetsCurrentPage
        {
            get; set
            {
                if (field != value)
                    field = value;
            }
        } = 1;
        public string PlanetsSearchQuery { get; set; } = string.Empty;
        public List<string> PlanetsSortKeys { get; set; } = ["Name", "Mass", "Radius", "LastModified"];
        public string PlanetsSortKey { get; set; } = "Name";
        public bool PlanetsSortAsc { get; set; } = true;

        // Stars
        public ObservableCollection<Star> Stars { get; set; } = [];
        public int StarsCount { get; set; } = 0;

        public ObservableCollection<int> StarsPages { get; set; } = [];
        public int StarsCurrentPage
        {
            get; set
            {
                if (field != value)
                    field = value;
            }
        } = 1;
        public string StarsSearchQuery { get; set; } = string.Empty;
        public List<string> StarsSortKeys { get; set; } = ["Name", "Constellation", "SpectralClass", "LastModified"];
        public string StarsSortKey { get; set; } = "Name";
        public bool StarsSortAsc { get; set; } = true;

        private CelestialDatabase database;
        private IStarsService starsService;
        private IPlanetsService planetsService;

        public MainPage(CelestialDatabase celestialDatabase, IStarsService _starsService, IPlanetsService _planetsService)
        {
            InitializeComponent();
            database = celestialDatabase;
            starsService = _starsService;
            planetsService = _planetsService;
            BindingContext = this;
        }

        private List<int> GetPages(List<int> allPages, int currentPage)
        {
            if (allPages.Count > 5)
            {
                List<int> pages = [allPages.First()];
                int min = Math.Clamp(currentPage - 3, 2, allPages.Last() - 1);
                int max = Math.Clamp(currentPage + 3, 2, allPages.Last() - 1);
                pages.AddRange(Enumerable.Range(min, max - min + 1));
                pages.Add(allPages.Last());
                return pages;
            }

            return allPages;
        }

        private async Task UpdatePlanetsPages()
        {
            PlanetsCount = PlanetsSearchQuery != string.Empty
                ? await database.CountItemsAsync<Planet>(planet => planet.Name.ToLower().Contains(PlanetsSearchQuery.ToLower()))
                : await database.CountItemsAsync<Planet>();

            var allPages = Enumerable.Range(1, (int)Math.Ceiling((decimal)PlanetsCount / Constants.PageSize)).ToList();

            if (allPages.Count == 0)
                allPages.Add(1);

            if (allPages.Last() < PlanetsCurrentPage)
                PlanetsCurrentPage = allPages.Last();

            var pages = GetPages(allPages, PlanetsCurrentPage);

            Trace.WriteLine($"{PlanetsCount}, [{string.Join(", ", allPages)}], [{string.Join(", ", pages)}]");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                PlanetsPages.Clear();
                foreach (int page in pages)
                    PlanetsPages.Add(page);
            });
        }

        private async Task UpdatePlanetsAsync()
        {
            await UpdatePlanetsPages();

            var planets = PlanetsSearchQuery != string.Empty
                ? await database.GetItemsAsync<Planet>(PlanetsCurrentPage - 1, PlanetsSortKey ?? "Name", PlanetsSortAsc, planet => planet.Name.ToLower().Contains(PlanetsSearchQuery.ToLower()))
                : await database.GetItemsAsync<Planet>(PlanetsCurrentPage - 1, PlanetsSortKey ?? "Name", PlanetsSortAsc);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Planets.Clear();
                foreach (var planet in planets)
                    Planets.Add(planet);
            });
        }

        private async Task UpdateStarsPages()
        {
            StarsCount = StarsSearchQuery != string.Empty
                ? await database.CountItemsAsync<Star>(star => star.Name.ToLower().Contains(StarsSearchQuery.ToLower()))
                : await database.CountItemsAsync<Star>();

            var allPages = Enumerable.Range(1, (int)Math.Ceiling((decimal)StarsCount / Constants.PageSize)).ToList();

            if (allPages.Count == 0)
                allPages.Add(1);

            if (allPages.Last() < StarsCurrentPage)
                StarsCurrentPage = allPages.Last();

            var pages = GetPages(allPages, StarsCurrentPage);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                StarsPages.Clear();
                foreach (int page in pages)
                    StarsPages.Add(page);
            });
        }

        private async Task UpdateStarsAsync()
        {
            await UpdateStarsPages();

            var stars = StarsSearchQuery != string.Empty
                ? await database.GetItemsAsync<Star>(StarsCurrentPage - 1, StarsSortKey ?? "Name", StarsSortAsc, star => star.Name.ToLower().Contains(StarsSearchQuery.ToLower()))
                : await database.GetItemsAsync<Star>(StarsCurrentPage - 1, StarsSortKey ?? "Name", StarsSortAsc);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Stars.Clear();
                foreach (var star in stars)
                    Stars.Add(star);
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await UpdatePlanetsAsync();
            await UpdateStarsAsync();
        }

        private async void OnPlanetsPageChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdatePlanetsAsync();
        }

        private async void OnPlanetsSortChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not string sortKey)
                return;

            if (PlanetsSortKey == sortKey)
                PlanetsSortAsc = !PlanetsSortAsc;
            else
                PlanetsSortKey = sortKey;

            PlanetsCurrentPage = 1;
            await UpdatePlanetsAsync();
            ((CollectionView)sender).SelectedItem = null;
        }

        private async void OnPlanetSearchCompleted(object sender, EventArgs e)
        {
            PlanetsCurrentPage = 1;
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

        private async void OnStarsPageChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateStarsAsync();
        }

        private async void OnStarsSortChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not string sortKey)
                return;

            if (StarsSortKey == sortKey)
                StarsSortAsc = !StarsSortAsc;
            else
                StarsSortKey = sortKey;

            StarsCurrentPage = 1;
            await UpdateStarsAsync();
            ((CollectionView)sender).SelectedItem = null;
        }

        private async void OnStarSearchCompleted(object sender, EventArgs e)
        {
            StarsCurrentPage = 1;
            await UpdateStarsAsync();
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

            Planet planet = await database.GetItemAsync<Planet>(planet => planet.Name == Name);
            if (planet is not null)
            {
                await database.DeleteItemAsync(planet);
                await UpdatePlanetsAsync();
            }

            Star star = await database.GetItemAsync<Star>(star => star.Name == Name);
            if (star is not null)
            {
                await database.DeleteItemAsync(star);
                await UpdateStarsAsync();
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
