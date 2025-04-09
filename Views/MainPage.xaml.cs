using Stargazer.Database;
using Stargazer.Database.Entities;
using Stargazer.ViewModels;

namespace Stargazer
{
    public partial class MainPage : ContentPage
    {
        private CelestialDatabase database;
        public List<Star> Stars { get; set; }

        public MainPage(CelestialDatabase celestialDatabase)
        {
            InitializeComponent();
            database = celestialDatabase;
            BindingContext = new StarsViewModel();
        }

        void OnNameTextChanged(object sender, TextChangedEventArgs e)
        {
            string oldText = e.OldTextValue;
            string newText = e.NewTextValue;
            string myText = Name.Text;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                await DisplayAlert("Name Required", "Please enter a name for the todo item.", "OK");
                return;
            }

            Star star = await database.FindItemAsync((Star star) => star.Name == Name.Text) ?? new(Name.Text);

            await database.SaveItemAsync(star);
            await Shell.Current.GoToAsync("..");
        }
    }

}
