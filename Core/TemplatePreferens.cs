using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using testWpf.MVVM.View;
using Vlc.DotNet.Wpf;

namespace testWpf.Core
{
    public class TemplatePreferens
    {
        private static ReleaseInfo _releaseInfo = new ReleaseInfo();
        private static RelatedReleases _relatedReleases = new RelatedReleases();
        private static VlcControl _VideoView = new VlcControl();
        public static VlcControl VideoView
        {
            get { return _VideoView; }
            set { _VideoView = value; }
        }

        public  static ReleaseInfo releaseInfo
        {
            get { return _releaseInfo; } 
            set { _releaseInfo = value; } 
        }
        public static RelatedReleases relatedReleases
        {
            get { return _relatedReleases; }
            set { _relatedReleases = value; }
        }

        public TemplatePreferens(ReleaseInfo r)
        {
            releaseInfo = r;
        }

    }
}
