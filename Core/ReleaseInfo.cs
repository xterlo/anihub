using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using testWpf.MVVM.View;

namespace testWpf.Core
{

    #region SOCKETS
    public class RoomInfo
    {
        public string nameroom;
        public string owner;

        [JsonProperty("animeid")]
        public int animeid { get; set; }

        [JsonProperty("voiceid")]
        public string voiceid { get; set; }

        [JsonProperty("seria")]
        public int seria { get; set; }

        [JsonProperty("urlvideo")]
        public string urlvideo { get; set; }
        public RoomInfo(int aId, string vId, int s,string url)
        {
            animeid = aId;
            voiceid = vId;
            seria = s;
            nameroom = "dygquwdqkdq";
            owner = "pizdatiyChel";
            urlvideo = url;
        }
        public RoomInfo(IntegratedPleer.Properties prop)
        {
            animeid = TemplatePreferens.releaseInfo.GetId();
            voiceid = prop.voicesId;
            seria = prop.episode;
            nameroom = "dygquwdqkdq";
            owner = "token123";
            urlvideo = prop.urlvideo;
        }

    }
    public class RoomUrl
    {
        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }
    }

    public class SocketPauseInformation
    {
        [JsonProperty("pauseState")]
        public bool pauseState { get; set; }

        [JsonProperty("positionState")]
        public float positionState { get; set; }

        [JsonProperty("urlroom")]
        public string urlroom { get; set; }
    }



    #endregion

    #region SHIKIMORI

    public class animesPopularity
    {
        
        public List<anime> animes { get; set; }
        
        public class anime
        {
            [JsonProperty("url")]
            public string url { get; set;}

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("russian")]
            public string russian { get; set; }

            [JsonProperty("image")]
            public images image { get; set; }


        }

        public class images
        {
            [JsonProperty("original")]
            public string original { get; set; }

            [JsonProperty("preview")]
            public string preview { get; set; }

            [JsonProperty("x96")]
            public string x96 { get; set; }

            [JsonProperty("x48")]
            public string x48 { get; set; }
        }

    }

    public class ShikiAnimeInformation
    {
        public List<AnimeInfo> anime { get; set; }

        public class AnimeInfo
        {
            [JsonProperty("id")]
            public int id { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("russian")]
            public string russian { get; set; }

            [JsonProperty("url")]
            public string url { get; set; }

            [JsonProperty("kind")]
            public string kind { get; set; }

            [JsonProperty("score")]
            public string score { get; set; }

            [JsonProperty("status")]
            public string status { get; set; }

            [JsonProperty("episodes")]
            public string episodes { get; set; }

            [JsonProperty("episodes_aired")]
            public string episodes_aired { get; set; }

            [JsonProperty("aired_on")]
            public string aired_on { get; set; }

            [JsonProperty("released_on")]
            public string released_on { get; set; }

            [JsonProperty("image")]
            public Images image { get; set; }



            public class Images
            {
                [JsonProperty("original")]
                public string original { get; set; }

                [JsonProperty("preview")]
                public string preview { get; set; }

                [JsonProperty("x96")]
                public string x96 { get; set; }

                [JsonProperty("x48")]
                public string x48 { get; set; }
            }
        }
    }

    #endregion

    public class Query
    {
        public string query;
        public int searchBy;
        public Query(string q)
        {
            query = q;
            searchBy = 0;
        }
    }

    public class ReleaseInfo
    {

        [JsonProperty("release")]
        public ReleaseData result { get; set; }

        public int GetId() {
            return result.id;
        }
        public void SetId(int param)
        {
            result.id = param;
        }
        public string GetPoster()
        {
            return $@"https://static.anixart.tv/posters/{result.poster}.jpg";
        }
        public void SetPoster(string param)
        {
            result.poster = param;
        }
        public string GetYear()
        {
            return result.year;
        }
        public void SetYear(string param)
        {
            result.year = param;
        }
        public string GetGenres()
        {
            return result.genres;
        }
        public void SetGenres(string param)
        {
            result.genres = param;
        }
        public string GetCountry()
        {
            return result.country;
        }
        public void SetCountry(string param)
        {
            result.country = param;
        }
        public string GetDirector()
        {
            return result.director;
        }
        public void SetDirector(string param)
        {
            result.director = param;
        }
        public string GetAuthor()
        {
            return result.author;
        }
        public void SetAuthor(string param)
        {
            result.author = param;
        }
        public string GetTranslators()
        {
            return result.translators;
        }
        public void SetTranslators(string param)
        {
            result.translators = param;
        }
        public string GetStudio()
        {
            return result.studio;
        }
        public void SetStudio(string param)
        {
            result.studio = param;
        }
        public string GetDescription()
        {
            return result.description;
        }
        public void SetDescription(string param)
        {
            result.description = param;
        }
        public string GetTitleRu()
        {
            return result.title_ru;
        }
        public void SetTitleRu(string param)
        {
            result.title_ru = param;
        }
        public string GetTitleOriginal()
        {
            return result.title_original;
        }
        public void SetTitleOriginal(string param)
        {
            result.title_original = param;
        }

        public string GetRelated()
        {
            return result.related.id;
        }

        public List<string> GetScreenshots()
        {
            return result.screenshots;
        }

        public Dictionary<int,string> GetGrades()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>
            {
                {0,result.grade},
                {1,result.vote_1_count},
                {2,result.vote_2_count},
                {3,result.vote_3_count},
                {4,result.vote_4_count},
                {5,result.vote_5_count},
                {6,result.vote_count}
            };
            return dict;
        }

        public class ReleaseData
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("poster")]
            public string poster { get; set; }
            [JsonProperty("year")]
            public string year { get; set; }
            [JsonProperty("genres")]
            public string genres { get; set; }
            [JsonProperty("country")]
            public string country { get; set; }
            [JsonProperty("director")]
            public string director { get; set; }
            [JsonProperty("author")]
            public string author { get; set; }
            [JsonProperty("translators")]
            public string translators { get; set; }
            [JsonProperty("studio")]
            public string studio { get; set; }
            [JsonProperty("description")]
            public string description { get; set; }

            [JsonProperty("title_ru")]
            public string title_ru { get; set; }
            [JsonProperty("title_original")]
            public string title_original { get; set; }

            [JsonProperty("related")]
            public Related related { get; set; }

            [JsonProperty("screenshots")]
            public List<string> screenshots { get; set; }

            [JsonProperty("grade")]
            public string grade { get; set; }
            [JsonProperty("vote_1_count")]
            public string vote_1_count { get; set; }

            [JsonProperty("vote_2_count")]
            public string vote_2_count { get; set; }

            [JsonProperty("vote_3_count")]
            public string vote_3_count { get; set; }

            [JsonProperty("vote_4_count")]
            public string vote_4_count { get; set; }

            [JsonProperty("vote_5_count")]
            public string vote_5_count { get; set; }

            [JsonProperty("vote_count")]
            public string vote_count { get; set; }
        }
        public class Related
        {
            [JsonProperty("id")]
            public string id { get; set; }

        }
    }

    public class RelatedReleases
    {
        [JsonProperty("content")]
        public List<RelatedContent> related_content;


        public class RelatedContent 
        {
            [JsonProperty("id")]
            public string id;

            [JsonProperty("poster")]
            public string poster;

            [JsonProperty("title_ru")]
            public string title_ru;

            [JsonProperty("year")]
            public string year;
        }
    }

    public class ResponseSearch
    {
        [JsonProperty("content")]
        private List<Response> _contentt { get; set; }
        public List<Response> contentt
        {
            get { return _contentt; }
            set { _contentt = value; }
        }

        public class Response
        {
            [JsonProperty("title_ru")]
            public string title_ru { get; set; }

            [JsonProperty("poster")]
            public string poster { get; set; }

            [JsonProperty("description")]
            public string description { get; set; }

            [JsonProperty("id")]
            public string id { get; set; }

        }
    }

    public class Voices
    {
        [JsonProperty("types")]
        private List<Voice> _result { get; set; }
        public List<Voice> result
        {
            get { return _result; }
            set { _result = value; }
        }

        public class Voice
        {
            [JsonProperty("id")]
            public string id { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("workers")]
            public string workers { get; set; }

            [JsonProperty("episodes_count")]
            public int episodes_count { get; set; }

            [JsonProperty("view_count")]
            public string view_count { get; set; }


        }
    }

    public class VoicesId
    {
        [JsonProperty("sources")]
        private List<VoiceId> _vID { get; set; }
        public List<VoiceId> vID
        {
            get { return _vID; }
            set { _vID = value; }
        }

        public class VoiceId
        {
            [JsonProperty("id")]
            public string id { get; set; }

        }
    }

    public class GetKodikUrl
    {
        [JsonProperty("episode")]
        private Response _eepisode { get; set; }
        public Response eepisode
        {
            get { return _eepisode; }
            set { _eepisode = value; }
        }

        public class Response
        {
            [JsonProperty("url")]
            public string url { get; set; }

        }
    }

    public class GetVideoUrl
    {
        [JsonProperty("links")]
        public Quality links { get; set; }

        public class Quality
        {
            [JsonProperty("360")]
            public Link bad { get; set; }

            [JsonProperty("480")]
            public Link medium { get; set; }

            [JsonProperty("720")]
            public Link good { get; set; }

        }

        public class Link
        {
            [JsonProperty("Src")]
            public string Src { get; set; }
        }
    }

    public class Series
    {
        [JsonProperty("episodes")]
        public List<Positions> episodes { get; set; }

        public class Positions
        {
            [JsonProperty("position")]
            public int position { get; set; }
        }
    }
}
