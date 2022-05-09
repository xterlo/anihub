using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using testWpf.Core;
using testWpf.Dialogs.Service;

namespace testWpf.Dialogs.MediaViewer
{

    public class MediaViewerModel : DialogViewModelBase<DialogResults>, INotifyPropertyChanged
    {
        private string _message;
        public string message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(message));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand MouseClick
        {
            get
            {
                return new RelayCommand<IDialogWindow>(pickCommand);
            }
        }

        public ICommand ScrollImage
        {
            get
            {
                return new RelayCommand<string>(ScrollImageFunc);
            }
        }

        public void ScrollImageFunc(string direction)
        {
            if (direction == "Right")
                positionImg++;
            else
                positionImg--;
            if(positionImg < 0) positionImg = imgSource.Count-1;
            else if (positionImg > imgSource.Count-1) positionImg = 0;
            Console.WriteLine($"TEST POS {positionImg}");
            currentImage = imgSource[positionImg];
        }

        public void pickCommand(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.Yes);
        }

        private int _positionImg;
        public int positionImg
        {
            get { return _positionImg; }
            set
            {
                _positionImg = value;
                OnPropertyChanged(nameof(positionImg));
            }
        }

        private List<string> _imgSource;
        public List<string> imgSource
        {
            get { return _imgSource; }
            set
            {
                _imgSource = value;
                OnPropertyChanged(nameof(imgSource));
            }
        }

        private string _currentImage;
        public string currentImage
        {
            get { return $@"https://static.anixart.tv/screenshots/{_currentImage}.jpg"; }
            set { _currentImage = value;
                OnPropertyChanged(nameof(currentImage));
            }
        }


        public MediaViewerModel(int pos, List<string> images) : base()
        {

            _imgSource = TemplatePreferens.releaseInfo.GetScreenshots();
            positionImg = pos;
            message = "adssadsad";
            currentImage =  images[pos];
        }



        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
