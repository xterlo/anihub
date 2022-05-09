using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testWpf.MVVM.View;
using testWpf.MVVM.ViewModel;

namespace testWpf.Core
{

    public static class SocketManager
    {
        public static event SocketResponseHandler responseHandler;
        public delegate void SocketResponseHandler(object o, eventType et);

        private static SocketIO socket;

        public enum eventType
        {
            joinedRoom,
            acceptRoomInfo,
            newUserConnected,
            setNewPleerProperties,
            setPause
        }
        public static void CreateSocket(string roomUrl)
        {
            socket = new SocketIO(ResponseHandler.serverUrl);
            socket.ConnectAsync();
            socket.On("hi", response =>
            {
                Console.WriteLine(response);
                Console.WriteLine(1);
                //string text = response.GetValue<DataCon>().data;
                //Console.WriteLine(text);
            });
            socket.On("joined", response =>
            {
                if (responseHandler != null)
                    responseHandler(response, eventType.joinedRoom);
            });
            socket.On("leaved", response =>
            {
                Console.WriteLine(response);
                string text = response.GetValue<string>();
            });
            socket.On("setPauseState", response =>
            {
                if (responseHandler != null)
                    responseHandler(response.GetValue<SocketPauseInformation>(), eventType.setPause);
            });
            socket.On("newUserConnected", response =>
            {
                if (responseHandler != null)
                    responseHandler(response.GetValue<string>(), eventType.newUserConnected);
            });
            socket.On("setNewPleerProperties", response =>
            {
                if (responseHandler != null)
                    responseHandler(response.GetValue<SocketPleerPropertiesGet>(), eventType.setNewPleerProperties);
            });
            socket.On("acceptRoomInfo", response =>
            {
                if(responseHandler != null)
                    responseHandler(response.GetValue<SocketRoomInfo>(), eventType.acceptRoomInfo);
            });
            socket.On("text", response =>
            {
                Console.WriteLine(response);
                //textSocket = response.GetValue<string>();
            });
            socket.OnConnected += async (senderr, ee) =>
            {
                await socket.EmitAsync("join", new RoomUrl
                {
                    url = roomUrl,
                    token = "token123"
                });
            };
        }

        public async static void GetRoomInfo(string roomUrl)
        {

            await socket.EmitAsync("requestRoomInfo",
                new RoomUrl
                {
                    url = roomUrl
                });
        }

        public async static void SetPause(IntegratedPleer.Properties prop)
        {

            await socket.EmitAsync("setPause", new SocketPauseInformation
            {
                pauseState = prop.isPause,
                positionState = prop.winPos,
                urlroom = prop.urlroom
            }) ;
        }

        public async static void GetPleerProperties(string urlroom)
        {

            await socket.EmitAsync("getProperties",urlroom);
        }

        public async static void SendPleerProperties(SocketPleerPropertiesSet prop)
        {

            await socket.EmitAsync("sendProperties", prop);
        }

    }
}
