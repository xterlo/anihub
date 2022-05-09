using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using testWpf.Core;

namespace testWpf.MVVM.ViewModel
{
    public class RelatedReleaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;



        private string _title_ru;
        public string title_ru
        {
            get { return _title_ru; }
            set { _title_ru = value; 
            OnPropertyChanged(nameof(title_ru));}
        }

        private string _imageSourceLink;
        public string imageSourceLink
        {
            get {  return $@"https://static.anixart.tv/posters/{_imageSourceLink}.jpg"; }
            set
            {
                _imageSourceLink = value;
                OnPropertyChanged(nameof(imageSourceLink));
            }
        }

        private string _year;
        public string year
        {
            get { return _year;}
            set
            {
                _year = value;
                OnPropertyChanged(nameof(year));
            }
        }

        private string _id;
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }

        private ICommand _mouseClick;
        public ICommand MouseClick
        {
            get
            {
                return _mouseClick ?? (_mouseClick = new RelayCommand(PickAnimeE));
            }
        }


        public async void PickAnimeE()
        {
            //Console.WriteLine("PICKED");
            ResponseHandler response = new ResponseHandler();
            await response.GetRelease(id);
            MainViewModel.Instance.CurrentView = new ReleaseViewModel();
        }


        public RelatedReleaseModel(RelatedReleases.RelatedContent related)
        {
            title_ru = related.title_ru;
            year = related.year;
            imageSourceLink = related.poster;
            id = related.id;
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
