using InstaAPI.Header;
using InstaAPI.Helpers.Errors;
using InstaAPI.Helpers.Preloads;
using InstaAPI.Helpers.WebHelper;
using InstaAPI.Model.Return;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstaAPI.Profile.Id
{
    public static class IdClass
    {
        /// <summary>
        /// Get User ID in Search Bar
        /// </summary>
        /// <param name="insta">Instagram instance</param>
        /// <param name="username">Username</param>
        /// <returns>
        /// 2 = User not Found
        /// 1 = Success
        /// 0 = Default
        /// -1 = Error
        /// -2 = User Not found
        /// -3 = Account disconnected
        /// -4 = 
        /// -5 = Challenge
        /// -6 = Feedback
        /// 
        /// </returns>
        public async static Task<IReturn> GetIdBySearchBar(this Instagram insta, string username, int attemps = 0)
        {
            IReturn ret = new(0, "Default");
            try
            {
                bool preload = await insta.Preload();
                if (preload)
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
                    insta.HeaderSearchBar();
                    HttpRequestMessage req = new(HttpMethod.Get, $"https://www.instagram.com/web/search/topsearch/?context=blended&query={username.ToLower()}&rank_token=0.9950787703275124&include_reel=true");
                    HttpResponseMessage res = await insta.Client.SendAsync(req);
                    string resContent = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        if (resContent.IndexOf("\"challengeType\"") > -1)
                        {
                            string json2 = WebHelperClass.GetJson(resContent);
                            insta._sharedData = JsonConvert.DeserializeObject(json2);
                            ret.Status = -5;
                            ret.Response = "Block: " + insta._sharedData.entry_data.Challenge[0].challengeType.ToString();
                            return ret;
                        }
                        if (resContent.Contains("\"viewer\": null,") && resContent.Contains("\"viewerId\": null") && resContent.Contains("\"LoginAndSignupPage\""))
                        {
                            ret.Status = -3;
                            ret.Response = "Account disconnected";
                            ret.Json = null;
                            return ret;
                        }
                        if (!resContent.Contains("\"status\":\"ok\""))
                        {
                            ret.Status = -1;
                            ret.Response = "Failed to make the request ";
                            return ret;
                        }
                        if(resContent.Contains("\"users\":[],"))
                        {
                            ret.Status = 2;
                            ret.Response = "User not found";
                            return ret;
                        }
                        dynamic json = JsonConvert.DeserializeObject(resContent);
                        ret.Json = json;
                        if (json.users[0].user.username.ToString() == username.ToLower())
                        {
                            ret.Response = json.users[0].user.pk.ToString();
                            ret.Status = 1;
                            return ret;
                        }
                        else
                        {
                            ret.Status = 2;
                            ret.Response = "User not found";
                            return ret;
                        }
                    }
                    else
                    {
                        if (resContent.IndexOf("Login  Instagram") > -1)
                        {
                            ret.Response = "Account disconnected";
                            ret.Status = -3;
                            return ret;
                        }
                        if (resContent.IndexOf("Go back to Instagram.") > -1 && attemps < 3)
                        {
                            await Task.Delay(1246);
                            int i = attemps + 1;
                            return await insta.GetIdBySearchBar(username, i);
                        }
                        string title = WebHelperClass.GetTitle(resContent);
                        if (title.IndexOf("Instagram") > -1)
                        {
                            ret.Response = "User not Found";
                            ret.Status = 2;
                            return ret;
                        }
                        else
                        {
                            if (resContent.IndexOf("Account disconnected") > -1)
                            {
                                ret.Response = "Account disconnected";
                                ret.Status = -3;
                                return ret;
                            }
                            if (resContent.IndexOf("Go back to Instagram.") > -1 && attemps < 3)
                            {
                                await Task.Delay(1246);
                                int i = attemps + 1;
                                return await insta.GetIdBySearchBar(username, i);
                            }
                            else
                            {
                                if (resContent.IndexOf("\"checkpoint_required\"") > -1)
                                {
                                    ret.Status = -5;
                                    ret.Response = resContent;
                                    dynamic challenge = JsonConvert.DeserializeObject(resContent);
                                    insta.Challenge_URL = challenge.checkpoint_url;
                                    return ret;
                                }
                                else
                                {
                                    if (resContent.IndexOf("\"feedback_required\"") > -1)
                                    {
                                        ret.Status = -6;
                                        ret.Response = resContent;
                                        return ret;
                                    }
                                    else
                                    {
                                        ret.Response = "User not found";
                                        ret.Status = 2;
                                        return ret;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    ret.Response = "Unable to load instagram";
                    ret.Status = -2;
                }
            }
            catch (Exception err)
            {
                ret.Err = err;
                insta.ErrWrite(err, "GetIdBySearchBar");
                ret.Status = -1;
                ret.Response = "An unexpected error has occurred";
            }
            return ret;
        }
    }
}
