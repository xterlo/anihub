using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testWpf.Core;
using Vlc.DotNet.Wpf;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace testWpf.MVVM.ViewModel
{
    internal class HomeViewModel : ReleaseInfo, INotifyPropertyChanged
    {
        private VlcControl _VideoView = new VlcControl();
        public event PropertyChangedEventHandler PropertyChanged;
        private  ReleaseInfo _releaseInfo = TemplatePreferens.releaseInfo;
        private string _title;

        public ICommand loadedFunction { get; set; }

        private ObservableCollection<hotReleaseViewModel> _hotReleaseModel;
        public ObservableCollection<hotReleaseViewModel> hotReleaseModel
        {
            get { return _hotReleaseModel; }
            set
            {
                _hotReleaseModel = value;
                OnPropertyChanged(nameof(hotReleaseModel));
            }
        }

        private ObservableCollection<RelatedReleaseModel> _ongoingsReleaseModel;
        public ObservableCollection<RelatedReleaseModel> ongoingsReleaseModel
        {
            get { return _ongoingsReleaseModel; }
            set
            {
                _ongoingsReleaseModel = value;
                OnPropertyChanged(nameof(ongoingsReleaseModel));
            }
        }

        public VlcControl VideoView
        {
            get { return _VideoView; }
            set { _VideoView=value; 
                OnPropertyChanged(nameof(VideoView)); }
        }

        public string title
        {
            get { return _title; }
            set {
                _title = value;
                OnPropertyChanged(nameof(title));    
            }
        }
        private string _videoSource;
        public string videoSource
        {
            get { return _videoSource; }
            set
            {
                _videoSource = value;
                OnPropertyChanged(nameof(videoSource));
            }
        }

        public ReleaseInfo releaseInfo
        {
            get { return _releaseInfo; }
            set { _releaseInfo = value; } 
        }

        public HomeViewModel()
        {
            Console.WriteLine("PERESHEL");
            VideoView = new VlcControl();
            loadedFunction = new RelayCommand(()=>
            {
                Console.WriteLine("LOADED");
                VideoView = new VlcControl();
                if (VideoView.SourceProvider.MediaPlayer is null)
                    StartVideo();
                else VideoView.SourceProvider.MediaPlayer.SetPause(false);
                VideoView.Unloaded += Example2_Unloaded;
            });
            hotReleaseModel = new ObservableCollection<hotReleaseViewModel>();
            ongoingsReleaseModel = new ObservableCollection<RelatedReleaseModel>();
            ResponseHandler resp = new ResponseHandler();
            changeTitle();

            var animes = resp.getHotAnimes(15).Result;
            for (int i = 0; i < 3; i++)
            {
                hotReleaseModel.Add(new hotReleaseViewModel(animes[i].name, animes[i].url, $"https://shikimori.one/{animes[i].image.preview}"));
            }
            foreach (var anime in animes)
            {
                
                RelatedReleases related = TemplatePreferens.relatedReleases;
                RelatedReleaseModel relatedReleaseModelR = new RelatedReleaseModel(anime);
                ongoingsReleaseModel.Add(relatedReleaseModelR);

            }

        }

        public async void StartVideo(bool nextEpisod = false)
        {
            var youtube = new YoutubeClient();
            StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync("https://www.youtube.com/embed/h_iYEoLmgww");
            videoSource = streamManifest.Streams[1].Url;
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            VideoView.SourceProvider.CreatePlayer(libDirectory);
            var media = new Uri("https://rr6---sn-oxuctoxu-n8ve.googlevideo.com/videoplayback?expire=1654136750&ei=TsuXYqKSJcGM7ATO0ZvQBw&ip=176.193.215.228&id=o-AOQovdvNMP4OgsixEx6p_MnyBoty2nGm9iIE73vKUNVu&itag=18&source=youtube&requiressl=yes&mh=-I&mm=31%2C29&mn=sn-oxuctoxu-n8ve%2Csn-oxuctoxu-n8vl&ms=au%2Crdu&mv=u&mvi=6&pcm2cms=yes&pl=27&vprv=1&mime=video%2Fmp4&gir=yes&clen=28235932&ratebypass=yes&dur=337.757&lmt=1651358236713838&mt=1654114311&fvip=6&fexp=24001373%2C24007246&c=ANDROID&txp=4538232&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cgir%2Cclen%2Cratebypass%2Cdur%2Clmt&sig=AOq0QJ8wRQIgFomtvXoVwx6PW3fbRHPUFPGm54nt_X_1h3aKhtWzJBoCIQDnVt0wrzPxxfpQknzxkE-werJ39np6uK-hGfPUoI4A-w%3D%3D&lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpcm2cms%2Cpl&lsig=AG3C_xAwRgIhAN-K7lJKsnN27sGtSBSY6Y8rwX63owVO5J7OOrX1Y1FmAiEAsCrkBca8gGBTaulojLFN5T8_0b4YsIbh-7xjAxoMA6E%3D");
            VideoView.SourceProvider.MediaPlayer.Play(videoSource);
            VideoView.SourceProvider.MediaPlayer.Audio.Volume = 0;
            VideoView.SourceProvider.MediaPlayer.SetPause(false);
        }

        private void Example2_Unloaded(object sender, RoutedEventArgs e)
        {
            if (VideoView.SourceProvider.MediaPlayer is null)
            {
                //VideoView.SourceProvider.MediaPlayer.Stop();
                (sender as VlcControl).SourceProvider.MediaPlayer.Dispose();
            }
            //Console.WriteLine("UNLOADED");
            //
            (sender as VlcControl).Dispose();
            VideoView.Unloaded -= Example2_Unloaded;

            //VideoView.Dispose();
            //VideoView.SourceProvider.MediaPlayer.SetPause(true);
        }

        public async void changeTitle()
        {

        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
