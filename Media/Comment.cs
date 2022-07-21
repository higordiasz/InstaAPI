using InstaAPI.Header;
using InstaAPI.Helpers.Errors;
using InstaAPI.Helpers.Preloads;
using InstaAPI.Helpers.WebHelper;
using InstaAPI.Model;
using InstaAPI.Model.Return;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstaAPI.Media.Comment
{
    public static class CommentClass
    {
        /// <summary>
        /// Comment media by mediaId
        /// </summary>
        /// <param name="insta">Instagram instance</param>
        /// <param name="mediaId">MediaId</param>
        /// <param name="comment">Comment</param>
        /// <returns>
        /// 2 = Failed to comment media
        /// 1 = Success
        /// 0 = Default
        /// -1 = Error
        /// -2 = Media Not found
        /// -3 = Account disconnected
        /// -4 = 
        /// -5 = Challenge
        /// -6 = Feedback
        /// 
        /// </returns>
        public static async Task<IReturn> CommentMediaByMediaId(this Instagram insta, string mediaId, string comment, int attemps = 0)
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
                    insta.HeaderComment();
                    Dictionary<string, string> sender = new()
                    {
                        { "comment_text", comment},
                        { "replied_to_comment_id", ""}
                    };
                    insta.HeaderContentLenght(36 + comment.Length);
                    HttpRequestMessage req = new(HttpMethod.Post, $"https://www.instagram.com/web/comments/{mediaId}/add/") { Content = new FormUrlEncodedContent(sender)};
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
                            ret.Response = "Failed to comment media";
                            return ret;
                        }
                        if (resContent.Contains("feedback_required"))
                        {
                            ret.Response = "Account temporary blocked";
                            ret.Status = -6;
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
                            return await insta.CommentMediaByMediaId(mediaId, comment, i);
                        }
                        string title = WebHelperClass.GetTitle(resContent);
                        if (title.IndexOf("Instagram") > -1)
                        {
                            ret.Response = "Media not Found";
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
                                return await insta.CommentMediaByMediaId(mediaId, comment, i);
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
                                        ret.Response = "Media not found";
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
                insta.ErrWrite(err, "LikeMediaByMediaId");
                ret.Status = -1;
                ret.Response = "An unexpected error has occurred";
            }
            return ret;
        }
    }
}
