﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using testWpf.Core;
using testWpf.MVVM.ViewModel;
using Vlc.DotNet.Wpf;

namespace testWpf.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для IntegratedPleer.xaml
    /// </summary>
    /// 
    public static class RemoveChildHelper
    {
        public static void RemoveChild(this DependencyObject parent, UIElement child)
        {
            var panel = parent as Panel;
            if (panel != null)
            {
                panel.Children.Remove(child);
                return;
            }

            var decorator = parent as Decorator;
            if (decorator != null)
            {
                if (decorator.Child == child)
                {
                    decorator.Child = null;
                }
                return;
            }

            var contentPresenter = parent as ContentPresenter;
            if (contentPresenter != null)
            {
                if (contentPresenter.Content == child)
                {
                    contentPresenter.Content = null;
                }
                return;
            }

            var contentControl = parent as ContentControl;
            if (contentControl != null)
            {
                if (contentControl.Content == child)
                {
                    contentControl.Content = null;
                }
                return;
            }
        }
    }
    public partial class IntegratedPleer : UserControl
    {

        public string videoUrl;
        private VlcControl VideoView;
        public Pleer pleerWindow;
        public bool fullscreen = false;
        public bool isPlaying = false;
        public bool isDoubleClicked = false;
        public static Thread ThreadControlsHidden;
        public static bool isControlsHidden;
        public static Properties properties;
        public static Dictionary<string, bool> isNeedUpdateProperties = new Dictionary<string, bool>
        {
            { "position",false },
            { "time",false },
            { "pause",false },
            { "episode",false }
        };
        public bool isProgressBarActive = false;
        public bool isProgressBarActiveClick = false;


        public class Properties : INotifyPropertyChanged
        {
            public Properties(WatchingRoomModel.Properties prop)
            {
                this.voicesId = prop.voicesId;
                this.winPos = prop.winPos;
                this.winTime = prop.winTime;
                this.episodes = prop.episodes;
                this.videoLength = prop.videoLength;
                this.urlvideo = prop.urlvideo;
                this.urlroom = prop.urlroom;
                this.isShared = prop.isShared;
                this.isOwner = prop.isOwner;
                this.episode = prop.episode;
                this.lockPause = prop.lockPause;
            }

            public Properties(string url,string VID, string ep, ObservableCollection<string> episodes)
            {
                ObservableCollection<int> episodes_int = new ObservableCollection<int>();
                this.voicesId = VID;
                foreach (var episode in episodes)
                    episodes_int.Add(int.Parse(episode));
                this.episodes = episodes_int;
                this.urlvideo = url;
                this.isShared = false;
                episode = int.Parse(ep);
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private float _winPos;

            public float winPos
            {
                get { return _winPos; }
                set
                {
                    _winPos = value;
                    OnPropertyChanged(nameof(winPos));
                }
            }

            private long _winTime;
            public long winTime
            {
                get { return _winTime; }
                set
                {
                    _winTime = value;
                    OnPropertyChanged(nameof(winTime));

                }

            }

            private long _videoLength;
            public long videoLength
            {
                get { return _videoLength; }
                set
                {
                    _videoLength = value;
                    OnPropertyChanged(nameof(videoLength));

                }

            }

            private string _voicesId;
            public string voicesId
            {
                get { return _voicesId; }
                set
                {
                    _voicesId = value;

                }

            }

            private int _episode;
            public int episode
            {
                get { return _episode; }
                set
                {
                    _episode = value;
                    OnPropertyChanged(nameof(episode));
                }

            }

            private ObservableCollection<int> _episodes;
            public ObservableCollection<int> episodes
            {
                get { return _episodes; }
                set
                {
                    _episodes = value;
                }

            }

            private string _urlvideo;
            public string urlvideo
            {
                get { return _urlvideo; }
                set
                {
                    _urlvideo = value;
                }

            }

            private string _urlroom;
            public string urlroom
            {
                get { return _urlroom; }
                set
                {
                    _urlroom = value;
                }

            }

            private bool _isPause;
            public bool isPause
            {
                get { return _isPause; }
                set
                {
                    _isPause = value;
                    OnPropertyChanged(nameof(isPause));
                }

            }

            private bool _isShared;
            public bool isShared
            {
                get { return _isShared; }
                set
                {
                    _isShared = value;
                }

            }

            private bool _isOwner;
            public bool isOwner
            {
                get { return _isOwner; }
                set
                {
                    _isOwner = value;
                }

            }

            private bool _lockPause;
            public bool lockPause
            {
                get { return _lockPause; }
                set
                {
                    _lockPause = value;
                    OnPropertyChanged(nameof(lockPause));
                }

            }

            public void updateValues(WatchingRoomModel.Properties prop)
            {
                this.videoLength = prop.videoLength;
                this.winPos = prop.winPos;
                this.winTime = prop.winTime;
                this.isPause = prop.isPause;
                isNeedUpdateProperties["position"] = true;
                isNeedUpdateProperties["time"] = true;
                isNeedUpdateProperties["pause"] = true;
            }

            public void setPause(SocketPauseInformation info)
            {
                isNeedUpdateProperties["pause"] = true;
                isNeedUpdateProperties["position"] = true;
                this.winPos = info.positionState;
                this.isPause = info.pauseState;
            }
            public void LockPause(bool locked)
            {
                this.lockPause = locked;
            }

            public void updatePosition(float position)
            {
                this.winPos = position;
                isNeedUpdateProperties["position"] = true;
            }

            public void updateUrlRoom(string room)
            {
                this.urlroom = room;
            }

            public void updateEpisode(int episode)
            {
                this.episode = episode;
                isNeedUpdateProperties["episode"] = true;
            }
        }
        public IntegratedPleer(string url, string VID, string ep, ObservableCollection<string> episodes)
        {
            properties = new Properties(url,VID,ep, episodes);
            Initialization();
            DateTime now = new DateTime(properties.videoLength);
            timeTotal.Content = TimeSpan.FromMilliseconds(properties.videoLength).ToString(@"mm\:ss");
        }
        public IntegratedPleer(Properties prop)
        {
            properties = prop;
            Initialization();
        }
        public IntegratedPleer(WatchingRoomModel.Properties prop)
        {
            properties = new Properties(prop);
            Initialization();
        }
        public void Initialization()
        {
            InitializeComponent();
            initVideoView();
            
            properties.PropertyChanged += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    UIHandler(s, e);
                }));
            };
            this.MouseDown += new MouseButtonEventHandler(async (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed && s.GetType() != typeof(Rectangle))
                {
                    await Task.Delay(150);
                    if (!isDoubleClicked)
                    {
                        if (e.LeftButton == MouseButtonState.Released)
                        {
                            togglePlay();
                        }
                    }
                    else if (isDoubleClicked) isDoubleClicked = false;
                }

            });
            //this.MouseMove += new MouseEventHandler(progressVideoBarMove);
            this.MouseMove += new MouseEventHandler(hideControls);
            this.KeyDown += new KeyEventHandler(ShortcutEvent);
            this.MouseDoubleClick += new MouseButtonEventHandler((s, e) =>
            {
                isDoubleClicked = true;
                //WindowStateToggle();
                togglePlay();
            });

            if (!VideoView.IsLoaded)
            {
                VideoView.Loaded += (sender, e) =>
                {

                    StartVideo();
                    VideoView.SourceProvider.MediaPlayer.SetPause(true);
                    VideoView.SourceProvider.MediaPlayer.LengthChanged += (s, ev) =>
                    {
                        properties.videoLength = VideoView.SourceProvider.MediaPlayer.Length;
                    };

                    VideoView.SourceProvider.MediaPlayer.PositionChanged += (s, ev) =>
                    {
                        
                        properties.winPos = VideoView.SourceProvider.MediaPlayer.Position;
                    };
                    VideoView.SourceProvider.MediaPlayer.TimeChanged += (s, ev) =>
                    {

                        properties.winTime = VideoView.SourceProvider.MediaPlayer.Time;
                    };
                    if (properties.isShared)
                    {
                        if (!properties.isOwner)
                        {
                            SocketManager.GetPleerProperties(properties.urlroom);
                        }
                    }

                };
                VideoView.Unloaded += Example2_Unloaded;
            }
            else
            {
                UpdateProperties();
            }
        }


        public void initVideoView()
        {
            VideoView = TemplatePreferens.VideoView;
            VideoView.Background = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
            ControlContainer.Content = VideoView;
        }
        public async void UIHandler(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "winTime":
                    if (isNeedUpdateProperties["time"])
                    {
                        isNeedUpdateProperties["time"] = false;
                        VideoView.SourceProvider.MediaPlayer.Time = properties.winTime;
                    }
                    long time = VideoView.SourceProvider.MediaPlayer.Time;
                    timePassed.Content = TimeSpan.FromMilliseconds(time).ToString(@"mm\:ss");
                    break;
                case "winPos":
                    if (isNeedUpdateProperties["position"])
                    {
                        isNeedUpdateProperties["position"] = false;
                        VideoView.SourceProvider.MediaPlayer.Position = properties.winPos;
                    }
                    SliderProgress.Value = VideoView.SourceProvider.MediaPlayer.Position;
                    break;
                case "videoLength":
                    DateTime now = new DateTime(properties.videoLength);
                    timeTotal.Content = TimeSpan.FromMilliseconds(properties.videoLength).ToString(@"mm\:ss");
                    break;
                case "episode":
                    if (VideoView.IsLoaded)
                    {
                        ResponseHandler resp = new ResponseHandler(TemplatePreferens.releaseInfo.GetId().ToString());
                        string kodikUrl = await resp.GetSeriaLink(resp.getEpisodesUrl, properties.voicesId, properties.episode.ToString());
                        properties.urlvideo = await resp.GetVideoUrl(kodikUrl);
                        StartVideo(true);
                    }
                    break;
                case "isPause":
                    if (properties.isShared)
                    {
                        if (isNeedUpdateProperties["pause"])
                        {
                            isNeedUpdateProperties["pause"] = false;
                            VideoView.SourceProvider.MediaPlayer.SetPause(properties.isPause);
                        }
                        //else SocketManager.SetPause(properties);
                    }
                    CustomAnimation anim = new CustomAnimation(playPauseToggle);
                    await anim.SvgAnimation(anim.easeOut, 500, SvgsAnimation.SvgsAnimation.playPause, Color.FromArgb(255, 194, 194, 194), Color.FromArgb(255, 252, 90, 129),!properties.isPause);
                    break;
                case "lockPause":
                    if (properties.isShared)
                    {
                        if (properties.lockPause)
                        {
                            VideoView.SourceProvider.MediaPlayer.SetPause(true);
                        }
                        else VideoView.SourceProvider.MediaPlayer.SetPause(properties.isPause);
                    }
                    break;
            }


        }

        private void UpdateProperties()
        {
            timePassed.Content = TimeSpan.FromMilliseconds(properties.winTime).ToString(@"mm\:ss");
            DateTime now = new DateTime(properties.videoLength);
            timeTotal.Content = TimeSpan.FromMilliseconds(properties.videoLength).ToString(@"mm\:ss");
            //moveCircleAndProgressbar();
        }

        public void StartVideo(bool nextEpisod = false)
        {
            Console.WriteLine("StartVideoStart");
            //VideoView  = _mediaPlayer;
            if (TemplatePreferens.VideoView.SourceProvider.MediaPlayer == null)
            {
                var currentAssembly = Assembly.GetEntryAssembly();
                var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
                var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
                VideoView.SourceProvider.CreatePlayer(libDirectory);
                var media = new Uri(properties.urlvideo);
                VideoView.SourceProvider.MediaPlayer.Play(media);
                VideoView.SourceProvider.MediaPlayer.Audio.Volume= 20;
                

            }
            if (nextEpisod && !isNeedUpdateProperties["episode"])
            {
                if (properties.isShared)
                    SocketManager.ChangeEpisode(properties.episode);
                VideoView.SourceProvider.MediaPlayer.Play(new Uri(properties.urlvideo));

            }
            else if (nextEpisod && isNeedUpdateProperties["episode"])
            {
                VideoView.SourceProvider.MediaPlayer.Play(new Uri(properties.urlvideo));
                isNeedUpdateProperties["episode"] = false;
            }
            animeName.Content = TemplatePreferens.releaseInfo.GetTitleRu() + " ⚫ " + properties.episode.ToString();

            //_mediaPlayer.Volume = 30;


        }

        public void ShortcutEvent(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.F || (e.Key == Key.Escape && fullscreen))
            {
                
                //WindowStateToggle();
                //timePassed.Content = prop.winTime.ToString();
                //progressVideoBar.Fill = new LinearGradientBrush(); ;
            }

            if (e.Key == Key.Space)
            {
                togglePlay();

            }
            if (e.Key == Key.J)
            {

                //_mediaPlayer.SeekTo(TimeSpan.FromSeconds(360));
                //_mediaPlayer.Position -= 0.01f;
                //VideoFrame videoFrame = new VideoFrame(BitmapPixelFormat.Bgra8, (int)previewProperties.Width, (int)previewProperties.Height);


            }
            if (e.Key == Key.L)
            {
                VideoView.SourceProvider.MediaPlayer.Position += 0.01f;
            }

        }

        private void Example2_Unloaded(object sender, RoutedEventArgs e)
        {
            //VideoView.SourceProvider.MediaPlayer.Stop();
            //VideoView.SourceProvider.MediaPlayer.Dispose();
            //VideoView.Dispose();
        }

        void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoView.SourceProvider.MediaPlayer.IsPlaying())
            {
                VideoView.SourceProvider.MediaPlayer.Stop();

            }
        }

        void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            togglePlay();
        }

        private void WindowStateToggle(object sender = null, RoutedEventArgs e = null)
        {
            if (!fullscreen)
            {
                fullscreen = true;
                TemplatePreferens.VideoView = VideoView;
                pleerWindow = new Pleer(this);
                pleerWindow.Show();

            }
            else
            {
                pleerWindow?.Close();
                fullscreen = false;
                if (properties.isShared)
                    WatchingRoomModel.Instance.pleerWindow = new IntegratedPleer(properties);
                else
                    ReleaseViewModel.Instance.pleerWindow = new IntegratedPleer(properties);
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            VideoView.SourceProvider.MediaPlayer.Pause();
        }

        private void moveCircleAndProgressbar(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (isProgressBarActive || (!isProgressBarActive && (((sender as Slider).IsMouseOver && Mouse.LeftButton == MouseButtonState.Pressed) && !isProgressBarActiveClick)))
            {
                VideoView.SourceProvider.MediaPlayer.Position = (float)(sender as Slider).Value;
                if(properties.isShared)
                    SocketManager.changeVideoTime((sender as Slider).Value);
                isProgressBarActive = false;
            }
        }
        private void startedSlider(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isProgressBarActive = false;
            isProgressBarActiveClick = true;
            VideoView.SourceProvider.MediaPlayer.SetPause(true);
        }

        private void endedSlider(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

            if (properties.isShared)
            {
                properties.lockPause = true;
                SocketManager.changeVideoTime((sender as Slider).Value);
            }
            VideoView.SourceProvider.MediaPlayer.Position = (float)(sender as Slider).Value;
            isProgressBarActive = true;
            isProgressBarActiveClick = false;
            VideoView.SourceProvider.MediaPlayer.SetPause(properties.isPause);

        }
        private async void togglePlay()
        {
            if (VideoView.SourceProvider.MediaPlayer.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
            {
                properties.isPause = true;
                if(properties.isShared)
                    SocketManager.SetPause(properties);
                VideoView.SourceProvider.MediaPlayer.SetPause(true);
                //CustomAnimation anim = new CustomAnimation(playPauseToggle);
                //await anim.SvgAnimation(anim.easeOut, 500, SvgsAnimation.SvgsAnimation.playPause, Color.FromArgb(255, 194, 194, 194), Color.FromArgb(255, 252, 90, 129));
                
                
            }
            else
            {
                properties.isPause = false;
                if (properties.isShared)
                    SocketManager.SetPause(properties);
                VideoView.SourceProvider.MediaPlayer.SetPause(false);
                //CustomAnimation anim = new CustomAnimation(playPauseToggle);
                //await anim.SvgAnimation(anim.easeOut, 500, SvgsAnimation.SvgsAnimation.playPause, Color.FromArgb(255, 194, 194, 194), Color.FromArgb(255, 252, 90, 129), true);
                
            }
        }

        private void VideoNormalizer(object sender, SizeChangedEventArgs e)
        {
            Height = (ActualWidth * 9) / 16;
        }
        private Point _lastMove;
        private async void hideControls(object sender, MouseEventArgs e)
        {
            if (sender == controlsGrid) ThreadControlsHidden?.Abort();
            else
            {
                Console.WriteLine(sender);
                var p = e.GetPosition((IInputElement)sender);
                if (_lastMove != p)
                {
                    var cildren = controlsGrid.Children;
                    foreach (object c in cildren)
                    {
                        if (c is Label) (c as Label).Visibility = Visibility.Visible;
                        if (c is Button) (c as Button).Visibility = Visibility.Visible;
                        if (c is Canvas) (c as Canvas).Visibility = Visibility.Visible;
                        if (c is Rectangle) (c as Rectangle).Visibility = Visibility.Visible;
                        if (c is Slider) (c as Slider).Visibility = Visibility.Visible;
                        this.Cursor = Cursors.Arrow;
                        animeName.Visibility = Visibility.Visible;
                    }
                    nextEpisodeBut.Visibility = Visibility.Visible;
                    prevEpisodeBut.Visibility = Visibility.Visible;
                    closeButton.Visibility = Visibility.Visible;

                    ThreadControlsHidden = new Thread(checkControls);

                    ThreadControlsHidden.Start();
                    await Task.Delay(2000);
                    //bool check = await checkControls(isControlsHidden);

                    if (isControlsHidden && sender != controlsGrid)
                    {
                        isControlsHidden = false;

                        foreach (object c in cildren)
                        {

                            animeName.Visibility = Visibility.Hidden;
                            if (c is Label) (c as Label).Visibility = Visibility.Hidden;
                            if (c is Button) (c as Button).Visibility = Visibility.Hidden;
                            if (c is Canvas) (c as Canvas).Visibility = Visibility.Hidden;
                            if (c is Rectangle) (c as Rectangle).Visibility = Visibility.Hidden;
                            if (c is Slider) (c as Slider).Visibility = Visibility.Hidden;
                            this.Cursor = Cursors.None;
                        }
                        nextEpisodeBut.Visibility = Visibility.Hidden;
                        prevEpisodeBut.Visibility = Visibility.Hidden;
                        closeButton.Visibility = Visibility.Hidden;
                    }
                    _lastMove = p;
                }
            }
        }

        private void checkControls()
        {
            Thread.Sleep(1500);
            isControlsHidden = true;
        }

        //private void OnClosed(object sender, RoutedEventArgs e)
        //{
        //    OnClosed(new EventArgs());
        //}

        private void NextEpisode(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"EPISODE:{properties.episode}, LastEpisode:{properties.episodes.Last()} ");
            if (properties.episode + 1 <= properties.episodes.Last())
                properties.episode++;

        }

        private void PrevEpisode(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"EPISODE:{properties.episode}, FIRSTEPISODE:{properties.episodes.First()} ");
            if (properties.episode - 1 >= properties.episodes.First())
                properties.episode--;
        }



        private void soundSliderChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoView.SourceProvider.MediaPlayer.Audio.Volume = (int)(sender as Slider).Value;
            Console.WriteLine((int)(sender as Slider).Value);
        }
    }
}
