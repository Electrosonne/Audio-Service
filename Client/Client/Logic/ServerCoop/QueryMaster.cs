using Newtonsoft.Json;
using RestSharp;
using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Client.Logic
{
    static class QueryMaster
    {
        private static string m_URL = "https://localhost:44383";

        public static bool Connection { get; set; } = false;

        public static string Fix(this string str)
        {
            return (string)JsonConvert.DeserializeObject(str);
        }

        public static bool SuccessConnect()
        {
            var client = new RestClient(m_URL + "/Hello");
            var request = new RestRequest("", Method.GET);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request).Content.Equals("\"Hello!\"");
        }

        public static Uri GetMusicUrl(int id)
        {
            return new Uri(m_URL + "/File?" + id);
        }

        public static Stream GetMusicFromServer(int id)
        {
            var request = WebRequest.Create(m_URL + "/File?" + id);
            request.Headers.Add("Id", id.ToString());
            return request.GetResponse().GetResponseStream();
        }

        public static Stream GetImageFromServer(int id)
        {
            var request = WebRequest.Create(m_URL + "/Image");
            request.Headers.Add("Id", id.ToString());
            return request.GetResponse().GetResponseStream();
        }

        public static Stream GetPlaylistImageFromServer(int id)
        {
            var request = WebRequest.Create(m_URL + "/PlaylistImage");
            request.Headers.Add("Id", id.ToString());
            return request.GetResponse().GetResponseStream();
        }

        public static List<Playlist> GetAllPlaylist()
        {
            if (!Connection)
                return null;

            var client = new RestClient(m_URL + "/GetAllPlaylist");
            var request = new RestRequest("", Method.GET);
            request.RequestFormat = DataFormat.Json;

            string result = client.Execute(request).Content;

            return JsonConvert.DeserializeObject<List<Playlist>>(result.Fix());
        }

        public static List<Forum> GetForum(int id)
        {
            if (!Connection)
                return null;

            var client = new RestClient(m_URL + "/Forum");
            var request = new RestRequest("", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Id", id.ToString());

            string result = client.Execute(request).Content;

            return JsonConvert.DeserializeObject<List<Forum>>(result.Fix());
        }

        public static List<Interpretation> GetInterpretation(int id)
        {
            if (!Connection)
                return null;

            var client = new RestClient(m_URL + "/Interpretation");
            var request = new RestRequest("", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Id", id.ToString());

            string result = client.Execute(request).Content;

            return JsonConvert.DeserializeObject<List<Interpretation>>(result.Fix());
        }

        public static string Authorization(string login, string password)
        {
            if (!Connection)
                return "No connection";

            var client = new RestClient(m_URL + "/Authorization");
            var request = new RestRequest("", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Login", login);
            request.AddHeader("Password", password);

            return client.Execute(request).Content;
        }

        public static string Registration(string login, string password)
        {
            if (!Connection)
                return "No connection";

            var client = new RestClient(m_URL + "/Registration");
            var request = new RestRequest("", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Login", login);
            request.AddHeader("Password", password);

            return client.Execute(request).Content;
        }

        public static bool AddCommentary(Forum forum)
        {
            if (!Connection)
                return false;

            var client = new RestClient(m_URL + "/NewCommentary");
            var request = new RestRequest("", Method.POST);
            request.AddParameter("Forum", JsonConvert.SerializeObject(forum), ParameterType.RequestBody);

            return client.Execute(request).IsSuccessful;
        }

        public static List<Music> GetMusicFromPlaylist(int id)
        {
            if (!Connection)
                return null;

            var client = new RestClient(m_URL + "/GetMusicFromPlaylist");
            var request = new RestRequest("", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Id", id.ToString());

            string result = client.Execute(request).Content;

            return JsonConvert.DeserializeObject<List<Music>>(result.Fix());
        }

        public static bool AddInterpretation(Interpretation inter)
        {
            if (!Connection)
                return false;

            var client = new RestClient(m_URL + "/AddInterpretation");
            var request = new RestRequest("", Method.POST);
            request.AddParameter("Forum", JsonConvert.SerializeObject(inter), ParameterType.RequestBody);

            return client.Execute(request).IsSuccessful;
        }

        public static void AddInPlaylist(List<int> playlist, int music)
        {
            if (!Connection)
                return;

            var client = new RestClient(m_URL + "/AddInPlaylist");
            var request = new RestRequest("", Method.POST);
            request.AddParameter("Playlist", JsonConvert.SerializeObject(playlist), ParameterType.RequestBody);
            request.AddHeader("Music", music.ToString());

            client.Execute(request);
        }

        public static void DeleteFromPlaylist(List<int> playlist, int music)
        {
            if (!Connection)
                return;

            var client = new RestClient(m_URL + "/DeleteFromPlaylists");
            var request = new RestRequest("", Method.POST);
            request.AddParameter("Playlist", JsonConvert.SerializeObject(playlist), ParameterType.RequestBody);
            request.AddHeader("Music", music.ToString());

            client.Execute(request);
        }

        public static bool AddPlaylist(SharedData.Playlist playlist)
        {
            if (!Connection)
                return false;

            var client = new RestClient(m_URL + "/NewPlaylist");
            var request = new RestRequest("", Method.POST);
            request.AddParameter("Playlist", JsonConvert.SerializeObject(playlist), ParameterType.RequestBody);

            return client.Execute(request).IsSuccessful;
        }

        public static bool UploadMusic(SharedData.Music music)
        {
            if (!Connection)
                return false;

            var client = new RestClient(m_URL + "/UploadMusic");
            var request = new RestRequest("", Method.POST);
            request.AddParameter("Playlist", JsonConvert.SerializeObject(music), ParameterType.RequestBody);

            return client.Execute(request).IsSuccessful;
        }

        public static List<Music> FindMusic(string str, int start, int end)
        {
            if (!Connection)
                return null;

            var client = new RestClient(m_URL + "/FindMusic");
            var request = new RestRequest("", Method.POST);
            request.AddParameter("Playlist", str, ParameterType.RequestBody);
            request.AddHeader("Start", start.ToString());
            request.AddHeader("End", end.ToString());

            var result = client.Execute(request).Content;

            return JsonConvert.DeserializeObject<List<Music>>(result.Fix());
        }

        public static List<int> GetMusicPlaylists(int musicId)
        {
            if (!Connection)
                return null;

            var client = new RestClient(m_URL + "/MusicPlaylists");
            var request = new RestRequest("", Method.GET);
            request.AddHeader("Id", musicId.ToString());
            request.RequestFormat = DataFormat.Json;

            string result = client.Execute(request).Content;

            return JsonConvert.DeserializeObject<List<int>>(result.Fix());
        }
    }
}
