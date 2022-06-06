using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using testWpf.Core;
using testWpf.MVVM.View;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using static testWpf.Core.Series;
using static testWpf.Core.Voices;
using Color = System.Windows.Media.Color;
using LinearGradientBrush = System.Windows.Media.LinearGradientBrush;

namespace testWpf.MVVM.ViewModel
{
    public class ReleaseViewModel : INotifyPropertyChanged
    {
        public static ReleaseViewModel Instance;


        Voices voices;
        RelatedRelease related;
        Voice selectedVoice;
        VoicesId voicesID;
        Series series;
        GetKodikUrl kodikUrl;


        ResponseHandler resp = new ResponseHandler();

        private IntegratedPleer _pleerWindow;
        public IntegratedPleer pleerWindow
        {
            get { return _pleerWindow; }
            set
            {
                _pleerWindow = value;
                OnPropertyChanged(nameof(pleerWindow));
            }
        }
        public ICommand WatchButtonClickCommand { get; set; }
        public ICommand TogetherButtonClickCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static bool isPleerNeedOpen;


        private ReleaseInfo _releaseInfo;
        public ReleaseInfo releaseInfo
        {
            get { return _releaseInfo; }
            set { _releaseInfo = value; }

        }

        private string _searchData;

        public string searchData
        {
            get { return _searchData; }
            set
            {
                _searchData = value;
                OnPropertyChanged(nameof(searchData));
            }
        }

        #region ReleaseInfo

        private string _ReleaseDescription;
        private string _ReleaseName;
        private string _ReleaseOriginalName;
        private ImageSource _ReleasePoster;

        public string ReleaseDescription
        {
            get
            {
                return _ReleaseDescription;
            }
            set
            {
                _ReleaseDescription = value;
                OnPropertyChanged(nameof(ReleaseDescription));
            }
        }
        public string ReleaseName
        {
            get
            {
                return _ReleaseName;
            }
            set
            {
                _ReleaseName = value;
                OnPropertyChanged(nameof(ReleaseName));
            }
        }

        public string ReleaseOriginalName
        {
            get
            {
                return _ReleaseOriginalName;
            }
            set
            {
                _ReleaseOriginalName = value;
                OnPropertyChanged(nameof(ReleaseOriginalName));
            }
        }
        public ImageSource ReleasePoster
        {
            get
            {
                return _ReleasePoster ;
            }
            set
            {
                _ReleasePoster = value;
                OnPropertyChanged(nameof(ReleasePoster));
            }
        }
        #endregion

        #region Related
        private ObservableCollection<RelatedReleaseModel> _relatedReleaseModel;
        public ObservableCollection<RelatedReleaseModel> relatedReleaseModel
        {
            get { return _relatedReleaseModel; }
            set
            {
                _relatedReleaseModel = value;
                OnPropertyChanged(nameof(relatedReleaseModel));
            }
        }
        #endregion

        #region Screenshots
        private ObservableCollection<ScreenshotsViewModel> _screenshotsViewModel;
        public ObservableCollection<ScreenshotsViewModel> screenshotsViewModel
        {
            get { return _screenshotsViewModel; }
            set
            {
                _screenshotsViewModel = value;
                OnPropertyChanged(nameof(screenshotsViewModel));
            }
        }
        #endregion

        #region Grades

        private string _grade;
        public string grade
        {
            get { return _grade; }
            set
            {
                _grade = value;
                OnPropertyChanged(nameof(grade));
            }
        }

        private double _vote1;
        public double vote1
        {
            get { return _vote1; }
            set
            {
                _vote1 = value;
                OnPropertyChanged(nameof(vote1));
            }
        }

        private double _vote2;
        public double vote2
        {
            get { return _vote2; }
            set
            {
                _vote2 = value;
                OnPropertyChanged(nameof(vote2));
            }
        }

        private double _vote3;
        public double vote3
        {
            get { return _vote3; }
            set
            {
                _vote3 = value;
                OnPropertyChanged(nameof(vote3));
            }
        }

        private double _vote4;
        public double vote4
        {
            get { return _vote4; }
            set
            {
                _vote4 = value;
                OnPropertyChanged(nameof(vote4));
            }
        }

        private double _vote5;
        public double vote5
        {
            get { return _vote5; }
            set
            {
                _vote5 = value;
                OnPropertyChanged(nameof(vote5));
            }
        }

        #endregion

        #region ComboBoxes

        private ObservableCollection<string> _comboboxVoicesData;
        public ObservableCollection<string> comboboxVoicesData
        {
            get { return _comboboxVoicesData; }
            set { _comboboxVoicesData = value;
                OnPropertyChanged(nameof(comboboxVoicesData));
            }
        }

