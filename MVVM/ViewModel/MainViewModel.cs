using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using testWpf.Core;

namespace testWpf.MVVM.ViewModel
{
    internal class MainViewModel : ObserverObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand ReleaseViewCommand { get; set; }
        public HomeViewModel HomeVm { get; set; }
        public DiscoveryViewModel DiscoveryVm { get; set; }
        public ReleaseViewModel ReleaseVm { get; set; }
        public WatchingRoomModel WatchVm { get; set; }

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
        public MainViewModel()
        {
            Instance = this;
            searchViewModel = new ObservableCollection<SearchViewModel>();
            HomeVm = new HomeViewModel();
            DiscoveryVm = new DiscoveryViewModel();
            //ReleaseVm = new ReleaseViewModel();
            CurrentView = HomeVm;

           // HomeViewCommand = new RelayCommand<MainViewModel>(o =>
           // {
           //     CurrentView = HomeVm;
           // });

           // DiscoveryViewCommand = new RelayCommand(o =>
           //{
           //    CurrentView = DiscoveryVm;
           //});
            //ReleaseViewCommand = new RelayCommand(o =>
            //{
            //    CurrentView = ReleaseVm;
            //});
        }

    }
}
