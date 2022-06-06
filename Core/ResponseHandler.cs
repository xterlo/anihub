using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace testWpf.Core
{
    public class ResponseHandler
    {
        public event ReadyGetResponse endGetRelease;
        public delegate void ReadyGetResponse();


        //https://api.anixart.tv/release/18265?extended_mode=true&token=d49a1e1b743a044bdefa82164e67e404532750ca
        public string getReleaseUrl = "https://api.anixart.tv/release/";
        public string getRelatedUrl = "https://api.anixart.tv/related/";
        public string getEpisodesUrl = "https://api.anixart.tv/episode/";
        public static string serverUrl = "http://176.193.215.228:4321/";
        public string releaseID;
        public ResponseHandler()
        {
            this.releaseID = null;
        }
        public ResponseHandler(string releaseID)
        {
            this.releaseID = releaseID;
        }

        private async Task<string> GetResponseAsync(string url,bool needSign = false)
        {
            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (needSign)
            {
                request.Method = "GET";
                request.Headers.Add("Sign", "TmdmSFowb6paVTVqTTJrekpqUXdNRHcxUWpVbU4EQXhXVGhHT3hJMWJWNHZOalo5TDFKWE7xbDJNVGxFUTExRGIyb6hOek46OCsyJjA7NDBiNWYyZWc6MDQ8ZzRkYjc5MmczOGI3MTM5ZWdnZXFvLnV8a5h5dXFodi8jcGt9Y6R5Zm48dE8JeWhv7vqrSYyC");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.UserAgent = "AnixartApp / 7.9.2 - 21121202(Android 7.1.2; SDK 25; x86; samsung SM-G965N; ru";
                request.Host = "api.anixart.tv";
            }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                return "ServerError";
            }
        }

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://176.193.215.228:4321/"))
                {
                    return true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                    return true;
                return false;
            }
        }

        public async Task<List<ShikiAnimeInformation.AnimeInfo>> getHotAnimes(int limit)
        {
            string url = $"https://shikimori.one/api/animes?order=popularity&season=2022&limit={limit.ToString()}&status=ongoing";
            string response = await GetResponseAsync(url);

            return JsonConvert.DeserializeObject<List<ShikiAnimeInformation.AnimeInfo>>(response);
        }
        

        public async Task<string> CheckRoomExist(string roomid)
        {
            string url = $"{serverUrl}api/room?roomid={roomid}";
            string response = await GetResponseAsync(url);

            return response;
        }

        public async Task GetRelease(bool extMode = true,string token="")
        {
           
            string url = $"{getReleaseUrl}{this.releaseID}?extended_mode={extMode}&token={token}";
            string html = await GetResponseAsync(url);

            TemplatePreferens.releaseInfo = JsonConvert.DeserializeObject<ReleaseInfo>(html);
            await GetRelated(TemplatePreferens.releaseInfo.GetRelated());
            endGetRelease();

        }

        public async Task GetRelated(string relatedID, bool extMode = true, string token = "")
        {

            string url = $"{getRelatedUrl}{relatedID}/0";
            string html = await GetResponseAsync(url,true);
            TemplatePreferens.relatedReleases = JsonConvert.DeserializeObject<RelatedReleases>(html);
        }


        public async Task<Voices> GetVoice(string _url, bool extMode = true, string token = "")
        {

            string url = $"{_url}{this.releaseID}?extended_mode={extMode}&token={token}";
            string html = await GetResponseAsync(url);
            return JsonConvert.DeserializeObject<Voices>(html); ;
           
        }

        public async Task<VoicesId> GetVoiceID(string _url, string voice)
        {

            string url = $"{_url}{this.releaseID}/{voice}";
            string html = await GetResponseAsync(url);
            return JsonConvert.DeserializeObject<VoicesId>(html);

        }

        public async Task<string> GetSeriaLink(string _url, string voiceID, string seria)
        {


            string url = $"{_url}target/{this.releaseID}/{voiceID}/{seria}";
            string html = await GetResponseAsync(url, true);
            GetKodikUrl kodikUrl = JsonConvert.DeserializeObject<GetKodikUrl>(html);
            string formattedText = "http://kodik.biz/api/video-links?p=56a768d08f43091901c44b54fe970049&link=" + kodikUrl.eepisode.url.Replace('?', '&').Remove(0, 6);
            return formattedText;
        }

        public async Task<string> GetVideoUrl(string _url, bool extMode = true, string token = "")
        {

            string html = await GetResponseAsync(_url);
            GetVideoUrl videoUrl = JsonConvert.DeserializeObject<GetVideoUrl>(html);
            string formattedtext = videoUrl.links.good.Src.Replace(":hls:manifest.m3u8", "");
            if (!formattedtext.Contains("https")) formattedtext = "https:" + videoUrl.links.good.Src.Replace(":hls:manifest.m3u8", "");
            return formattedtext;
        }

        public async Task<Series> GetSeries(string _url, string voice, string voiceID)
        {

            string url = $"{_url}{this.releaseID}/{voice}/{voiceID}";
            string html = await GetResponseAsync(url);
            return JsonConvert.DeserializeObject<Series>(html);

        }


        public async Task<ResponseSearch> postRequestAsync(Query q)
        {

            
            string result;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.anixart.tv/search/releases/0?token=");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(q);
                await streamWriter.WriteAsync(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = await streamReader.ReadToEndAsync();
            }
            ResponseSearch resp = JsonConvert.DeserializeObject<ResponseSearch>(result);
            return resp;

        }

        public async Task<string> CreateRoom(RoomInfo q)
        {
            string result;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{serverUrl}api/createroom");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(q);
                await streamWriter.WriteAsync(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = await streamReader.ReadToEndAsync();
            }
              
            return JsonConvert.DeserializeObject<RoomUrl>(result).url;
        }


    }
}