        private ObservableCollection<string> _comboboxEpisodesData;
        public ObservableCollection<string> comboboxEpisodesData
        {
            get { return _comboboxEpisodesData; }
            set { _comboboxEpisodesData = value;
                OnPropertyChanged(nameof(comboboxEpisodesData));
            }
        }
        private string _selectedVoiceItem;
        public string SelectedVoiceItem
        {
            get
            {
                return _selectedVoiceItem;
            }

            set
            {
                _selectedVoiceItem = value;
                GetIDAsync();


            }
        }

        private string _selectedEpisodeItem;
        public string SelectedEpisodeItem
        {
            get
            {
                return _selectedEpisodeItem;
            }

            set
            {
                _selectedEpisodeItem = value;
            }
        }
        #endregion

        #region Sizes

        private double _windowWidth;
        public double WindowWidth
        {
            set
            {
                _windowWidth = value;
                resizeX(value);
            }
        }

        private double _windowHeight;
        public double WindowHeight
        {
              set
            {
                _windowHeight = value;
                resizeY(value);
            }
        }

        private double _imageBlockWidth;
        public double ImageBlockWidth
        {
            get { return _imageBlockWidth; }

            set
            {
                _imageBlockWidth = value;
                OnPropertyChanged(nameof(ImageBlockWidth));
            }
        }

        private double _imageBlockHeight;
        public double ImageBlockHeight
        {
            get { return _imageBlockHeight; }

            set
            {
                _imageBlockHeight = value;
                OnPropertyChanged(nameof(ImageBlockHeight));
            }
        }


        public void resizeX(double width)
        {
            ImageBlockWidth = width;
        }

        public void resizeY(double height)
        {
            Console.WriteLine($"RESIZE Y {height}");

        }

        #endregion

        #region UI SETTINGS

        private bool _isEnabledEpisode = false;
        public bool isEnabledEpisode
        {
            get { return _isEnabledEpisode; }
            set
            {
                _isEnabledEpisode = value;
                if (value)
                    opacityEpisode = 1f;
                else
                    opacityEpisode = .5f;
                OnPropertyChanged(nameof(isEnabledEpisode));
            }
        }

        private float _opacityEpisode = .5f;
        public float opacityEpisode
        {
            get { return _opacityEpisode; }
            set
            {
                _opacityEpisode = value;
                OnPropertyChanged(nameof(opacityEpisode));
            }
        }

        private Visibility _templateImage = Visibility.Visible;
        public Visibility TemplateImage
        {
            get { return _templateImage;}
            set { 
                _templateImage = value;
                OnPropertyChanged(nameof(TemplateImage));
            }
        }

        private Visibility _templateReleaseText = Visibility.Visible;
        public Visibility TemplateReleaseText
        {
            get { return _templateReleaseText; }
            set
            {
                _templateReleaseText = value;
                OnPropertyChanged(nameof(TemplateReleaseText));
            }
        }

        private System.Windows.Media.LinearGradientBrush _animateTemplate;

        public System.Windows.Media.LinearGradientBrush AnimateTemplate
        {
            get { return _animateTemplate;}
            set
            {
                _animateTemplate = value;
                OnPropertyChanged(nameof(AnimateTemplate));
            }
        }

        public double stepLinearGradient = 0;
        public DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Render);
        #endregion


        public ReleaseViewModel(string releaseID)
        {
            Instance = this;
            WatchButtonClickCommand = new RelayCommand<object>(WatchButtonClick);
            TogetherButtonClickCommand = new RelayCommand<object>(TogetherButtonClick);
            resp = new ResponseHandler(releaseID);
            resp.endGetRelease += Initialization;
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Tick += (sender, args) =>
            {
                AnimateTemplates();
            };
            _timer.Start();
            
            Task.Run(()=>resp.GetRelease());
        }

        public ReleaseViewModel()
        {
            Instance = this;
        }
        public async void Initialization()
        {
            comboboxVoicesData = new ObservableCollection<string>();
            comboboxEpisodesData = new ObservableCollection<string>();
            relatedReleaseModel = new ObservableCollection<RelatedReleaseModel>();
            screenshotsViewModel = new ObservableCollection<ScreenshotsViewModel>();
            Console.WriteLine(TemplatePreferens.releaseInfo.result);
            releaseInfo = TemplatePreferens.releaseInfo;
            ReleaseName = releaseInfo.GetTitleRu();
            ReleaseOriginalName = releaseInfo.GetTitleOriginal();
            ReleaseDescription = releaseInfo.GetDescription();
            TemplateReleaseText = Visibility.Collapsed;
            Invoked(() => getRelated());
            

#pragma warning disable CS4014
            Task.Run(() =>
            {
                Invoked(() => setPoster(releaseInfo.GetPoster()));
            });
            Task.Run(() => getVoices());
            Task.Run(() =>addScreenshots());
            Task.Run(() =>
            {
                Invoked(() =>getGrades());
            });

#pragma warning restore CS4014
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
        }



