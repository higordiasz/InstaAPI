using InstaAPI.Cookies;
using InstaAPI.Helpers.UA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Header
{
    public static class Header
    {
        private static string X_IG_WWW_CLAIM { get; set; }
        private static string InstagramAjax { get; set; }

        private static string RequestedWith = "XMLHttpRequest";
        private static string AcceptLanguage = "en-US";
        private static string CacheControl = "max-age=0";
        private static string AsbdId = "198387";
        private static string AppId = "936619743392459";
        private static string Origin = "https://www.instagram.com";
        private static string Host = "www.instagram.com";
        private static string Connection = "keep-alive";

        public static void SetAjax(this Instagram insta, string ajax) => InstagramAjax = ajax;
        public static string GetAjax(this Instagram insta) => InstagramAjax;
        public static void SetClaim(this Instagram insta, string claim) => X_IG_WWW_CLAIM = claim;
        public static string GetClaim(this Instagram insta) => X_IG_WWW_CLAIM;

        public static void HeaderClaim(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("X-IG-WWW-Claim", X_IG_WWW_CLAIM);
        public static void HeaderAjax(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("X-Instagram-AJAX", InstagramAjax);
        public static void HeaderReferer(this Instagram insta, string referer) => insta.Client.DefaultRequestHeaders.Add("Referer", referer);
        public static void HeaderRequested(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("X-Requested-With", RequestedWith);
        public static void HeaderAccept(this Instagram insta, string accept) => insta.Client.DefaultRequestHeaders.Add("Accept", accept);
        public static void HeaderAcceptEncoding(this Instagram insta, string encoding) => insta.Client.DefaultRequestHeaders.Add("Accept-Encoding", encoding);
        public static void HeaderAcceptLanguage(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("Accept-Language", AcceptLanguage);
        public static void HeaderAsbdId(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("X-ASBD-ID", AsbdId);
        public static void HeaderAppId(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("X-IG-App-ID", AppId);
        public static void HeaderOrigin(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("Origin", Origin);
        public static void HeaderSecFetchMode(this Instagram insta, string mode) => insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", mode);
        public static void HeaderSecFetchSite(this Instagram insta, string site) => insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Site", site);
        public static void HeaderSecFetchDest(this Instagram insta, string dest) => insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", dest);
        public static void HeaderSecFetchUser(this Instagram insta, string user) => insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-User", user);
        public static void HeaderUpgradeInsecureRequests(this Instagram insta, string upgrade) => insta.Client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", upgrade);
        public static void HeaderViewPortWidth(this Instagram insta, string view) => insta.Client.DefaultRequestHeaders.Add("Viewport-Width", view);
        public static void HeaderCsrfToken(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("X-CSRFToken", insta.Cookie.GetCSRFTOKEN());
        public static void HeaderContentLenght(this Instagram insta, string lenght) => insta.Client.DefaultRequestHeaders.Add("Content-Length", lenght);
        public static void HeaderContentType(this Instagram insta, string type) => insta.Client.DefaultRequestHeaders.Add("Content-Type", type);
        public static void HeaderConnection(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("Connection", Connection);
        public static void HeaderHost(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("Host", Host);
        public static void HeaderHost(this Instagram insta, string host) => insta.Client.DefaultRequestHeaders.Add("Host", host);
        public static void HeaderUserAgent(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("User-Agent", insta.Ua.Ua);
        public static void HeaderCacheControl(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("Cache-Control", CacheControl);
        public static void HeaderSecChUa(this Instagram insta)
        {
            if (insta.Ua.Sec_Ch_Ua != "")
                insta.Client.DefaultRequestHeaders.Add("sec-ch-ua", insta.Ua.Sec_Ch_Ua);
        }
        public static void HeaderSecChUaMobile(this Instagram insta)
        {
            if (insta.Ua.Sec_Ch_Ua_Mobile != "")
                insta.Client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", insta.Ua.Sec_Ch_Ua_Mobile);
        }
        public static void HeaderSecChUaPlatform(this Instagram insta)
        {
            if (insta.Ua.Sec_Ch_Ua_Platform != "")
                insta.Client.DefaultRequestHeaders.Add("sec-ch-ua-platform", insta.Ua.Sec_Ch_Ua_Platform);
        }
        public static void HeaderContentLenght(this Instagram insta, int lenght) => insta.Client.DefaultRequestHeaders.Add("Content-Length", lenght.ToString());
        public static void HeaderContentType(this Instagram insta) => insta.Client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
        public static void ClearHeader(this Instagram insta) => insta.Client.DefaultRequestHeaders.Clear();

        public static void HeaderLogin(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderUserAgent();
            insta.HeaderSecChUa();
            insta.HeaderSecChUaMobile();
            insta.HeaderSecChUaPlatform();
            insta.HeaderAppId();
            insta.HeaderClaim();
            insta.HeaderAjax();
            insta.HeaderRequested();
            insta.HeaderSecFetchDest("empty");
            insta.HeaderSecFetchMode("cors");
            insta.HeaderSecFetchSite("same-origin");
            insta.HeaderOrigin();
            //insta.HeaderContentType("application/x-www-form-urlencoded");
            insta.HeaderCsrfToken();
            insta.HeaderReferer("https://www.instagram.com/");
        }

        public static void HeaderDefault(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            insta.HeaderAcceptLanguage();
            insta.HeaderUserAgent();
            insta.HeaderSecChUa();
            insta.HeaderSecChUaMobile();
            insta.HeaderSecChUaPlatform();
            insta.HeaderSecFetchDest("document");
            insta.HeaderSecFetchMode("navigate");
            insta.HeaderSecFetchSite("none");
            insta.HeaderSecFetchUser("?1");
            insta.HeaderUpgradeInsecureRequests("1");
            insta.HeaderConnection();
            insta.HeaderHost();
        }

        public static void HeaderSearchBar(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderHost();
            insta.HeaderConnection();
            insta.HeaderSecChUa();
            insta.HeaderAppId();
            insta.HeaderClaim();
            insta.HeaderSecChUaMobile();
            insta.HeaderUserAgent();
            insta.HeaderAccept("*/*");
            insta.HeaderCsrfToken();
            insta.HeaderRequested();
            insta.HeaderAsbdId();
            insta.HeaderSecChUaPlatform();
            insta.HeaderSecFetchSite("same-origin");
            insta.HeaderSecFetchMode("cors");
            insta.HeaderSecFetchDest("empty");
            insta.HeaderReferer("https://www.instagram.com/");
            insta.HeaderAcceptLanguage();
        }

        public static void HeaderFollow(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderHost();
            insta.HeaderConnection();
            insta.HeaderSecChUa();
            insta.HeaderAppId();
            insta.HeaderClaim();
            insta.HeaderRequested();
            insta.HeaderSecChUaMobile();
            insta.HeaderAjax();
            insta.HeaderAccept("*/*");
            insta.HeaderCsrfToken();
            insta.HeaderUserAgent();
            insta.HeaderAsbdId();
            insta.HeaderSecChUaPlatform();
            insta.HeaderOrigin();
            insta.HeaderSecFetchSite("same-origin");
            insta.HeaderSecFetchMode("cors");
            insta.HeaderSecFetchDest("empty");
            insta.HeaderReferer("https://www.instagram.com/");
            insta.HeaderAcceptLanguage();
        }

        public static void HeaderLike(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderHost();
            insta.HeaderConnection();
            insta.HeaderSecChUa();
            insta.HeaderAppId();
            insta.HeaderClaim();
            insta.HeaderRequested();
            insta.HeaderSecChUaMobile();
            insta.HeaderAjax();
            insta.HeaderAccept("*/*");
            insta.HeaderCsrfToken();
            insta.HeaderUserAgent();
            insta.HeaderAsbdId();
            insta.HeaderSecChUaPlatform();
            insta.HeaderOrigin();
            insta.HeaderSecFetchSite("same-origin");
            insta.HeaderSecFetchMode("cors");
            insta.HeaderSecFetchDest("empty");
            insta.HeaderReferer("https://www.instagram.com/");
            insta.HeaderAcceptLanguage();
        }

        public static void HeaderComment(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderHost();
            insta.HeaderConnection();
            insta.HeaderSecChUa();
            insta.HeaderAppId();
            insta.HeaderClaim();
            insta.HeaderRequested();
            insta.HeaderSecChUaMobile();
            insta.HeaderAjax();
            insta.HeaderContentType();
            insta.HeaderAccept("*/*");
            insta.HeaderCsrfToken();
            insta.HeaderUserAgent();
            insta.HeaderAsbdId();
            insta.HeaderSecChUaPlatform();
            insta.HeaderOrigin();
            insta.HeaderSecFetchSite("same-origin");
            insta.HeaderSecFetchMode("cors");
            insta.HeaderSecFetchDest("empty");
            insta.HeaderReferer("https://www.instagram.com/");
            insta.HeaderAcceptLanguage();
        }

        public static void HeaderStories(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderHost("i.instagram.com");
            insta.HeaderConnection();
            insta.HeaderSecChUa();
            insta.HeaderClaim();
            insta.HeaderSecChUaMobile();
            insta.HeaderUserAgent();
            insta.HeaderAccept("*/*");
            insta.HeaderAsbdId();
            insta.HeaderCsrfToken();
            insta.HeaderSecChUaPlatform();
            insta.HeaderAppId();
            insta.HeaderAjax();
            insta.HeaderOrigin();
            insta.HeaderSecFetchSite("same-site");
            insta.HeaderSecFetchMode("cors");
            insta.HeaderSecFetchDest("empty");
            insta.HeaderReferer("https://www.instagram.com/");
            insta.HeaderAcceptLanguage();
        }

        public static void HeaderProfile(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderHost();
            insta.HeaderConnection();
            insta.HeaderCacheControl();
            insta.HeaderSecChUa();
            insta.HeaderSecChUaMobile();
            insta.HeaderSecChUaPlatform();
            insta.HeaderUpgradeInsecureRequests("1");
            insta.HeaderUserAgent();
            insta.HeaderAccept("application/json");
            insta.HeaderSecFetchSite("none");
            insta.HeaderSecFetchMode("navigate");
            insta.HeaderSecFetchDest("document");
            insta.HeaderAcceptLanguage();
        }

        public static void HeaderProfileInfo(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderHost("i.instagram.com");
            insta.HeaderConnection();
            insta.HeaderSecChUa();
            insta.HeaderClaim();
            insta.HeaderSecChUaMobile();
            insta.HeaderUserAgent();
            insta.HeaderAccept("*/*");
            insta.HeaderAsbdId();
            insta.HeaderCsrfToken();
            insta.HeaderSecChUaPlatform();
            insta.HeaderAppId();
            insta.HeaderOrigin();
            insta.HeaderSecFetchSite("same-site");
            insta.HeaderSecFetchMode("cors");
            insta.HeaderSecFetchDest("empty");
            insta.HeaderReferer("https://www.instagram.com/");
            insta.HeaderAcceptLanguage();
        }

        public static void HeaderRelation(this Instagram insta)
        {
            if (String.IsNullOrEmpty(X_IG_WWW_CLAIM))
                X_IG_WWW_CLAIM = "0";
            insta.ClearHeader();
            insta.HeaderHost();
            insta.HeaderConnection();
            insta.HeaderSecChUa();
            insta.HeaderAppId();
            insta.HeaderClaim();
            insta.HeaderRequested();
            insta.HeaderSecChUaMobile();
            insta.HeaderAjax();
            insta.HeaderAccept("*/*");
            insta.HeaderCsrfToken();
            insta.HeaderUserAgent();
            insta.HeaderAsbdId();
            insta.HeaderSecChUaPlatform();
            insta.HeaderOrigin();
            insta.HeaderSecFetchSite("same-site");
            insta.HeaderSecFetchMode("cors");
            insta.HeaderSecFetchDest("empty");
            insta.HeaderReferer("https://www.instagram.com/");
            insta.HeaderAcceptLanguage();
        }
    }
}
