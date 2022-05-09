using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using LibVLCSharp.Shared;
using testWpf.Core;
using testWpf.MVVM.View;
using testWpf.MVVM.ViewModel;
using Vlc.DotNet.Wpf;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace testWpf
{
    /// <summary>
    /// Логика взаимодействия для Pleer.xaml
    /// </summary>
    public partial class Pleer : Window
    {
        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource hwndSource;
        public string videoUrl;

        private enum ResizeDirection
        {
            Left = 61441,
            Right = 61442,
            Top = 61443,
            TopLeft = 61444,
            TopRight = 61445,
            Bottom = 61446,
            BottomLeft = 61447,
            BottomRight = 61448,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void Window1_SourceInitialized(object sender, EventArgs e)
        {
            hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)direction, IntPtr.Zero);
        }
        protected void ResetCursor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private VlcControl VideoView;
        public bool fullscreen = false;
        public bool isPlaying = false;
        public bool isDoubleClicked = false;
        public static Thread ThreadControlsHidden;
        public static bool isControlsHidden;
        Properties properties;
        public static Dictionary<string, bool> isNeedUpdateProperties = new Dictionary<string, bool>
        {
            { "position",false },
            { "time",false },
            { "pause",false }
        };
        public bool isProgressBarActive = false;
        private WindowState _oldWindowState;
        private Size _oldWindowSize;

        public class Properties : INotifyPropertyChanged
        {
            public Properties(IntegratedPleer.Properties prop)
            {
                this.voicesId = prop.voicesId;
                this.winPos = prop.winPos;
                this.winTime = prop.winTime;
                this.lastEpisode = prop.lastEpisode;
                this.firstEpisode = prop.firstEpisode;
                this.videoLength = prop.videoLength;
                this.urlvideo = prop.urlvideo;
                this.urlroom = prop.urlroom;
                this.isShared = prop.isShared;
                this.isOwner = prop.isOwner;
                this.episode = prop.episode;
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

            private int _lastEpisode;
            public int lastEpisode
            {
                get { return _lastEpisode; }
                set
                {
                    _lastEpisode = value;
                }

            }

            private int _firstEpisode;
            public int firstEpisode
            {
                get { return _firstEpisode; }
                set
                {
                    _firstEpisode = value;
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

            public void updateUrlRoom(string room)
            {
                this.urlroom = room;
            }
        }
        public Pleer(string url, IntegratedPleer.Properties p)
        {
            videoUrl = url;
            SourceInitialized += Window1_SourceInitialized;
            InitializeComponent();
            properties = new Properties(p);
            ChangeVideoLength();
            ChangeVideoTime();
            moveCircleAndProgressbar();
            animeName.Content = TemplatePreferens.releaseInfo.GetTitleRu();
            properties.PropertyChanged += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    UIHandler(s, e);
                }
                    )
                );
            };
            
            VideoView = TemplatePreferens.VideoView;
            ControlContainer.Content = VideoView;
            this.MouseDown += new MouseButtonEventHandler(async (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    await Task.Delay(150);
                    if (!isDoubleClicked)
                    {
                        if (e.LeftButton == MouseButtonState.Released)
                        {
                            togglePlay();
                        }
                        if (e.LeftButton == MouseButtonState.Pressed)
                            DragMove();
                    }
                    else if (isDoubleClicked) isDoubleClicked = false;
                }

            });
            this.MouseMove += new MouseEventHandler(progressVideoBarMove);
            this.MouseMove += new MouseEventHandler(hideControls);
            this.MouseUp += new MouseButtonEventHandler(progressVideoBarClickReleased);
            this.KeyDown += new KeyEventHandler(ShortcutEvent);
            this.MouseDoubleClick += new MouseButtonEventHandler((s, e) =>
            {
                isDoubleClicked = true;
                WindowStateToggle();
                togglePlay();
            });

            //VideoView.Loaded += (sender, e) =>
            //{
            //    StartVideo();
            //};

            VideoView.SourceProvider.MediaPlayer.LengthChanged += (sender, e) =>
            {
                properties.videoLength = VideoView.SourceProvider.MediaPlayer.Length;
            };

            VideoView.SourceProvider.MediaPlayer.PositionChanged += (sender, e) =>
            {
                properties.winPos = VideoView.SourceProvider.MediaPlayer.Position;
            };
            VideoView.SourceProvider.MediaPlayer.TimeChanged += (sender, e) =>
            {
                properties.winTime = VideoView.SourceProvider.MediaPlayer.Time;
            };

            Unloaded += Example2_Unloaded;

        }

        private void OnClosed(object sender, RoutedEventArgs e)
        {
            TemplatePreferens.VideoView = VideoView;
            if (properties.isShared)
                WatchingRoomModel.Instance.pleerWindow = new IntegratedPleer(properties);
            else
                ReleaseViewModel.Instance.pleerWindow = new IntegratedPleer(properties);
            Close();
            //Close();
            //DiscoveryView asd = new DiscoveryView();
            //integratedPleer.ControlContainer.Content = asd;
            //OnClosed(new EventArgs());
        }

        public async void UIHandler(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "winTime":
                    ChangeVideoTime();
                    break;
                case "winPos":
                    moveCircleAndProgressbar();
                    break;
                case "videoLength":
                    ChangeVideoLength();
                    break;
                case "episode":
                    if (VideoView.IsLoaded)
                    {
                        ResponseHandler resp = new ResponseHandler();
                        string kodikUrl = await resp.GetSeriaLink(resp.getEpisodesUrl, TemplatePreferens.releaseInfo.GetId().ToString(), properties.voicesId, properties.episode.ToString());
                        videoUrl = await resp.GetVideoUrl(kodikUrl);
                        StartVideo();
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
                    break;
            }
        }
        private void ChangeVideoTime()
        {
            timePassed.Content = TimeSpan.FromMilliseconds(properties.winTime).ToString(@"mm\:ss");
        }

        private void ChangeVideoLength()
        {
            DateTime now = new DateTime(properties.videoLength);
            timeTotal.Content = TimeSpan.FromMilliseconds(properties.videoLength).ToString(@"mm\:ss");
        }

        //UPDATE POTOM POMENYAT
        private void UpdateProperties()
        {
            ChangeVideoLength();
            ChangeVideoTime();
            moveCircleAndProgressbar();
        }

        public void StartVideo()
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            VideoView.SourceProvider.CreatePlayer(libDirectory);
            var media = new Uri(videoUrl);
            VideoView.SourceProvider.MediaPlayer.Play(media);
        }

        public void ShortcutEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F || (e.Key == Key.Escape && fullscreen))
            {
                WindowStateToggle();

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
            if (WindowState == WindowState.Normal)
            {
                _oldWindowSize = new Size(Width, Height);
                _oldWindowState = WindowState;
                //ResizeMode = ResizeMode.NoResize;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                fullscreen = true;

            }
            else
            {
                WindowState = _oldWindowState;
                fullscreen = false;
                //ResizeMode = ResizeMode.CanResize;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            //ReleaseViewModel.isPleerNeedOpen = false;
            //VideoView.Dispose();
            //Close();
            //integratedPleer.ControlContainer.Content = VideoView;
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            VideoView.SourceProvider.MediaPlayer.Pause();
        }

        private void progressVideoBarClickPressed(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                moveCircleAndProgressbar(e);
            }
        }

        private void progressVideoBarMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(sender);
            if (sender == progressVideoBar && e.LeftButton == MouseButtonState.Pressed)
            {
                VideoView.SourceProvider.MediaPlayer.SetPause(true);
                moveCircleAndProgressbar(null, e);
                isProgressBarActive = true;
            }
            else if (sender != progressVideoBar && isProgressBarActive)
            {
                moveCircleAndProgressbar(null, e);
            }
        }

        private void moveCircleAndProgressbar(MouseButtonEventArgs e = null, MouseEventArgs ev = null)
        {
            float progress;

            if (e != null)
            {
                progress = (float)(1 / (progressVideoBar.ActualWidth / e.GetPosition(progressVideoBar).X));
                VideoView.SourceProvider.MediaPlayer.Position = progress;
            }
            else if (ev != null)
            {
                progress = (float)(1 / (progressVideoBar.ActualWidth / ev.GetPosition(progressVideoBar).X));
                VideoView.SourceProvider.MediaPlayer.Position = progress;
            }
            else
                progress = properties.winPos;
            float pos = properties.winPos;

            LinearGradientBrush LGB = new LinearGradientBrush();
            LGB.StartPoint = new Point(0, 0);
            LGB.EndPoint = new Point(1, 1);
            LGB.GradientStops.Add(new GradientStop(Color.FromArgb(255, 252, 44, 94), pos));
            LGB.GradientStops.Add(new GradientStop(Color.FromArgb(130, 214, 213, 217), pos));
            progressVideoBar.Fill = LGB;
            Canvas.SetLeft(asddsa, (progressVideoBar.ActualWidth * progress) - 5);
        }

        private void progressVideoBarClickReleased(object sender, MouseButtonEventArgs e)
        {
            if (VideoView.SourceProvider.MediaPlayer.State ==Vlc.DotNet.Core.Interops.Signatures.MediaStates.Paused && isProgressBarActive)
            {
                VideoView.SourceProvider.MediaPlayer.SetPause(false);
                isProgressBarActive = false;
            }
        }

        private async void togglePlay()
        {
            if (VideoView.SourceProvider.MediaPlayer.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
            {
                CustomAnimation anim = new CustomAnimation(playPauseToggle);
                await anim.SvgAnimation(anim.easeOut, 200, SvgsAnimation.SvgsAnimation.playPause, Color.FromArgb(255, 194, 194, 194), Color.FromArgb(255, 252, 90, 129));
                VideoView.SourceProvider.MediaPlayer.SetPause(true);
            }
            else
            {
                CustomAnimation anim = new CustomAnimation(playPauseToggle);
                await anim.SvgAnimation(anim.easeOut, 200, SvgsAnimation.SvgsAnimation.playPause, Color.FromArgb(255, 194, 194, 194), Color.FromArgb(255, 252, 90, 129),true);
                VideoView.SourceProvider.MediaPlayer.SetPause(false);
            }
        }

        private void VideoNormalizer(object sender, SizeChangedEventArgs e)
        {

            Height = (ActualWidth * 9) / 16;
        }

        protected void Resize(object sender, MouseButtonEventArgs e)
        {
            var clickedShape = sender as Shape;

            switch (clickedShape.Name)
            {
                case "ResizeW":
                    this.Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "ResizeE":
                    this.Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                default:
                    break;
            }
        }
        protected void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            var clickedShape = sender as Shape;

            switch (clickedShape.Name)
            {
                case "ResizeE":
                case "ResizeW":
                    this.Cursor = Cursors.SizeWE;
                    break;
                default:
                    break;
            }
        }

        private async void hideControls(object sender, MouseEventArgs e)
        {
            if (sender == controlsGrid) ThreadControlsHidden?.Abort();
            else
            {
                var cildren = controlsGrid.Children;
                foreach (object c in cildren)
                {
                    if (c is Label) (c as Label).Visibility = Visibility.Visible;
                    if (c is Button) (c as Button).Visibility = Visibility.Visible;
                    if (c is Canvas) (c as Canvas).Visibility = Visibility.Visible;
                    if (c is Rectangle) (c as Rectangle).Visibility = Visibility.Visible;
                    this.Cursor = Cursors.Arrow;
                    animeName.Visibility = Visibility.Visible;
                }
                nextEpisodeBut.Visibility = Visibility.Visible;
                prevEpisodeBut.Visibility = Visibility.Visible;
                closeButton.Visibility = Visibility.Visible;
                ThreadControlsHidden?.Abort();
                ThreadControlsHidden = new Thread(checkControls);
                ThreadControlsHidden.Start();
                await Task.Delay(2000);
                //bool check = await checkControls(isControlsHidden);
                if (isControlsHidden)
                {
                    foreach (object c in cildren)
                    {
                        isControlsHidden = false;
                        animeName.Visibility = Visibility.Hidden;
                        if (c is Label) (c as Label).Visibility = Visibility.Hidden;
                        if (c is Button) (c as Button).Visibility = Visibility.Hidden;
                        if (c is Canvas) (c as Canvas).Visibility = Visibility.Hidden;
                        if (c is Rectangle) (c as Rectangle).Visibility = Visibility.Hidden;
                        this.Cursor = Cursors.None;
                    }
                    nextEpisodeBut.Visibility = Visibility.Hidden;
                    prevEpisodeBut.Visibility = Visibility.Hidden;
                    closeButton.Visibility = Visibility.Hidden;
                }
            }                    
        }

        private async void checkControls()
        {
            Thread.Sleep(1500);
            isControlsHidden = true;
        }

        private void NextEpisode(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"EPISODE:{properties.episode}, LastEpisode:{properties.lastEpisode} ");
            if (properties.episode+1 <= properties.lastEpisode)
                properties.episode++;
             
        }

        private void PrevEpisode(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"EPISODE:{properties.episode}, FIRSTEPISODE:{properties.firstEpisode} ");
            if (properties.episode-1 >= properties.firstEpisode)
                properties.episode--;
        }
    }
}