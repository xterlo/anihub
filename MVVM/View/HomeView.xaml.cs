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
        }

        private async void PickSerialAsync(object sender, MouseButtonEventArgs e)
        {
            ResponseHandler response = new ResponseHandler();
            await response.GetRelease("3333");
            object releaseViewContent = this.Resources["ReleaseViewContent"];
            this.Content = releaseViewContent;
        }


    }
}
