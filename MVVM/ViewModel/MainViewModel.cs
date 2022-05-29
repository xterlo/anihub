
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using testWpf.Core;

namespace testWpf.MVVM.ViewModel
{
    internal class MainViewModel : ObserverObject
    {

        #region UI ICOMMANDS

        public ICommand _homeViewCommand { get; set; }
        public ICommand _discoveryViewCommand { get; set; }
        public ICommand _releaseViewCommand { get; set; }
        public ICommand testNewFicha { get; set; }
        public ICommand _unchecked { get; set; }
        public ICommand _showPickWtg { get; set; }
        public ICommand _hidePickWtg { get; set; }
        public ICommand _connectToRoom { get; set; }

        #endregion

        #region UI RelayCommands 
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand ReleaseViewCommand { get; set; }
        public HomeViewModel HomeVm { get; set; }
        public DiscoveryViewModel DiscoveryVm { get; set; }
        public ReleaseViewModel ReleaseVm { get; set; }
        public WatchingRoomModel WatchVm { get; set; }

        #endregion

        #region UI PROPERTIES

        private float _opacityWindow = 1f;
        public float OpacityWindow
        {
            get { return _opacityWindow; }
            set { 
                _opacityWindow = value; 
                OnPropertyChanged(nameof(OpacityWindow));
            }
        }

        private Visibility _visibilityWtg = Visibility.Collapsed;
        public Visibility VisibilityWtg
        {
            get { return _visibilityWtg; }
            set {
                _visibilityWtg = value;
                OnPropertyChanged(nameof(VisibilityWtg));
            }
        }

        private bool isEnabledMainWindow = true;
        public bool IsEnabledMainWindow
        {
            get { return isEnabledMainWindow; }
            set { 
                isEnabledMainWindow = value;
                OnPropertyChanged(nameof(isEnabledMainWindow));
            }
        }

        private string _roomid;
        public string Roomid
        {
            get { return _roomid; }
            set {
                _roomid = value;
                OnPropertyChanged(nameof(Roomid));
            }
        }
        #endregion

        private ObservableCollection<SearchViewModel> _searchViewModel;
        public ObservableCollection<SearchViewModel> searchViewModel
        {
            get { return _searchViewModel; }
            set
            {
                _searchViewModel = value;
                OnPropertyChanged(nameof(searchViewModel));
            }
        }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        private string _searchData;

        public string searchData
        {
            get { return _searchData; }
            set
            {
                _searchData = value;
                OnPropertyChanged(nameof(searchData));
                new Thread(() => SearchChanged()).Start();
                //Application.Current.Dispatcher.BeginInvoke(
                //  DispatcherPriority.Background,
                //  new Action());
            }
        }

        public async void SearchChanged()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                searchViewModel?.Clear();
            });
            Query product = new Query(_searchData);
            ResponseHandler response = new ResponseHandler();
            ResponseSearch res = await response.postRequestAsync(product);
            foreach (ResponseSearch.Response a in res.contentt)
            {
                SearchViewModel _search = new SearchViewModel(a);
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    searchViewModel.Add(_search);
                });
                
            }
        }
        public static MainViewModel Instance;

        public async void animateIcons(Path svg,bool reverse=false)
        {
            CustomAnimation customAnimation = new CustomAnimation(svg);
            svg.Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#00ffffff");
            System.Windows.Media.Color fillStart = System.Windows.Media.Color.FromArgb(0, 246, 178, 69);
            System.Windows.Media.Color fillEnd = System.Windows.Media.Color.FromArgb(255, 246, 178, 69);
            System.Windows.Media.Color strokeStart = System.Windows.Media.Color.FromArgb(255, 225, 232, 235);
            System.Windows.Media.Color strokeEnd = System.Windows.Media.Color.FromArgb(0, 225, 232, 235);
            await customAnimation.SvgAnimation(customAnimation.easeOut, 300, fillStart, fillEnd, strokeStart, strokeEnd, reverse);
        }

        private void showPickWtg()
        {
            OpacityWindow = .4f;
            VisibilityWtg = Visibility.Visible;
            IsEnabledMainWindow = false;
        }

        private void hidePickWtg()
        {
            OpacityWindow = 1f;
            VisibilityWtg = Visibility.Collapsed;
            IsEnabledMainWindow = true;
        }

        private void connectToRoom()
        {
            ResponseHandler responseHandler = new ResponseHandler();
            string status = Task.Run(async () => await responseHandler.CheckRoomExist(Roomid)).Result;
            if (status == "ServerError")
            {
                MessageBox.Show(" Нет связи с сервером", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Convert.ToBoolean(status))
            {
                MessageBox.Show("Данной комнаты не существует", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                hidePickWtg();
                CurrentView = new WatchingRoomModel(Roomid);
            }
        }

        public MainViewModel()
        {
            Instance = this;
            testNewFicha = new RelayCommand<object>(o =>
            {
                Grid border = ((o as Border).Child as Grid);
                foreach (object child in border.Children)
                {
                    if (child is RadioButton) {
                        RadioButton btn = child as RadioButton;
                        btn.IsChecked = true;
                        }
                    if (child is Path)
                    {
                        Path svg = (child as Path);
                        animateIcons(svg);

                    }

                }

            });
            _unchecked = new RelayCommand<object>(o =>
            {
                RadioButton rbtn = o as RadioButton;
                if ((rbtn.Parent as Grid).Children[0] is Path)
                { 
                    Path svg = (rbtn.Parent as Grid).Children[0] as Path;
                    animateIcons(svg, true);
                }

            });
            searchViewModel = new ObservableCollection<SearchViewModel>();
            _showPickWtg = new RelayCommand(showPickWtg);
            _hidePickWtg = new RelayCommand(hidePickWtg);
            _connectToRoom = new RelayCommand(connectToRoom);
            HomeVm = new HomeViewModel();
            DiscoveryVm = new DiscoveryViewModel();
            ReleaseVm = new ReleaseViewModel();
            CurrentView = HomeVm;

            _homeViewCommand = new RelayCommand<HomeViewModel>(o =>
            {
                Console.WriteLine("HOME");
                CurrentView = HomeVm;
            });

            _discoveryViewCommand = new RelayCommand<DiscoveryViewModel>(o =>
           {
               Console.WriteLine("DISCOVERY");
               CurrentView = DiscoveryVm;
           });

            _releaseViewCommand = new RelayCommand<ReleaseViewModel>(o =>
            {
                CurrentView = ReleaseVm;
            });
        }

    }
}
