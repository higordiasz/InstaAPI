﻿using System;
using System.Net;
using System.Net.Http;
using InstaAPI.Cookies;
using InstaAPI.Header;
using InstaAPI.Helpers.Date;
using InstaAPI.Helpers.UA;
using InstaAPI.Helpers.User;

namespace InstaAPI
{
    public class Instagram
    {
        //Private Variables
        private CookieHelper _cookie = new();
        private HttpClient _client;
        private HttpClientHandler _handler;
        private UserAgent _ua;
        private WebProxy _proxy;

        //Internal Variables
        internal CookieHelper Cookie { get { return _cookie; } }
        internal HttpClientHandler Handler { get { return _handler; } }
        internal HttpClient Client { get { return _client; } }
        internal UserAgent Ua { get { return _ua; } }
        internal dynamic _sharedData { get; set; }
        internal string Challenge_URL { get; set; }
        internal WebProxy Proxy { get { return _proxy; } }
        internal long UserID { get; set; }
        internal UserData User { get; set; }

        //Constructors

        public Instagram (string username, string password)
        {
            User = new(username.ToLower(), GetEncryptedPassword(password));
            Challenge_URL = "challenge/";
            _cookie = new();
            _handler = new()
            {
                CookieContainer = _cookie.Cookies,
                UseCookies = true,
                UseDefaultCredentials = false
            };
            _handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _client = new(handler: _handler, disposeHandler: true)
            {
                BaseAddress = new Uri("https://instagram.com/"),
                Timeout = TimeSpan.FromSeconds(180)
            };
            _ua = UAHelper.GetUa();
            Client.DefaultRequestHeaders.Add("Accept-Language", "en-US");
            Client.DefaultRequestHeaders.Add("X-Instagram-AJAX", "1");
            Client.DefaultRequestHeaders.Add("User-Agent", _ua.Ua);
            Client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            Client.DefaultRequestHeaders.Add("Referer", "https://www.instagram.com");
            HttpResponseMessage response = Client.GetAsync("https://www.instagram.com").Result;
            this.Client.DefaultRequestHeaders.Add("X-CSRFToken", Cookie.GetCSRFTOKEN());
        }

        public Instagram(string username, string password, string url, string usernameProxy, string passwordProxy)
        {
            User = new(username.ToLower(), GetEncryptedPassword(password));
            Challenge_URL = "challenge/";
            _cookie = new();
            _proxy = new()
            {
                Address = new Uri(url),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    userName: usernameProxy,
                    password: passwordProxy
                    )
            };
            _handler = new()
            {
                CookieContainer = _cookie.Cookies,
                UseCookies = true,
                UseDefaultCredentials = false,
                Proxy = _proxy
            };
            _handler.UseProxy = true;
            _handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _client = new(handler: _handler, disposeHandler: true)
            {
                BaseAddress = new Uri("https://instagram.com/"),
                Timeout = TimeSpan.FromSeconds(180)
            };
            _ua = UAHelper.GetUa();
            Client.DefaultRequestHeaders.Add("Accept-Language", "en-US");
            Client.DefaultRequestHeaders.Add("X-Instagram-AJAX", "1");
            Client.DefaultRequestHeaders.Add("User-Agent", _ua.Ua);
            Client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            Client.DefaultRequestHeaders.Add("Referer", "https://www.instagram.com");
            HttpResponseMessage response = Client.GetAsync("https://www.instagram.com").Result;
            this.Client.DefaultRequestHeaders.Add("X-CSRFToken", Cookie.GetCSRFTOKEN());
        }

        public Instagram(string username, string password, string cookie, string claim)
        {
            User = new(username.ToLower(), GetEncryptedPassword(password));
            Challenge_URL = "challenge/";
            _cookie = new(cookie);
            _handler = new()
            {
                CookieContainer = _cookie.Cookies,
                UseCookies = true,
                UseDefaultCredentials = false
            };
            _handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _client = new(handler: _handler, disposeHandler: true)
            {
                BaseAddress = new Uri("https://instagram.com/"),
                Timeout = TimeSpan.FromSeconds(180)
            };
            _ua = UAHelper.GetUa();
            this.SetClaim(claim);
            Client.DefaultRequestHeaders.Add("Accept-Language", "en-US");
            Client.DefaultRequestHeaders.Add("X-Instagram-AJAX", "1");
            Client.DefaultRequestHeaders.Add("User-Agent", _ua.Ua);
            Client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            Client.DefaultRequestHeaders.Add("Referer", "https://www.instagram.com");
            HttpResponseMessage response = Client.GetAsync("https://www.instagram.com").Result;
            this.Client.DefaultRequestHeaders.Add("X-CSRFToken", Cookie.GetCSRFTOKEN());
        }

        public Instagram(string username, string password, string cookie, string claim, string url, string usernameProxy, string passwordProxy)
        {
            User = new(username.ToLower(), GetEncryptedPassword(password));
            Challenge_URL = "challenge/";
            _cookie = new(cookie);
            _proxy = new()
            {
                Address = new Uri(url),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    userName: usernameProxy,
                    password: passwordProxy
                    )
            };
            _handler = new()
            {
                CookieContainer = _cookie.Cookies,
                UseCookies = true,
                UseDefaultCredentials = false,
                Proxy = _proxy
            };
            _handler.UseProxy = true;
            _handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _client = new(handler: _handler, disposeHandler: true)
            {
                BaseAddress = new Uri("https://instagram.com/"),
                Timeout = TimeSpan.FromSeconds(180)
            };
            _ua = UAHelper.GetUa();
            this.SetClaim(claim);
            Client.DefaultRequestHeaders.Add("Accept-Language", "en-US");
            Client.DefaultRequestHeaders.Add("X-Instagram-AJAX", "1");
            Client.DefaultRequestHeaders.Add("User-Agent", _ua.Ua);
            Client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            Client.DefaultRequestHeaders.Add("Referer", "https://www.instagram.com");
            HttpResponseMessage response = Client.GetAsync("https://www.instagram.com").Result;
            this.Client.DefaultRequestHeaders.Add("X-CSRFToken", Cookie.GetCSRFTOKEN());
        }

        //Public Methods

        public string CookieString() => Cookie.ToString();

        public string GetIgClaim() => this.GetClaim();

        public string GetUserID() => _cookie.GetId();

        //Internal Methods

        internal string GetEncryptedPassword(string password, long? providedTime = null)
        {
            long time = providedTime ?? DateTime.UtcNow.ToUnixTime();
            return $"#PWD_INSTAGRAM:0:{time}:{password}";
        }
    }
}
