using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using testWpf.Core;
using testWpf.MVVM.View;
using testWpf.MVVM.ViewModel;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Collections.Generic;
using System.Windows.Media;
using System.Reflection;

namespace testWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private WindowState _oldWindowState;
        private Size _oldWindowSize;

        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            _oldWindowSize = new Size(Width, Height);
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void leftBar_MouseEnter(object sender, MouseEventArgs e)
        {
            //AnimOpenOrCloseLeftBar(leftBar.ActualWidth, 170, leftBar);
            //AnimOpenOrCloseLeftBar(aHomeLabel.ActualWidth, 100, HomeLabel);
            //AnimOpenOrCloseLeftBar(DiscoveryLabel.ActualWidth, 100, DiscoveryLabel);
            //AnimOpenOrCloseLeftBar(BookmarksLabel.ActualWidth, 100, BookmarksLabel);
        }

        private void leftBar_MouseLeave(object sender, MouseEventArgs e)
        {
            //AnimOpenOrCloseLeftBar(leftBar.ActualWidth, 90, leftBar);
            //AnimOpenOrCloseLeftBar(HomeLabel.ActualWidth, 10, HomeLabel);
            //AnimOpenOrCloseLeftBar(DiscoveryLabel.ActualWidth, 10, DiscoveryLabel);
            //AnimOpenOrCloseLeftBar(BookmarksLabel.ActualWidth, 10, BookmarksLabel);
        }

        internal void AnimOpenOrCloseLeftBar(double start, int end, object obj)
        {
            DoubleAnimation animLeftBar = new DoubleAnimation();
            animLeftBar.From = start;
            animLeftBar.To = end;
            animLeftBar.Duration = TimeSpan.FromSeconds(.15);
            if (obj is Label) (obj as Label).BeginAnimation(Rectangle.WidthProperty, animLeftBar);
            if (obj is Grid) (obj as Grid).BeginAnimation(Rectangle.WidthProperty, animLeftBar);
        }

        private void OnClickExitButton(object sender, RoutedEventArgs e)
        {
            //SocketManager.SocketLeaveRoom();
            Close();
        }

        private void MoveMainWindow(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized) {
                Point clickPosition = e.GetPosition(this);
                int ratioWindow = (int)((clickPosition.X * _oldWindowSize.Width) / Width);
                Left = (int)clickPosition.X- ratioWindow;
                Top = (int)clickPosition.Y;
                WindowStateToggle();
            }
            if(e.ClickCount == 2) {
                WindowStateToggle();
            }
            DragMove();

        }


        private void MinimizeWindow(object sender, RoutedEventArgs e) 
        {
            Minimize(this);
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowStateToggle();
        }

        #region TEST

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        // To get the working area of the specified monitor
        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MonitorInfoEx monitorInfo);

        private static MonitorInfoEx GetMonitorInfo(Window window, IntPtr monitorPtr)
        {
            var monitorInfo = new MonitorInfoEx();

            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            GetMonitorInfo(new HandleRef(window, monitorPtr), monitorInfo);

            return monitorInfo;
        }

        private static void Minimize(Window window)
        {
            if (window == null)
            {
                return;
            }

            window.WindowState = WindowState.Minimized;
        }

        private static void Restore(Window window)
        {
            if (window == null)
            {
                return;
            }

            window.WindowState = WindowState.Normal;
            window.ResizeMode = ResizeMode.CanResizeWithGrip;
        }

        private static void Maximize(Window window)
        {
            window.ResizeMode = ResizeMode.NoResize;

            // Get handle for nearest monitor to this window
            var wih = new WindowInteropHelper(window);

            // Nearest monitor to window
            const int MONITOR_DEFAULTTONEAREST = 2;
            var hMonitor = MonitorFromWindow(wih.Handle, MONITOR_DEFAULTTONEAREST);

            // Get monitor info
            var monitorInfo = GetMonitorInfo(window, hMonitor);

            // Create working area dimensions, converted to DPI-independent values
            var source = HwndSource.FromHwnd(wih.Handle);

            if (source?.CompositionTarget == null)
            {
                return;
            }

            var matrix = source.CompositionTarget.TransformFromDevice;
            var workingArea = monitorInfo.rcWork;

            var dpiIndependentSize =
                matrix.Transform(
                    new Point(workingArea.Right - workingArea.Left,
                              workingArea.Bottom - workingArea.Top));



            // Maximize the window to the device-independent working area ie
            // the area without the taskbar.
            window.Top = workingArea.Top;
            window.Left = workingArea.Left;

            window.MaxWidth = dpiIndependentSize.X;
            window.MaxHeight = dpiIndependentSize.Y;

            window.WindowState = WindowState.Maximized;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Monitor information (used by GetMonitorInfo())
        [StructLayout(LayoutKind.Sequential)]
        public class MonitorInfoEx
        {
            public int cbSize;
            public Rect rcMonitor; // Total area
            public Rect rcWork; // Working area
            public int dwFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
            public char[] szDevice;
        }

        #endregion


        private void WindowStateChecker(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                if (ResizeMode != ResizeMode.NoResize)
                {
                    ResizeMode = ResizeMode.NoResize;
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Normal;
                    WindowState = WindowState.Maximized;
                    Visibility = Visibility.Visible;
                }
            }
        }

        private void WindowStateToggle()
        {


            if (WindowState == WindowState.Normal)
            {
                //_oldWindowSize = new Size(Width, Height);
                //_oldWindowState = WindowState;
                //WindowState = WindowState.Maximized;
                //ResizeMode = ResizeMode.NoResize;
                //WindowStyle = WindowStyle.None;
                Maximize(this);
                
            }
            else
            {
                //WindowState = _oldWindowState;
                //ResizeMode = ResizeMode.CanResize;
                Restore(this);
            }
        }


        private void hideSearchContent(object sender, MouseEventArgs e)
        {
            searchContentScroll.Visibility = Visibility.Hidden;
        }

        private void showSearchContent(object sender, MouseEventArgs e)
        {
            searchContentScroll.Visibility= Visibility.Visible;
        }
    }
}
