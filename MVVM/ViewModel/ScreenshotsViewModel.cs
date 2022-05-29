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
using testWpf.Dialogs.MediaViewer;
using testWpf.Dialogs.Service;

namespace testWpf.MVVM.ViewModel
{
    public class ScreenshotsViewModel : INotifyPropertyChanged
    {
        private IDialogService _dialogService;

        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand _mouseClick;
        public ICommand MouseClick
        {
            get
            {
                return _mouseClick ?? (_mouseClick = new RelayCommand(pickScreenshot));
            }
        }

        private int _position;
        public int position
        {
            get { return _position; }
            set { _position = value; }
        }

        private ImageSource _imageSourceLink;
        public ImageSource imageSourceLink
        {
            get { return _imageSourceLink; }
            set
            {
                _imageSourceLink = value;
                OnPropertyChanged(nameof(imageSourceLink));
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

        public void pickScreenshot()
        {
            var dialog = new MediaViewerModel(position, TemplatePreferens.releaseInfo.GetScreenshots());
            var result = _dialogService.OpenDialog(dialog);
            Console.WriteLine(result);
        }

        public ScreenshotsViewModel(int pos)
        {
            _dialogService = new DialogService();
            position = pos;
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Tick += (sender, args) =>
            {
                AnimateTemplates();
            };
            _timer.Start();
            setPoster($@"https://static.anixart.tv/screenshots/{TemplatePreferens.releaseInfo.GetScreenshots()[position]}.jpg");
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

            imageSourceLink = bitmapImage;
            TemplateImage = Visibility.Collapsed;
            _timer.Stop();
        }


        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
