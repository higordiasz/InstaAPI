using InstaAPI.Header;
using InstaAPI.Helpers.Errors;
using InstaAPI.Helpers.Preloads;
using InstaAPI.Helpers.WebHelper;
using InstaAPI.Model;
using InstaAPI.Model.Return;
using InstaAPI.Profile.Id;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstaAPI.Profile.UserProfile
{
    public static class UserProfileClass
    {
        public static async Task<IReturn> GetUserProfileFromUsername(this Instagram insta, string username, int attemps = 0)
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

                    IReturn userId = await insta.GetIdBySearchBar(username);
                    if (userId.Status == 1)
                    {
                        insta.HeaderProfile();
                        HttpRequestMessage req = new(HttpMethod.Get, $"https://www.instagram.com/{username}/");
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
                            if (resContent.Contains("feedback_required"))
                            {
                                ret.Response = "Account temporary blocked";
                                ret.Status = -6;
                                return ret;
                            }
                            if (WebHelperClass.CanReadJson(resContent))
                            {
                                insta._sharedData = JsonConvert.DeserializeObject(WebHelperClass.GetJson(resContent));
                                string ajax = !String.IsNullOrEmpty(insta._sharedData.rollout_hash.ToString()) ? insta._sharedData.rollout_hash.ToString() : "1";
                                insta.SetAjax(ajax);
                            }
                            else
                            {
                                insta.SetAjax(WebHelperClass.GetAjax(resContent));
                            }
                            insta.HeaderIProfileInfo();
                            string userid = userId.Response;
                            Console.WriteLine(userid);
                            req = new(HttpMethod.Get, $"https://i.instagram.com/api/v1/web/get_ruling_for_content/?content_type=PROFILE&target_id={userid}");
                            res = await insta.Client.SendAsync(req);
                            resContent = await res.Content.ReadAsStringAsync();
                            if (res.IsSuccessStatusCode)
                            {
                                req = new(HttpMethod.Get, $"https://i.instagram.com/api/v1/users/web_profile_info/?username={username}");
                                res = await insta.Client.SendAsync(req);
                                resContent = await res.Content.ReadAsStringAsync();
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
                                    if (resContent.Contains("feedback_required"))
                                    {
                                        ret.Response = "Account temporary blocked";
                                        ret.Status = -6;
                                        return ret;
                                    }
                                    if (!resContent.Contains("\"status\":\"ok\""))
                                    {
                                        Console.WriteLine(resContent);
                                        ret.Status = -1;
                                        ret.Response = "Failed to find user";
                                        return ret;
                                    }
                                    dynamic json = JsonConvert.DeserializeObject(resContent);
                                    ret.Json = json;
                                    ret.Response = "Success";
                                    ret.Status = 1;
                                    return ret;
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
                                        return await insta.GetUserProfileFromUsername(username, i);
                                    }
                                    if (resContent.IndexOf("Resource Isolation Policy Violated") > -1)
                                    {
                                        ret.Response = "Violated resource";
                                        ret.Status = -4;
                                        return ret;
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
                                            return await insta.GetUserProfileFromUsername(username, i);
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
                                Console.WriteLine(resContent);
                                ret.Response = "Not allowed";
                                ret.Status = -5;
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
                                return await insta.GetUserProfileFromUsername(username, i);
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
                                    return await insta.GetUserProfileFromUsername(username, i);
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
                        return userId;
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
                insta.ErrWrite(err, "GetUserProfileFromUsername");
                ret.Status = -1;
                ret.Response = "An unexpected error has occurred";
            }
            return ret;
        }
    }
}
