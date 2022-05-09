using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
using static testWpf.Core.ResponseSearch;

namespace testWpf.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        BackgroundWorker imgLoader = new BackgroundWorker();
        public string id;
        public Search()
        {
            InitializeComponent();
            //Init($@"https://static.anixart.tv/posters/{r.poster}.jpg");
            //TitleName.Content = r.title_ru;
            //TitleDesc.Text = r.description;
            //id = r.id;
        }
        void Init(string url)
        {
            imgLoader.WorkerReportsProgress = true;
            imgLoader.DoWork += bwLoader_DoWork;
            imgLoader.ProgressChanged += bwLoader_ProgressChanged;
            imgLoader.RunWorkerAsync(url);
        }
        void bwLoader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is BitmapImage)
            {
                BitmapImage image = e.UserState as BitmapImage;
                TitlePoster.ImageSource = image;
            }
        }


        void bwLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            string path = e.Argument as string;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            var stream = new MemoryStream(new WebClient().DownloadData(path));
            src.StreamSource = stream;
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            src.Freeze();
            imgLoader.ReportProgress(0, src);
        }
    }
}
