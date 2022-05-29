using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testWpf.Core;
using static testWpf.Core.ResponseSearch;

namespace testWpf.MVVM.ViewModel
{


    public class SearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _title_ru;
        private string _description;
        private string _id;
        private string _poster;

        public string title_ru
        {
            get { return _title_ru; }
            set
            {
                _title_ru = value;
                OnPropertyChanged(nameof(_title_ru));
            }
        }
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(_id));
            }
        }
        public string description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(_description));
            }
        }
        public string poster
        {
            get { return $"https://static.anixart.tv/posters/{_poster}.jpg"; }
            set
            {
                _poster = value;
                OnPropertyChanged(nameof(_poster));
            }
        }
        public static string idForRelease;

        public SearchViewModel(Response r = null)
        {

            title_ru = r.title_ru;
            description = r.description;
            id = r.id;
            poster = r.poster;
            idForRelease = id;

        }

        private ICommand _mouseClick;

        public ICommand MouseClick
        {
            get
            {
                return _mouseClick ?? (_mouseClick = new RelayCommand(pickAnime));
            }
        }


        public void pickAnime()
        {
            MainViewModel.Instance.CurrentView = new ReleaseViewModel(id);
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}