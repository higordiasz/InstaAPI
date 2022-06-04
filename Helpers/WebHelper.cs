using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Helpers.WebHelper
{
    internal static class WebHelperClass
    {
        const string StartTag = "type=\"text/javascript\">window._sharedData";
        const string EndTag = ";</script>";
        public static bool CanReadJson(this string html)
        {
            return html.Contains(StartTag);
        }
        public static string GetJson(this string html)
        {
            try
            {
                if (html.CanReadJson())
                {
                    var json = html.Substring(html.IndexOf(StartTag) + StartTag.Length);
                    json = json.Substring(0, json.IndexOf(EndTag));
                    json = json.Substring(json.IndexOf("=") + 2);
                    return json;
                }
            }
            catch (Exception ex) { Console.WriteLine($"WebHelper.GetJson ex: {ex.Message}\r\nSource: {ex.Source}\r\nTrace: {ex.StackTrace}"); }
            return null;
        }
        public static string GetAjax(this string html)
        {
            try
            {
                if (html.Contains("\"rollout_hash\":"))
                {
                    int index = html.IndexOf("\"rollout_hash\":");
                    string b = html.Substring(index, 33);
                    string[] aux = b.Split(":");
                    string c = aux[1];
                    return c.Split('"')[1];
                }
                return "1";
            }
            catch
            {
                return "1";
            }
        }

        public static string GetTitle(this string html)
        {
            string ret = html.Substring(html.IndexOf("<title>") + 7);
            ret = ret.Substring(0, ret.IndexOf("</title>"));
            return ret;
        }
        public static bool CanReadJsonMedia(this string html, string code)
        {
            string search = $"<script type=\"text/javascript\">window.__additionalDataLoaded('/p/{code}/',";
            if (html.IndexOf(search) > 1)
                return true;
            return false;
        }
        public static string GetJsonMedia(this string html, string code)
        {
            try
            {
                if (html.CanReadJsonMedia(code))
                {
                    string search = $"<script type=\"text/javascript\">window.__additionalDataLoaded('/p/{code}/',";
                    var json = html.Substring(html.IndexOf(search) + search.Length);
                    json = json.Substring(0, json.IndexOf(EndTag) - 1);
                    return json;
                }
            }
            catch (Exception ex) { Console.WriteLine($"WebHelper.GetJsonMedia ex: {ex.Message}\r\nSource: {ex.Source}\r\nTrace: {ex.StackTrace}"); }
            return null;
        }
    }
}
