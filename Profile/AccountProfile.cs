using InstaAPI.Header;
using InstaAPI.Helpers.Preloads;
using InstaAPI.Helpers.WebHelper;
using InstaAPI.Model.Return;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Profile.Account
{
    public static class AccountProfile
    {
        public static async Task<string> GetGender(this Instagram insta)
        {
            string ret = "M";
            try
            {
                bool preload = await insta.Preload();
                if(preload)
                {
                    string content = insta.PreloadContent();
                    if (WebHelperClass.CanReadJson(content))
                    {
                        insta._sharedData = JsonConvert.DeserializeObject(WebHelperClass.GetJson(content));
                        string ajax = !String.IsNullOrEmpty(insta._sharedData.rollout_hash.ToString()) ? insta._sharedData.rollout_hash.ToString() : "1";
                        insta.SetAjax(ajax);
                    }
                    else
                    {
                        insta.SetAjax(WebHelperClass.GetAjax(content));
                    }
                    insta.HeaderProfile();
                    HttpRequestMessage req = new(HttpMethod.Get, "https://www.instagram.com/accounts/edit/?__a=1");
                    HttpResponseMessage res = await insta.Client.SendAsync(req);
                    string resContent = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        try
                        {
                            dynamic json = JsonConvert.DeserializeObject(resContent);
                            return json.form_data.gender.ToString() == 2 ? "F" : "M";
                        }
                        catch
                        {
                            return "M";
                        }
                    }
                    else
                    {
                        return "M";
                    }
                }
                else
                {
                    return "M";
                }
            }
            catch
            {

            }
            return ret;
        }
    }
}
