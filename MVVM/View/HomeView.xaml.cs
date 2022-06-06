using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using testWpf.Core;
using testWpf.MVVM.ViewModel;
using System.Windows.Interactivity;

namespace testWpf.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            UserActivity.activityWindow = UserActivity.ActivityWindow.Home;
            //pizdaPleer.Source = new Uri("https://rr6---sn-oxuctoxu-n8ve.googlevideo.com/videoplayback?expire=1654136750&ei=TsuXYqKSJcGM7ATO0ZvQBw&ip=176.193.215.228&id=o-AOQovdvNMP4OgsixEx6p_MnyBoty2nGm9iIE73vKUNVu&itag=18&source=youtube&requiressl=yes&mh=-I&mm=31%2C29&mn=sn-oxuctoxu-n8ve%2Csn-oxuctoxu-n8vl&ms=au%2Crdu&mv=u&mvi=6&pcm2cms=yes&pl=27&vprv=1&mime=video%2Fmp4&gir=yes&clen=28235932&ratebypass=yes&dur=337.757&lmt=1651358236713838&mt=1654114311&fvip=6&fexp=24001373%2C24007246&c=ANDROID&txp=4538232&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cgir%2Cclen%2Cratebypass%2Cdur%2Clmt&sig=AOq0QJ8wRQIgFomtvXoVwx6PW3fbRHPUFPGm54nt_X_1h3aKhtWzJBoCIQDnVt0wrzPxxfpQknzxkE-werJ39np6uK-hGfPUoI4A-w%3D%3D&lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpcm2cms%2Cpl&lsig=AG3C_xAwRgIhAN-K7lJKsnN27sGtSBSY6Y8rwX63owVO5J7OOrX1Y1FmAiEAsCrkBca8gGBTaulojLFN5T8_0b4YsIbh-7xjAxoMA6E%3D");
        }

        private async void PickSerialAsync(object sender, MouseButtonEventArgs e)
        {
            
            
            MainViewModel.Instance.CurrentView = new ReleaseViewModel("3333");
            //object releaseViewContent = this.Resources["ReleaseViewContent"];
            //this.Content = releaseViewContent;
        }


    }
}
