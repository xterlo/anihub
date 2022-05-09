using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testWpf.Core;
using testWpf.Dialogs.MediaViewer;
using testWpf.Dialogs.Service;

namespace testWpf.MVVM.ViewModel
{
    public class ScreenshotsViewModel : INotifyPropertyChanged
    {
        private IDialogService _dialogService;

        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand _mouseClick;
        public ICommand MouseClick
        {
            get
            {
                return _mouseClick ?? (_mouseClick = new RelayCommand(pickScreenshot));
            }
        }

        private int _position;
        public int position
        {
            get { return _position; }
            set { _position = value; }
        }

        private string _imageSourceLink;
        public string imageSourceLink
        {
            get { return $@"https://static.anixart.tv/screenshots/{_imageSourceLink}.jpg"; }
            set
            {
                _imageSourceLink = value;
                OnPropertyChanged(nameof(imageSourceLink));
            }

        }
        public void pickScreenshot()
        {
            var dialog = new MediaViewerModel(position, TemplatePreferens.releaseInfo.GetScreenshots());
            var result = _dialogService.OpenDialog(dialog);
            Console.WriteLine(result);
        }

        public ScreenshotsViewModel(int pos)
        {
            _dialogService = new DialogService();
            position = pos;
            imageSourceLink = TemplatePreferens.releaseInfo.GetScreenshots()[position];
        }


        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
