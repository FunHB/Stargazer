using Stargazer.Database;
using Stargazer.Database.Models;

namespace Stargazer.Views
{
    [QueryProperty("Star", "Star")]
    public partial class StarPage : ContentPage
    {
        CelestialDatabase database;

        public Star Star
        {
            get => BindingContext as Star;
            set => BindingContext = value;
        }

        public StarPage(CelestialDatabase celestialDatabase)
        {
            InitializeComponent();
            database = celestialDatabase;
        }

        public async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Star.Name))
            {
                await DisplayAlert("Name Required", "Please enter a name for the star.", "OK");
                return;
            }

            await database.SaveItemAsync(Star);
            await Navigation.PopAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            await database.DeleteItemAsync(Star);
            await Navigation.PopAsync();
        }
    }
}