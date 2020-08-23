﻿using JBToolkit.StreamHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
namespace YTMusicUploader.Providers
{
    /// <summary>
    /// HttpWebRequest POST request to send to YouTube to check if the user's is authenticated (signed in) by determining 
    /// if a generic request is successful given the current authentication cookie value we have stored.
    /// 
    /// In this case, we're actually perform a request for personally uploaded music files
    /// 
    /// Thanks to: sigma67: 
    ///     https://ytmusicapi.readthedocs.io/en/latest/ 
    ///     https://github.com/sigma67/ytmusicapi
    /// </summary>
    public partial class Requests
    {
        /// <summary>
        /// HttpWebRequest POST request to send to YouTube to check if the user's is authenticated (signed in) by determining 
        /// if a generic request is successful given the current authentication cookie value we have stored.
        /// 
        /// In this case, we're actually perform a request for personally uploaded music files
        /// 
        /// Thanks to: sigma67: 
        ///     https://ytmusicapi.readthedocs.io/en/latest/ 
        ///     https://github.com/sigma67/ytmusicapi
        /// </summary>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <returns>True if successfully authenticated, false otherwise</returns>
        public static bool IsAuthenticated(string cookieValue)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(YouTubeBaseUrl + "browse" + Params);
                request = AddStandardHeaders(request, cookieValue);

                request.ContentType = "application/json; charset=UTF-8";
                request.Headers["X-Goog-AuthUser"] = "0";
                request.Headers["x-origin"] = "https://music.youtube.com";
                request.Headers["X-Goog-Visitor-Id"] = "CgtvVTcxa1EtbV9hayiMu-P0BQ%3D%3D";
                request.Headers["Authorization"] = GetAuthorisation(GetSAPISIDFromCookie(cookieValue));

                byte[] postBytes = GetPostBytes(
                                        SafeFileStream.ReadAllText(
                                                Path.Combine(Global.WorkingDirectory, @"AppData\context.json")));
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

                    var json = JsonConvert.DeserializeObject(result);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}