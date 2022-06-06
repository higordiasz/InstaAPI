using InstaAPI.Header;
using InstaAPI.Helpers.Errors;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstaAPI.Helpers.Preloads
{
    public static class Preloads
    {
        public static string _content = "";
        public async static Task<bool> Preload(this Instagram insta, int attemps = 0)
        {
            bool ret = false;
            try
            {
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
                HttpRequestMessage req = new(HttpMethod.Get, "https://www.instagram.com/");
                HttpResponseMessage res = await insta.Client.SendAsync(req);
                if (res.IsSuccessStatusCode)
                    ret = true;
                else
                    ret = false;
                _content = await res.Content.ReadAsStringAsync();
                if (attemps < 3 && ret == false)
                {
                    int i = attemps + 1;
                    return await insta.Preload(i);
                }
            }
            catch (Exception err)
            {
                insta.ErrWrite(err, "Preload");
            }
            return ret;
        }

        public static string PreloadContent(this Instagram insta) => _content;

    }
}
