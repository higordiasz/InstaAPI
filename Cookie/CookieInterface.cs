using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Cookies.Interface
{
    public interface CookieInterface
    {
        public string ToString();

        public void LoadCookiesFromString(string cookies);

        public string GetCSRFTOKEN();

        public string GetId();

        CookieCollection GetAllCookies(CookieContainer cookieJar);
    }
}
