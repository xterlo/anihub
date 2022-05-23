using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testWpf.MVVM.View;
using SocketIOClient;
using testWpf.Core;
using System.Threading;
using System.Collections.ObjectModel;

namespace testWpf.MVVM.ViewModel
{
   
    public class SocketRoomInfo
    {
        [JsonProperty("urlvideo")]
        public string urlvideo { get; set; }

        [JsonProperty("voiceid")]
        public int voiceid { get; set; }

        [JsonProperty("animeid")]
        public int animeid { get; set; }

        [JsonProperty("seria")]
        public int seria { get; set; }
    }

    public class SocketPleerPropertiesSet
    {
        public SocketPleerPropertiesSet(string clientSid,IntegratedPleer.Properties prop)
        {
            this.clientSid = clientSid;
            this.voicesId = prop.voicesId;
            this.episodes = prop.episodes;
            this.videoLength = prop.videoLength;
            this.winPos = prop.winPos;
            this.winTime = prop.winTime;
            this.isPause = prop.isPause;
            this.urlvideo = prop.urlvideo;
            this.urlroom = "pizdaKita";
            //this.urlroom = prop.urlroom;
            this.episode = prop.episode;
        }
        [JsonProperty("clientSid")]
        public string clientSid { get; set; }

        [JsonProperty("winPos")]
        public float winPos { get; set; }

        [JsonProperty("winTime")]
        public long winTime { get; set; }

        [JsonProperty("videoLength")]
        public long videoLength { get; set; }

        [JsonProperty("voicesId")]
        public string voicesId { get; set; }

        [JsonProperty("episode")]
        public int episode { get; set; }

        [JsonProperty("episodes")]
        public ObservableCollection<int> episodes { get; set; }

        [JsonProperty("urlvideo")]
        public string urlvideo { get; set; }

        [JsonProperty("urlroom")]
        public string urlroom { get; set; }

        [JsonProperty("isPause")]
        public bool isPause { get; set; }

    }
    public class SocketPleerPropertiesGet
    {

        [JsonProperty("winPos")]
        public float winPos { get; set; }

        [JsonProperty("winTime")]
        public long winTime { get; set; }

        [JsonProperty("videoLength")]
        public long videoLength { get; set; }

        [JsonProperty("voicesId")]
        public string voicesId { get; set; }

        [JsonProperty("episode")]
        public int episode { get; set; }

        [JsonProperty("episodes")]
        public ObservableCollection<int> episodes { get; set; }

        [JsonProperty("urlvideo")]
        public string urlvideo { get; set; }

        [JsonProperty("urlroom")]
        public string urlroom { get; set; }

        [JsonProperty("isPause")]
        public bool isPause { get; set; }

    }

    public class WatchingRoomModel : INotifyPropertyChanged
    {
        public static WatchingRoomModel Instance;
        SocketIO socket;
        Properties properties;
        SocketRoomInfo roomInfo;
        public event PropertyChangedEventHandler PropertyChanged;

        public class Properties : INotifyPropertyChanged
        {
            public Properties(SocketPleerPropertiesGet prop)
            {
                ObservableCollection<int> episodes_int = new ObservableCollection<int>();
                this.voicesId = prop.voicesId;
                this.episodes = prop.episodes;
                this.videoLength = prop.videoLength;
                this.winPos = prop.winPos;
                this.winTime = prop.winTime;
                this.isPause = prop.isPause;
                this.urlvideo = prop.urlvideo;
                this.urlroom = prop.urlroom;
                this.episode = prop.episode;
                this.lockPause = false;
            }

