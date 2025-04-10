using Stargazer.Database;
using Stargazer.Database.Models;

namespace Stargazer.Views
{
    [QueryProperty("Planet", "Planet")]
    public partial class PlanetPage : ContentPage
    {
        CelestialDatabase database;

        public Planet Planet
        {
            get => BindingContext as Planet;
            set => BindingContext = value;
        }

        public PlanetPage(CelestialDatabase celestialDatabase)
        {
            InitializeComponent();
            database = celestialDatabase;
        }

        public async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Planet.Name))
            {
                await DisplayAlert("Name Required", "Please enter a name for the star.", "OK");
                return;
            }

            await database.SaveItemAsync(Planet);
            await Navigation.PopAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            await database.DeleteItemAsync(Planet);
            await Navigation.PopAsync();
        }
    }
}