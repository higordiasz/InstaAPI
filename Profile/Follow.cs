using InstaAPI.Header;
using InstaAPI.Helpers.Errors;
using InstaAPI.Helpers.Preloads;
using InstaAPI.Helpers.WebHelper;
using InstaAPI.Model;
using InstaAPI.Model.Return;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstaAPI.Profile.Follow
{
    public static class FollowClass
    {
        /// <summary>
        /// Get User ID in Search Bar
        /// </summary>
        /// <param name="insta">Instagram instance</param>
        /// <param name="id">Id</param>
        /// <returns>
        /// 3 = Already follow user
        /// 2 = Failed to follow user
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
        public static async Task<IReturn> FollowUserById(this Instagram insta, string id, int attemps = 0)
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
                    insta.HeaderFollow();
                    HttpRequestMessage req = new(HttpMethod.Post, $"https://www.instagram.com/web/friendships/{id}/follow/");
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
                            ret.Response = "Failed to follow user";
                            return ret;
                        }
                        if (!resContent.Contains("\"result\":\"following\""))
                        {
                            ret.Status = 2;
                            ret.Response = "Failed to follow user ";
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
                            return await insta.FollowUserById(id, i);
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
                                return await insta.FollowUserById(id, i);
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
    
        public static async Task<IRelation> GetRelationById(this Instagram insta, string id, int attemps = 0)
        {
            //https://i.instagram.com/api/v1/friendships/show/50973861825/
            IRelation ret = new(0, "Default");
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
                    insta.HeaderRelation();
                    HttpRequestMessage req = new(HttpMethod.Post, $"https://i.instagram.com/api/v1/friendships/show/{id}/");
                    HttpResponseMessage res = await insta.Client.SendAsync(req);
                    string resContent = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        try
                        {
                            Relation aux = JsonConvert.DeserializeObject<Relation>(resContent);
                            ret.Status = 1;
                            ret.Relation = aux;
                            ret.Json = resContent;
                            ret.Response = "Success";
                            return ret;
                        }
                        catch
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
                            if (!resContent.Contains("\"status\":\"fail\""))
                            {
                                ret.Status = -1;
                                ret.Response = "Failed to load relation";
                                return ret;
                            }
                            ret.Status = -7;
                            ret.Response = "Failed to load relation.";
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
                            return await insta.GetRelationById(id, i);
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
                                return await insta.GetRelationById(id, i);
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
