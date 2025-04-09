using Stargazer.Database.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Stargazer.ViewModels
{
    public class StarsViewModel : INotifyPropertyChanged
    {
        readonly IList<Star> source;
        private Star selectedStar;

        public ObservableCollection<Star> Stars { get; private set; }

        public Star SelectedStar
        {
            get
            {
                return selectedStar;
            }
            set
            {
                if (selectedStar != value)
                {
                    selectedStar = value;
                }
            }
        }

        public ICommand DeleteCommand => new Command<Star>(RemoveStar);

        public StarsViewModel()
        {
            source = [];

            selectedStar = Stars.FirstOrDefault();
            OnPropertyChanged("SelectedStar");

        }

        void RemoveStar(Star star)
        {
            if (Stars.Contains(star))
            {
                Stars.Remove(star);
            }
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