            public Properties(string urlVideo, string voiceId, int ep,ObservableCollection<string> episodes, bool isOwner)
            {
                ObservableCollection<int> episodes_int = new ObservableCollection<int>();
                this.voicesId = voiceId;
                foreach (var episode in episodes)
                    episodes_int.Add(int.Parse(episode));
                this.episodes = episodes_int;
                this.videoLength = 0;
                this.urlvideo = urlVideo;
                this.urlroom = __roomLink;
                this.isShared = true;
                this.isOwner = isOwner;
                this.episode = ep;
                this.lockPause=false;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private float _winPos;
            public float winPos
            {
                get { return _winPos; }
                set
                {
                    _winPos = value;
                    OnPropertyChanged(nameof(winPos));
                }
            }

            private long _winTime;
            public long winTime
            {
                get { return _winTime; }
                set
                {
                    _winTime = value;
                    OnPropertyChanged(nameof(winTime));

                }

            }

            private long _videoLength;
            public long videoLength
            {
                get { return _videoLength; }
                set
                {
                    _videoLength = value;
                    OnPropertyChanged(nameof(videoLength));

                }

            }

            private string _voicesId;
            public string voicesId
            {
                get { return _voicesId; }
                set
                {
                    _voicesId = value;

                }

            }

            private int _episode;
            public int episode
            {
                get { return _episode; }
                set
                {
                    _episode = value;
                    OnPropertyChanged(nameof(episode));
                }

            }

            private ObservableCollection<int> _episodes;
            public ObservableCollection<int> episodes
            {
                get { return _episodes; }
                set
                {
                    _episodes = value;
                }

            }

            private string _urlvideo;
            public string urlvideo
            {
                get { return _urlvideo; }
                set
                {
                    _urlvideo = value;
                }

            }

            private string _urlroom;
            public string urlroom
            {
                get { return _urlroom; }
                set
                {
                    _urlroom = value;
                }

            }

            private bool _isPause;
            public bool isPause
            {
                get { return _isPause; }
                set
                {
                    _isPause = value;
                    OnPropertyChanged(nameof(isPause));
                }

            }

            private bool _isShared;
            public bool isShared
            {
                get { return _isShared; }
                set
                {
                    _isShared = value;
                }

            }

            private bool _isOwner;
            public bool isOwner
            {
                get { return _isOwner; }
                set
                {
                    _isOwner = value;
                }

            }

            private bool _lockPause;
            public bool lockPause
            {
                get { return _lockPause; }
                set
                {
                    _lockPause = value;
                    OnPropertyChanged(nameof(lockPause));
                }

            }


            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        

        private IntegratedPleer _pleerWindow;
        public IntegratedPleer pleerWindow
        {
            get { return _pleerWindow; }
            set
            {
                _pleerWindow = value;
                OnPropertyChanged(nameof(pleerWindow));
            }
        }


        public static string __roomLink;
        private string _roomLink;
        public string roomLink
        {
            get { return _roomLink; }
            set
            {
                _roomLink = value;
                OnPropertyChanged(nameof(roomLink));
            }
        }

        public WatchingRoomModel(string link)
        {
            roomLink = link;
            __roomLink = link;
            attachEvents();
            SocketManager.CreateSocket(roomLink);
            Instance = this;
        }

        public WatchingRoomModel(Properties prop)
        {
            
            properties = prop;
            pleerWindow = new IntegratedPleer(properties);
            initSocketConnection();
            attachEvents();
            Instance = this;

        }

        private void attachEvents()
        {
            SocketManager.responseHandler += responseHandler;
        }

        private async void responseHandler(object sender, SocketManager.eventType et)
        {
            switch (et)
            {
                case SocketManager.eventType.joinedRoom:
                    SocketManager.GetRoomInfo(roomLink);
                    break;
                case SocketManager.eventType.acceptRoomInfo:
                    roomInfo = sender as SocketRoomInfo;
                    ResponseHandler response = new ResponseHandler();
                    await response.GetRelease(roomInfo.animeid.ToString()); 
                    if (properties is null)
                        InitPleer();
                    break;
                case SocketManager.eventType.newUserConnected:
                    SocketPleerPropertiesSet newProperties = new SocketPleerPropertiesSet(sender as string, IntegratedPleer.properties);
                    SocketManager.SendPleerProperties(newProperties);
                    break;
                case SocketManager.eventType.setNewPleerProperties:
                    IntegratedPleer.properties.updateValues(new Properties(sender as SocketPleerPropertiesGet));
                    break;
                case SocketManager.eventType.setPause:
                    IntegratedPleer.properties.setPause(sender as SocketPauseInformation);
                    break;
                case SocketManager.eventType.setReady:
                    IntegratedPleer.properties.LockPause(true);
                    IntegratedPleer.properties.updatePosition(Convert.ToSingle(sender));
                    SocketManager.ChangeReadyState();
                    //IntegratedPleer.properties.setPause(sender as SocketPauseInformation);
                    break;
                case SocketManager.eventType.changeVideoTime:
                    IntegratedPleer.properties.LockPause(false);
                    //IntegratedPleer.properties.setPause(sender as SocketPauseInformation);
                    break;
                case SocketManager.eventType.changeEpisode:
                    IntegratedPleer.properties.updateEpisode(Convert.ToInt32(sender));
                    break;

            }
        }

        private void InitPleer()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                properties = new Properties(roomInfo.urlvideo, roomInfo.voiceid.ToString(), roomInfo.seria, new ObservableCollection<string>() { "1", "2", "3" }, false);
                pleerWindow = new IntegratedPleer(properties);
                
                //pleerWindow.StartVideo();
            });       
        }



        private async void initSocketConnection()
        {
            ResponseHandler resp = new ResponseHandler();
            roomLink = await resp.CreateRoom(new RoomInfo(IntegratedPleer.properties));
            IntegratedPleer.properties.updateUrlRoom(roomLink);
            SocketManager.CreateSocket(roomLink);
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
