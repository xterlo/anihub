using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testWpf.Core;

namespace testWpf.MVVM.ViewModel
{
    internal class HomeViewModel : ReleaseInfo, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private  ReleaseInfo _releaseInfo = TemplatePreferens.releaseInfo;
        private string _title;

        public string title
        {
            get { return _title; }
            set {
                _title = value;
                OnPropertyChanged(nameof(title));    
            }
        }

        public ReleaseInfo releaseInfo
        {
            get { return _releaseInfo; }
            set { _releaseInfo = value; } 
        }

        public HomeViewModel()
        {
            
        }

        public void changeTitle()
        {
            
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