        public async void getVoices()
        {
            voices = await resp.GetVoice(resp.getEpisodesUrl);
            foreach (Voice voice in voices.result)
            {
                Invoked(() => comboboxVoicesData.Add(voice.name));

            }
            Console.WriteLine("END VOICES");
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

            ReleasePoster = bitmapImage;
            TemplateImage = Visibility.Collapsed;
            _timer.Stop();
        }


        public async void getRelated()
        {
            
            relatedReleaseModel.Clear();
            RelatedReleases related = TemplatePreferens.relatedReleases;
            foreach (var r in related.related_content)
            {
                //ContentControl contentControl = new ContentControl();
                RelatedReleaseModel relatedReleaseModelR = new RelatedReleaseModel(r);
                relatedReleaseModel.Add(relatedReleaseModelR);
            }
        }

        public void addScreenshots()
        {
            int count = 0;
            foreach (var s in TemplatePreferens.releaseInfo.GetScreenshots())
            {
                Invoked(()=>screenshotsViewModel.Add(new ScreenshotsViewModel(count)));
                count++;
                
            }
            Console.WriteLine("END ADD SCREANSHOTES");
        }

        public void getGrades()
        {
            var a = TemplatePreferens.releaseInfo.GetGrades();
            grade = a[0].Remove(3, a[0].Length - 3);
            double maxWidth = 300;
            vote1 = gradesNormalizer((double.Parse(a[1]) / double.Parse(a[6])) * maxWidth);
            vote2 = gradesNormalizer((double.Parse(a[2]) / double.Parse(a[6])) * maxWidth);
            vote3 = gradesNormalizer((double.Parse(a[3]) / double.Parse(a[6])) * maxWidth);
            vote4 = gradesNormalizer((double.Parse(a[4]) / double.Parse(a[6])) * maxWidth);
            vote5 = gradesNormalizer((double.Parse(a[5]) / double.Parse(a[6])) * maxWidth);
            Console.WriteLine("END GRADES");
            //Console.WriteLine("s");
        }

        public double gradesNormalizer(double vote)
        {
            if (vote < 10) return 0;
            else return vote;
        }

        private async void WatchButtonClick(object notUsed = null)
        {
            if (SelectedEpisodeItem != null && SelectedVoiceItem != null)
            {

                string kodikUrl = await resp.GetSeriaLink(resp.getEpisodesUrl, voicesID.vID[0].id, _selectedEpisodeItem);
                string videoUrl = await resp.GetVideoUrl(kodikUrl);
                isPleerNeedOpen = true;
                pleerWindow = new IntegratedPleer(videoUrl, voicesID.vID[0].id, _selectedEpisodeItem, comboboxEpisodesData);
                //var url = "http://cloud.kodik-storage.com/useruploads/e0507793-424d-4c93-a605-f0aee3cca16c/281fdc9a21bdeebac532c582f1d79e86:2022060107/720.mp4:hls:manifest.m3u8";
                //var youtube = new YoutubeClient();
                //var streamManifest = await youtube.Videos.Streams.GetManifestAsync("https://www.youtube.com/watch?v=1La4QzGeaaQ");
                //pleerWindow = new IntegratedPleer(streamManifest.Streams[1].Url, voicesID.vID[0].id, _selectedEpisodeItem, comboboxEpisodesData);

            }
        }

        private async void TogetherButtonClick(object notUsed = null)
        {
            if (SelectedEpisodeItem != null && SelectedVoiceItem != null)
            {
                if (searchData is null || searchData.Length < 1)
                {
                    string kodikUrl = await resp.GetSeriaLink(resp.getEpisodesUrl,  voicesID.vID[0].id, _selectedEpisodeItem);
                    string videoUrl = await resp.GetVideoUrl(kodikUrl);
                    var a = new WatchingRoomModel.Properties(videoUrl, voicesID.vID[0].id, int.Parse(_selectedEpisodeItem), comboboxEpisodesData, true);
                    MainViewModel.Instance.CurrentView = new WatchingRoomModel(a);
                }

            }
            else if (searchData != null || searchData.Length > 1)
            {
                MainViewModel.Instance.CurrentView = new WatchingRoomModel(searchData);
            }

        }

        private async Task GetIDAsync(object notUsed = null)
        {
            comboboxEpisodesData.Clear();
            foreach (Voice voice in voices.result)
            {
                if (voice.name == SelectedVoiceItem)
                {
                    selectedVoice = voice;
                    voicesID = await resp.GetVoiceID(resp.getEpisodesUrl,  selectedVoice.id);
                    series = await resp.GetSeries(resp.getEpisodesUrl, selectedVoice.id, voicesID.vID[0].id);
                    foreach (Positions pos in series.episodes)
                    {
                        comboboxEpisodesData.Add(pos.position.ToString());
                    }
                    isEnabledEpisode = true;
                }
            }
        }

        public void Invoked(Action action)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                action();
            });
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}



