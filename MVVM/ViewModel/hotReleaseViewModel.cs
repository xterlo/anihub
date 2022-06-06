using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace testWpf.MVVM.ViewModel
{
    public class hotReleaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _animeName;
        public string animeName
        {
            get { return _animeName; }
            set { _animeName = value;
                OnPropertyChanged(nameof(animeName));
            }
        }


        private string _animeInfo;
        public string animeInfo
        {
            get { return _animeInfo; }
            set
            {
                _animeInfo = value;
                OnPropertyChanged(nameof(animeInfo));
            }
        }

        private ImageSource _hotPoster;
        public ImageSource hotPoster
        {
            get
            {
                return _hotPoster;
            }
            set
            {
                _hotPoster = value;
                OnPropertyChanged(nameof(hotPoster));
            }
        }

        public hotReleaseViewModel(string animeName, string animeInfo,string url)
        {
            this.animeName = animeName;
            this.animeInfo = animeInfo;
            setPoster(url);
        }

        private async void setPoster(string url)
        {
            var httpClient = new HttpClient();
            var responseStream = await httpClient.GetStreamAsync(url);
            var bitmapImage = new BitmapImage();

            using (var memoryStream = new MemoryStream())
            {
                await responseStream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            hotPoster = bitmapImage;
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
