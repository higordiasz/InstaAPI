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
        private static UserAgent UA { get; set; }
        private static string InstagramAjax { get; set; }

        private static string RequestedWith = "XMLHttpRequest";
        private static string Accept = "";
        private static string AcceptEncoding = "";
        private static string AcceptLanguage = "";
        private static string CacheControl = "max-age=0";
        private static string AsbdId = "198387";
        private static string AppId = "936619743392459";
        private static string Origin = "https://www.instagram.com";

        public static void SetAjax(this InstaAPI insta, string ajax) => InstagramAjax = ajax;
        public static void SetUa(this InstaAPI insta, UserAgent ua) => UA = ua;
        public static void SetClaim(this InstaAPI isnta, string claim) => X_IG_WWW_CLAIM = claim;

        public static void HeaderClaim(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("X-IG-WWW-Claim", X_IG_WWW_CLAIM);
        public static void HeaderAjax(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("X-Instagram-AJAX", InstagramAjax);
        public static void HeaderReferer(this InstaAPI insta, string referer) => insta.Client.DefaultRequestHeaders.Add("Referer", referer);
        public static void HeaderRequested(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("X-Requested-With", RequestedWith);
        public static void HeaderAccept(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("Accept", Accept);
        public static void HeaderAcceptEncoding(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("Accept-Encoding", AcceptEncoding);
        public static void HeaderAcceptLanguage(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("Accept-Language", AcceptLanguage);
        public static void HeaderAsbdId(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("X-ASBD-ID", AsbdId);
        public static void HeaderAppId(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("X-IG-App-ID", AppId);
        public static void HeaderOrigin(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("Origin", Origin);
        public static void HeaderSecFetchMode(this InstaAPI insta, string mode) => insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", mode);
        public static void HeaderSecFetchSite(this InstaAPI insta, string site) => insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Site", site);
        public static void HeaderSecFetchDest(this InstaAPI insta, string dest) => insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", dest);
        public static void HeaderViewPortWidth(this InstaAPI insta, string view) => insta.Client.DefaultRequestHeaders.Add("Viewport-Width", view);
        public static void HeaderCsrfToken(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("X-CSRFToken", insta.Cookie.GetCSRFTOKEN());
        public static void HeaderContentLenght(this InstaAPI insta, string lenght) => insta.Client.DefaultRequestHeaders.Add("Content-Length", lenght);
        public static void HeaderContentType(this InstaAPI insta, string type) => insta.Client.DefaultRequestHeaders.Add("Content-Type", type);
        public static void HeaderUserAgent(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("User-Agent", UA.Ua);
        public static void HeaderCacheControl(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Add("Cache-Control", CacheControl);
        public static void HeaderSecChUa(this InstaAPI insta)
        {
            if (UA.Sec_Ch_Ua != "")
                insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", UA.Sec_Ch_Ua);
        }
        public static void HeaderSecChUaMobile(this InstaAPI insta)
        {
            if (UA.Sec_Ch_Ua_Mobile != "")
                insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Site", UA.Sec_Ch_Ua_Mobile);
        }
        public static void HeaderSecChUaPlatform(this InstaAPI insta)
        {
            if (UA.Sec_Ch_Ua_Platform != "")
                insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", UA.Sec_Ch_Ua_Platform);
        }
        public static void ClearHeader(this InstaAPI insta) => insta.Client.DefaultRequestHeaders.Clear();
    }
}
