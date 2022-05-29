using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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

        private ImageSource _imageSourceLink;
        public ImageSource imageSourceLink
        {
            get {  return _imageSourceLink; }
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

        #region UI SETTINGS
        public DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Render);
        public double stepLinearGradient = 0;
        private Visibility _templateImage = Visibility.Visible;
        public Visibility TemplateImage
        {
            get { return _templateImage; }
            set
            {
                _templateImage = value;
                OnPropertyChanged(nameof(TemplateImage));
            }
        }

        private System.Windows.Media.LinearGradientBrush _animateTemplate;

        public System.Windows.Media.LinearGradientBrush AnimateTemplate
        {
            get { return _animateTemplate; }
            set
            {
                _animateTemplate = value;
                OnPropertyChanged(nameof(AnimateTemplate));
            }
        }

        #endregion

        public async void PickAnimeE()
        {
            //Console.WriteLine("PICKED");
            MainViewModel.Instance.CurrentView = new ReleaseViewModel(id);
        }

        public async void setPoster(string url)
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

            imageSourceLink = bitmapImage;
            TemplateImage = Visibility.Collapsed;
            _timer.Stop();
        }


        public RelatedReleaseModel(RelatedReleases.RelatedContent related)
        {
            title_ru = related.title_ru;
            year = related.year;
            //imageSourceLink = related.poster;
            setPoster($@"https://static.anixart.tv/posters/{related.poster}.jpg");
            id = related.id;
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Tick += (sender, args) =>
            {
                AnimateTemplates();
            };
            _timer.Start();
        }

        public void AnimateTemplates()
        {
            if (stepLinearGradient > 50)
                stepLinearGradient = 0;
            double pos = stepLinearGradient / 50;
            LinearGradientBrush LGB = new LinearGradientBrush();
            LGB.StartPoint = new System.Windows.Point(0, 0);
            LGB.EndPoint = new System.Windows.Point(1, 1);
            LGB.GradientStops.Add(new GradientStop(Color.FromArgb(255, 34, 34, 42), pos + .2));
            LGB.GradientStops.Add(new GradientStop(Color.FromArgb(150, 76, 76, 72), pos + .1));
            LGB.GradientStops.Add(new GradientStop(Color.FromArgb(255, 34, 34, 42), pos));
            AnimateTemplate = LGB;
            stepLinearGradient += 1;
            Console.WriteLine("ANIMATE");
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
