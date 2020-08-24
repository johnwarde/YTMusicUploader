﻿using JBToolkit.StreamHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using YTMusicUploader.Business;
using YTMusicUploader.Helpers.FuzzyLogic;
using YTMusicUploader.Providers.RequestModels;

namespace YTMusicUploader.Providers
{
    /// <summary>
    /// HttpWebRequest POST request to send to YouTube to determine if the song about to be uploaded already exists
    /// It may request a little attention depending on the results, seen as though we're trying to match a song based
    /// on artist, album and track name, which may be slightly different in the uploading music file's meta tags. Currently
    /// it uses a Levenshtein distance fuzzy logic implementation to check similarity between strings and is considered
    /// a match if it's above 0.75.
    /// 
    /// Thanks to: sigma67: 
    ///     https://ytmusicapi.readthedocs.io/en/latest/ 
    ///     https://github.com/sigma67/ytmusicapi
    /// </summary>
    public partial class Requests
    {
        /// <summary>
        /// HttpWebRequest POST request to send to YouTube to determine if the song about to be uploaded already exists
        /// It may request a little attention depending on the results, seen as though we're trying to match a song based
        /// on artist, album and track name, which may be slightly different in the uploading music file's meta tags. Currently
        /// it uses a Levenshtein distance fuzzy logic implementation to check similarity between strings and is considered
        /// a match if it's above 0.75.
        /// 
        /// Thanks to: sigma67: 
        ///     https://ytmusicapi.readthedocs.io/en/latest/ 
        ///     https://github.com/sigma67/ytmusicapi
        /// </summary>
        /// <param name="musicFilePath">Path to music file to be uploaded</param>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <returns>True if song is found, false otherwise</returns>
        public static bool IsSongUploaded(string musicFilePath, string cookieValue, MusicDataFetcher musicDataFetcher = null)
        {
            if (musicDataFetcher == null)
                musicDataFetcher = new MusicDataFetcher();

            var musicFileMetaData = musicDataFetcher.GetMusicFileMetaData(musicFilePath);
            if (musicFileMetaData == null)
                return false;

            string artist = musicFileMetaData.Artist;
            string album = musicFileMetaData.Album;
            string track = musicFileMetaData.Track;

            try
            {
                bool result = IsSongUploaded(artist, album, track, cookieValue);

                if (result)
                    return result;

                album = album.Substring(album.IndexOf("-") + 1, album.Length - 1 - album.IndexOf("-")).Trim();
                result = IsSongUploaded(artist, album, track, cookieValue);

                if (result)
                    return result;

                album = Regex.Replace(album, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
                result = IsSongUploaded(artist, album, track, cookieValue);

                return result;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// HttpWebRequest POST request to send to YouTube to determine if the song about to be uploaded already exists
        /// It may request a little attention depending on the results, seen as though we're trying to match a song based
        /// on artist, album and track name, which may be slightly different in the uploading music file's meta tags. Currently
        /// it uses a Levenshtein distance fuzzy logic implementation to check similarity between strings and is considered
        /// a match if it's above 0.75.
        /// 
        /// Thanks to: sigma67: 
        ///     https://ytmusicapi.readthedocs.io/en/latest/ 
        ///     https://github.com/sigma67/ytmusicapi
        /// </summary>
        /// <param name="artist">Artist name from music file meta tag</param>
        /// <param name="album">Album name from music file meta tag</param>
        /// <param name="track">Track or song name from music file meta tag</param>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <returns>True if song is found, false otherwise</returns>
        public static bool IsSongUploadedMulitpleAlbumNameVariations(string artist, string album, string track, string cookieValue)
        {
            bool result = IsSongUploaded(artist, album, track, cookieValue);

            if (result)
                return result;

            album = album.Substring(album.IndexOf("-") + 1, album.Length - 1 - album.IndexOf("-")).Trim();
            result = IsSongUploaded(artist, album, track, cookieValue);

            if (result)
                return result;

            album = Regex.Replace(album, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
            result = IsSongUploaded(artist, album, track, cookieValue);

            return result;
        }


        /// <summary>
        /// HttpWebRequest POST request to send to YouTube to determine if the song about to be uploaded already exists
        /// It may request a little attention depending on the results, seen as though we're trying to match a song based
        /// on artist, album and track name, which may be slightly different in the uploading music file's meta tags. Currently
        /// it uses a Levenshtein distance fuzzy logic implementation to check similarity between strings and is considered
        /// a match if it's above 0.75.
        /// 
        /// Thanks to: sigma67: 
        ///     https://ytmusicapi.readthedocs.io/en/latest/ 
        ///     https://github.com/sigma67/ytmusicapi
        /// </summary>
        /// <param name="artist">Artist name from music file meta tag</param>
        /// <param name="album">Album name from music file meta tag</param>
        /// <param name="track">Track or song name from music file meta tag</param>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <returns>True if song is found, false otherwise</returns>
        public static bool IsSongUploaded(string artist, string album, string track, string cookieValue)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(YouTubeBaseUrl + "search" + Params);
                request = AddStandardHeaders(request, cookieValue);

                request.ContentType = "application/json; charset=UTF-8";
                request.Headers["X-Goog-AuthUser"] = "0";
                request.Headers["x-origin"] = "https://music.youtube.com";
                request.Headers["X-Goog-Visitor-Id"] = "CgtvVTcxa1EtbV9hayiMu-P0BQ%3D%3D";
                request.Headers["Authorization"] = GetAuthorisation(GetSAPISIDFromCookie(cookieValue));

                var context = JsonConvert.DeserializeObject<SearchContext.Root>(
                                                SafeFileStream.ReadAllText(
                                                        Path.Combine(Global.WorkingDirectory, @"AppData\search_uploads_context.json")));

                context.query = string.Format("{0} {1} {2}", artist, album, track);

                byte[] postBytes = GetPostBytes(JsonConvert.SerializeObject(context));
                request.ContentLength = postBytes.Length;

                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();
                }

                postBytes = null;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    string result;
                    using (var brotli = new Brotli.BrotliStream(
                                                        response.GetResponseStream(),
                                                        System.IO.Compression.CompressionMode.Decompress,
                                                        true))
                    {
                        var streamReader = new StreamReader(brotli);
                        result = streamReader.ReadToEnd();
                    }

                    JObject jo = JObject.Parse(result);
                    List<JToken> runs = jo.Descendants()
                                    .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "runs")
                                    .Select(p => ((JProperty)p).Value).ToList();

                    float artistSimilarity = 0.0f;
                    float albumSimilartity = 0.0f;
                    float trackSimilarity = 0.0f;

                    foreach (JToken run in runs)
                    {
                        if (run.ToString().Contains("text"))
                        {
                            var runArray = run.ToObject<SearchResult.Run[]>();
                            if (runArray.Length > 0)
                            {
                                if (runArray[0].text.ToLower().Contains("no results found"))
                                    return false;
                                else
                                {
                                    foreach (var runElement in runArray)
                                    {
                                        float _artistSimilarity = Levenshtein.Similarity(runElement.text, artist);
                                        if (_artistSimilarity > 0.75)
                                        {
                                            if (artistSimilarity < _artistSimilarity)
                                                artistSimilarity = _artistSimilarity;
                                        }

                                        float _albumSimilartity = Levenshtein.Similarity(runElement.text, album);
                                        if (_albumSimilartity > 0.75)
                                        {
                                            if (albumSimilartity < _albumSimilartity)
                                                albumSimilartity = _albumSimilartity;
                                        }

                                        float _trackSimilarity = Levenshtein.Similarity(runElement.text, track);
                                        if (_trackSimilarity > 0.75)
                                        {
                                            if (trackSimilarity < _trackSimilarity)
                                                trackSimilarity = _trackSimilarity;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (artistSimilarity > 0.8 &&
                        albumSimilartity > 0.8 &&
                        trackSimilarity > 0.8)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}