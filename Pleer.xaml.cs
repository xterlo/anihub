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
using testWpf.Core;
using testWpf.MVVM.View;
using testWpf.MVVM.ViewModel;
using Vlc.DotNet.Wpf;

namespace testWpf
{
    /// <summary>
    /// Логика взаимодействия для Pleer.xaml
    /// </summary>
    public partial class Pleer : Window
    {
        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource hwndSource;
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

        private WindowState _oldWindowState;
        private Size _oldWindowSize;

        public Pleer(IntegratedPleer pleer)
        {
            InitializeComponent();
            PleerContainer.Content = pleer;
        }

        //private void OnClosed(object sender, RoutedEventArgs e)
        //{
        //    TemplatePreferens.VideoView = VideoView;
        //    if (properties.isShared)
        //        WatchingRoomModel.Instance.pleerWindow = new IntegratedPleer(properties);
        //    else
        //        ReleaseViewModel.Instance.pleerWindow = new IntegratedPleer(properties);
        //    Close();
        //    //Close();
        //    //DiscoveryView asd = new DiscoveryView();
        //    //integratedPleer.ControlContainer.Content = asd;
        //    //OnClosed(new EventArgs());
        //}

        //public void ShortcutEvent(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.F || (e.Key == Key.Escape && fullscreen))
        //    {
        //        WindowStateToggle();

        //        //timePassed.Content = prop.winTime.ToString();
        //        //progressVideoBar.Fill = new LinearGradientBrush(); ;
        //    }

        //    if (e.Key == Key.Space)
        //    {
        //        togglePlay();

        //    }
        //    if (e.Key == Key.J)
        //    {

        //        //_mediaPlayer.SeekTo(TimeSpan.FromSeconds(360));
        //        //_mediaPlayer.Position -= 0.01f;
        //        //VideoFrame videoFrame = new VideoFrame(BitmapPixelFormat.Bgra8, (int)previewProperties.Width, (int)previewProperties.Height);


        //    }
        //    if (e.Key == Key.L)
        //    {
        //        VideoView.SourceProvider.MediaPlayer.Position += 0.01f;
        //    }

        //}

        //private void WindowStateToggle(object sender = null, RoutedEventArgs e = null)
        //{
        //    if (WindowState == WindowState.Normal)
        //    {
        //        _oldWindowSize = new Size(Width, Height);
        //        _oldWindowState = WindowState;
        //        //ResizeMode = ResizeMode.NoResize;
        //        WindowStyle = WindowStyle.None;
        //        WindowState = WindowState.Maximized;
        //        fullscreen = true;

        //    }
        //    else
        //    {
        //        WindowState = _oldWindowState;
        //        fullscreen = false;
        //        //ResizeMode = ResizeMode.CanResize;
        //    }
        //}

        //protected override void OnClosed(EventArgs e)
        //{
        //    //ReleaseViewModel.isPleerNeedOpen = false;
        //    //VideoView.Dispose();
        //    //Close();
        //    //integratedPleer.ControlContainer.Content = VideoView;
        //}

        //private void VideoNormalizer(object sender, SizeChangedEventArgs e)
        //{

        //    Height = (ActualWidth * 9) / 16;
        //}

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
    }
}