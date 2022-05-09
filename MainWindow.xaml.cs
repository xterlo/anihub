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
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowStateToggle();
        }

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
                _oldWindowSize = new Size(Width, Height);
                _oldWindowState = WindowState;
                ResizeMode = ResizeMode.NoResize;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                
            }
            else
            {
                WindowState = _oldWindowState;
                ResizeMode = ResizeMode.CanResize;
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
