using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using testWpf.Core;
using testWpf.MVVM.View;
using static testWpf.Core.Series;
using static testWpf.Core.Voices;

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
            set { _pleerWindow = value;
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
        private string _ReleasePoster;

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
        public string ReleasePoster
        {
            get
            {
                return $@"https://static.anixart.tv/posters/{_ReleasePoster}.jpg"; ;
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
            set { _grade = value; 
                OnPropertyChanged(nameof(grade));}
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
            set { _comboboxVoicesData = value; }
        }

        private ObservableCollection<string> _comboboxEpisodesData;
        public ObservableCollection<string> comboboxEpisodesData
        {
            get { return _comboboxEpisodesData; }
            set { _comboboxEpisodesData = value; }
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

        public ReleaseViewModel( )
        {
            Instance = this;
            WatchButtonClickCommand = new RelayCommand<object>(WatchButtonClick);
            TogetherButtonClickCommand = new RelayCommand<object>(TogetherButtonClick);
            comboboxVoicesData = new ObservableCollection<string>();
            comboboxEpisodesData = new ObservableCollection<string>();
            releaseInfo = TemplatePreferens.releaseInfo;
            ReleaseName = releaseInfo.GetTitleRu();
            ReleaseOriginalName = releaseInfo.GetTitleOriginal();
            ReleaseDescription = releaseInfo.GetDescription();
            ReleasePoster = releaseInfo.GetPoster();
            relatedReleaseModel = new ObservableCollection<RelatedReleaseModel>();
            screenshotsViewModel = new ObservableCollection<ScreenshotsViewModel>();
            getReleases();
            getVoices();
            addScreenshots();
            getGrades();
        }

        public async void getVoices()
        {
            ResponseHandler resp = new ResponseHandler();
            voices = await resp.GetVoice(resp.getEpisodesUrl, releaseInfo.GetId().ToString());
            foreach(Voice voice in voices.result)
            {
                comboboxVoicesData.Add(voice.name);
            }
        }

        public async void getReleases()
        {
            relatedReleaseModel.Clear();
            ReleaseInfo relaseInfo = TemplatePreferens.releaseInfo;
            ResponseHandler resp = new ResponseHandler();
            RelatedReleases related = await resp.GetRelated(relaseInfo.GetRelated());
            foreach (var r in related.related_content)
            {
                ContentControl contentControl = new ContentControl();
                RelatedReleaseModel relatedReleaseModelR = new RelatedReleaseModel(r);
                relatedReleaseModel.Add(relatedReleaseModelR);
            }
        }

        public void addScreenshots()
        {
            int count=0;
            foreach (var s in TemplatePreferens.releaseInfo.GetScreenshots())
            {
                screenshotsViewModel.Add(new ScreenshotsViewModel(count));
                count++;
            }
        }

        public void getGrades()
        {
            var a = TemplatePreferens.releaseInfo.GetGrades();
            grade = a[0].Remove(3,a[0].Length-3);
            double maxWidth = 300;
            vote1 = gradesNormalizer((double.Parse(a[1]) / double.Parse(a[6])) * maxWidth);
            vote2 = gradesNormalizer((double.Parse(a[2]) / double.Parse(a[6])) * maxWidth);
            vote3 = gradesNormalizer((double.Parse(a[3]) / double.Parse(a[6])) * maxWidth);
            vote4 = gradesNormalizer((double.Parse(a[4]) / double.Parse(a[6])) * maxWidth);
            vote5 = gradesNormalizer((double.Parse(a[5]) / double.Parse(a[6])) * maxWidth);

            //Console.WriteLine("s");
        }

        public double gradesNormalizer(double vote)
        {
            if (vote < 10) return 0;
            else return vote;
        }

        private async void WatchButtonClick(object notUsed = null)
        {
            if(SelectedEpisodeItem != null && SelectedVoiceItem != null)
            {
                
                string kodikUrl = await resp.GetSeriaLink(resp.getEpisodesUrl, releaseInfo.GetId().ToString(), voicesID.vID[0].id, _selectedEpisodeItem);
                string videoUrl = await resp.GetVideoUrl(kodikUrl);
                isPleerNeedOpen = true;
                pleerWindow = new IntegratedPleer(videoUrl, voicesID.vID[0].id, _selectedEpisodeItem,comboboxEpisodesData.Last(),comboboxEpisodesData.First());

            }
        }

        private async void TogetherButtonClick(object notUsed = null)
        {
            if (SelectedEpisodeItem != null && SelectedVoiceItem != null)
            {
                if (searchData is null || searchData.Length < 1)
                {
                    string kodikUrl = await resp.GetSeriaLink(resp.getEpisodesUrl, releaseInfo.GetId().ToString(), voicesID.vID[0].id, _selectedEpisodeItem);
                    string videoUrl = await resp.GetVideoUrl(kodikUrl);
                    var a = new WatchingRoomModel.Properties(videoUrl, voicesID.vID[0].id, int.Parse(_selectedEpisodeItem), int.Parse(comboboxEpisodesData.Last()), int.Parse(comboboxEpisodesData.First()),true);
                    MainViewModel.Instance.CurrentView = new WatchingRoomModel(a);
                }

            }
            else if(searchData != null || searchData.Length > 1)
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
                    voicesID = await resp.GetVoiceID(resp.getEpisodesUrl, releaseInfo.GetId().ToString(), selectedVoice.id);
                    series = await resp.GetSeries(resp.getEpisodesUrl, releaseInfo.GetId().ToString(),selectedVoice.id,voicesID.vID[0].id);
                    foreach(Positions pos in series.episodes)
                    {
                        comboboxEpisodesData.Add(pos.position.ToString());
                    }
                }
            }
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}



