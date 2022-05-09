using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using testWpf.Core;
using testWpf.MVVM.ViewModel;
using System.Threading;
using static testWpf.Core.ReleaseInfo;
using System.Net;
using System.Windows.Media.Animation;
using System.IO;


namespace testWpf.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для ReleaseView.xaml
    /// </summary>
    /// 
    public partial class ReleaseView : UserControl
    {

        private static long _timeStart;
        private static double _releaseNameRatio;
        private static double _releaseOriginalNameRatio;
        private static double _releaseDescriptionRatio;
        private double _scrollHorizontalOffsetRelated;
        private double _scrollHorizontalOffsetFragments;

        public Pleer pleerWindow;
        

        public ReleaseView()
        {

            InitializeComponent();
            UserActivity.activityWindow = UserActivity.ActivityWindow.Release;
            ScrollViewerRelated.ScrollChanged += (s, e) => {
                ScrollViewer viewer = (s as ScrollViewer);
                _scrollHorizontalOffsetRelated = viewer.HorizontalOffset;
                if (viewer.HorizontalOffset > viewer.ScrollableWidth-5)
                    scrollToRightButton.Visibility = Visibility.Hidden;
                else if (viewer.HorizontalOffset < 5)
                    scrollToLeftButton.Visibility = Visibility.Hidden;
                else
                {
                    scrollToRightButton.Visibility = Visibility.Visible;
                    scrollToLeftButton.Visibility = Visibility.Visible;
                }
            };
            scrollViewerFragment.ScrollChanged += (s, e) => {
                ScrollViewer viewer = (s as ScrollViewer);
                _scrollHorizontalOffsetFragments = viewer.HorizontalOffset;
                if (viewer.HorizontalOffset > viewer.ScrollableWidth - 5)
                    scrollToRightButtonFragment.Visibility = Visibility.Hidden;
                else if (viewer.HorizontalOffset < 5)
                    scrollToLeftButtonFragment.Visibility = Visibility.Hidden;
                else
                {
                    scrollToRightButtonFragment.Visibility = Visibility.Visible;
                    scrollToLeftButtonFragment.Visibility = Visibility.Visible;
                }
            };
            _releaseNameRatio = (40 / previewContent.MaxWidth);
            _releaseOriginalNameRatio = (23 / previewContent.MaxWidth);
            _releaseDescriptionRatio = 20 / previewContent.MaxWidth;
            _timeStart = UnixTimeNow();
        }


        private void ImageResize(object sender, SizeChangedEventArgs e)
        {
            borderImage.Width = ActualWidth * .2247191011235955;
            borderImage.Height = borderImage.Width / .75;
            firstBlockPreview.Height = ActualHeight * .9;
            if (UnixTimeNow() - _timeStart > 0)
            {
                releaseName.FontSize = previewContent.ActualWidth * _releaseNameRatio;
                releaseOriginalName.FontSize = previewContent.ActualWidth * _releaseOriginalNameRatio;
                releaseDescription.FontSize = previewContent.ActualWidth * _releaseDescriptionRatio;
                _timeStart = UnixTimeNow();
            }

        }
        public long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }

        private void ScrollButtonRight(object sender, MouseButtonEventArgs e)
        {
            ScrollAnimatedFunc(ScrollViewerRelated, 200, _scrollHorizontalOffsetRelated, 300);
        }

        private void ScrollButtonLeft(object sender, MouseButtonEventArgs e)
        {
            ScrollAnimatedFunc(ScrollViewerRelated, 200, _scrollHorizontalOffsetRelated, -300);
        }

        private void ScrollButtonRightFragment(object sender, MouseButtonEventArgs e)
        {
            ScrollAnimatedFunc(scrollViewerFragment, 200, _scrollHorizontalOffsetFragments, 510);
        }

        private void ScrollButtonLeftFragment(object sender, MouseButtonEventArgs e)
        {
            ScrollAnimatedFunc(scrollViewerFragment, 200, _scrollHorizontalOffsetFragments, -510);
        }

        private void ScrollAnimatedFunc(ScrollViewer viewer,int duration,double startOffset, double scrollOffset)
        {
            if (!CustomAnimation.isStarted)
            {
                CustomAnimation scrollAnim = new CustomAnimation(viewer);
                scrollAnim.ScrollAnimation(scrollAnim.easeOut,duration, startOffset, scrollOffset);
                
            }
        }

    }
}
