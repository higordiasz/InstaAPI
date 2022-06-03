using System;
using System.Collections.Generic;
using System.Linq;

namespace InstaAPI.Helpers.UA
{
    public class UserAgent
    {
        public string Ua { get; set; }
        public string Sec_Ch_Ua { get; set; }
        public string Sec_Ch_Ua_Mobile { get; set; }
        public string Sec_Ch_Ua_Platform { get; set; }

        public UserAgent(string a, string b, string c, string d)
        {
            Ua = a;
            Sec_Ch_Ua = b;
            Sec_Ch_Ua_Mobile = c;
            Sec_Ch_Ua_Platform = d;
        }
    }
    internal class Ua
    {
        public string sec_ch_ua { get; set; }
        public string sec_ch_ua_mobile { get; set; }
        public string sec_ch_ua_platform { get; set; }

        public Ua(string a, string b, string c)
        {
            sec_ch_ua = a;
            sec_ch_ua_mobile = b;
            sec_ch_ua_platform = c;
        }
    }
    static class UAHelper
    {
        private static Dictionary<string, Ua> Uas = new() 
        {
            { "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36", new("\" Not A;Brand\"; v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"", "?0", "\"Windows\"")},
            { "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.30", new("\" Not A; Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Microsoft Edge\";v=\"102\"", "?0", "Windows") },
            { "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:101.0) Gecko/20100101 Firefox/101.0", new("", "", "") },
            { "Mozilla/5.0 (iPhone; CPU iPhone OS 14_7_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.2 Mobile/15E148 Safari/604.1", new("", "", "") },
            { "Mozilla/5.0 (Linux; Android 9; SM-N9600) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.78 Mobile Safari/537.36", new("\" Not A; Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"", "?1", "\"Android\"") },
            //{ "", new("", "", "") },
            //{ "", new("", "", "") },
            //{ "", new("", "", "") }
        };

        public static UserAgent GetUa()
        {
            int count = Uas.Count;
            Random rand = new();
            int r = rand.Next(0, count);
            return new UserAgent(Uas.ElementAt(r).Key, Uas.ElementAt(r).Value.sec_ch_ua, Uas.ElementAt(r).Value.sec_ch_ua_mobile, Uas.ElementAt(r).Value.sec_ch_ua_platform);
        }

    }
}
