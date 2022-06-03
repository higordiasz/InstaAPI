using InstaAPI.Cookies.Interface;
using System;
using System.Net;
using System.Reflection;
using System.Collections;

namespace InstaAPI.Cookies
{
    public class CookieHelper : CookieInterface
    {
        private CookieContainer _cookies;
        internal CookieContainer Cookies { get { return _cookies; } }

        public CookieHelper()
        {
            _cookies = new();
        }

        public CookieHelper(string cookies)
        {
            _cookies = new();
            var array = cookies.Split(";");
            if (array.Length > 2)
            {
                for (int i = 0; i < array.Length; i += 4)
                {
                    Cookie c = new()
                    {
                        Name = array[i],
                        Value = array[i + 1],
                        Domain = array[i + 2],
                        Path = array[i + 3]
                    };
                    _cookies.Add(c);
                }
            }
        }

        public override string ToString()
        {
            CookieCollection collector = GetAllCookies(this.Cookies);
            string ret = "";
            foreach (Cookie cookie in collector)
            {
                if (ret != "")
                    ret += ";";
                ret += $"{cookie.Name};{cookie.Value};{cookie.Domain};{cookie.Path}";
            }
            return ret;
        }

        public void LoadCookiesFromString(string cookies)
        {
            _cookies = new();
            var array = cookies.Split(";");
            if (array.Length > 2)
            {
                for (int i = 0; i < array.Length; i += 4)
                {
                    Cookie c = new()
                    {
                        Name = array[i],
                        Value = array[i + 1],
                        Domain = array[i + 2],
                        Path = array[i + 3]
                    };
                    _cookies.Add(c);
                }
            }
        }

        public string GetCSRFTOKEN()
        {
            CookieCollection collector = GetAllCookies(this.Cookies);
            string ret = "";
            foreach (Cookie cookie in collector)
            {
                if (cookie.Name == "csrftoken")
                    ret = cookie.Value;
            }
            return ret;
        }

        public string GetId()
        {
            CookieCollection collector = GetAllCookies(this.Cookies);
            string ret = "";
            foreach (Cookie cookie in collector)
            {
                if (cookie.Name == "ds_user_id")
                    ret = cookie.Value;
            }
            return ret;
        }

        public CookieCollection GetAllCookies(CookieContainer cookieJar)
        {
            CookieCollection cookieCollection = new CookieCollection();

            Hashtable table = (Hashtable)cookieJar.GetType().InvokeMember("m_domainTable",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.GetField |
                                                                            BindingFlags.Instance,
                                                                            null,
                                                                            cookieJar,
                                                                            new object[] { });

            foreach (var tableKey in table.Keys)
            {
                String str_tableKey = (string)tableKey;

                if (str_tableKey[0] == '.')
                {
                    str_tableKey = str_tableKey.Substring(1);
                }

                SortedList list = (SortedList)table[tableKey].GetType().InvokeMember("m_list",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.GetField |
                                                                            BindingFlags.Instance,
                                                                            null,
                                                                            table[tableKey],
                                                                            new object[] { });

                foreach (var listKey in list.Keys)
                {
                    String url = "https://" + str_tableKey + (string)listKey;
                    cookieCollection.Add(cookieJar.GetCookies(new Uri(url)));
                }
            }

            return cookieCollection;
        }
    }
}
