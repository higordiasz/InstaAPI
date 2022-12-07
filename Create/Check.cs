using InstaAPI.Header;
using InstaAPI.Helpers.Errors;
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

namespace InstaAPI.Create.Check
{
    public static class CheckClass
    {
        /// <summary>
        /// Check Username
        /// </summary>
        /// <param name="insta">InstaAPI instace</param>
        /// <param name="account">Account to Create</param>
        /// <returns>
        /// 1 = Username available
        /// -1 = Unexpected error
        /// -2 = Unable to load instagram
        /// -3 = Username is taken
        /// -4 = Unable to check
        /// 0 = Default - Error if send it
        /// </returns>
        public static async Task<IReturn> CheckUsername(this Instagram insta, string username, int attemp = 0)
        {
            IReturn ret = new(0, "Check Username method");
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
                    Dictionary<string, string> sender = new Dictionary<string, string>()
                    {
                        { "email", "" },
                        { "username", username },
                        { "first_name", "" },
                        { "opt_into_one_tap", "false" }
                    };
                    insta.HeaderCheck();
                    HttpRequestMessage req = new(HttpMethod.Post, "https://www.instagram.com/api/v1/web/accounts/web_create_ajax/attempt/") { Content = new FormUrlEncodedContent(sender) };
                    HttpResponseMessage res = await insta.Client.SendAsync(req);
                    string resContent = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        if (resContent.Contains("username_is_taken"))
                        {
                            ret.Response = "Username is taken";
                            ret.Status = -3;
                        }
                        else
                        {
                            ret.Response = "Username is available";
                            ret.Status = 1;
                        }
                    }
                    else
                    {
                        if (attemp >= 3)
                        {
                            if (resContent.IndexOf("Go back to Instagram") > -1)
                            {
                                ret.Status = -4;
                                ret.Response = "Unable to check";
                                return ret;
                            }
                            else
                            {
                                ret.Status = -4;
                                ret.Response = "Unable to check";
                                return ret;
                            }
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(15));
                            var i = attemp + 1;
                            return await insta.CheckUsername(username, i);
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
                insta.ErrWrite(err, "CheckUsername");
                ret.Status = -1;
                ret.Response = "An unexpected error has occurred";
            }
            return ret;
        }

        public static async Task<IReturn> CheckEmail(this Instagram insta, string email, int attemp = 0)
        {
            IReturn ret = new(0, "Check Email method");
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
                    Dictionary<string, string> sender = new Dictionary<string, string>()
                    {
                        { "email", email },
                        { "username", "" },
                        { "first_name", "" },
                        { "opt_into_one_tap", "false" }
                    };
                    insta.HeaderCheck();
                    HttpRequestMessage req = new(HttpMethod.Post, "https://www.instagram.com/api/v1/web/accounts/web_create_ajax/attempt/") { Content = new FormUrlEncodedContent(sender) };
                    HttpResponseMessage res = await insta.Client.SendAsync(req);
                    string resContent = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        if (resContent.Contains("email_is_taken"))
                        {
                            ret.Response = "Email is taken";
                            ret.Status = -3;
                        }
                        else
                        {
                            ret.Response = "Email is available";
                            ret.Status = 1;
                        }
                    }
                    else
                    {
                        if (attemp >= 3)
                        {
                            if (resContent.IndexOf("Go back to Instagram") > -1)
                            {
                                ret.Status = -4;
                                ret.Response = "Unable to check";
                                return ret;
                            }
                            else
                            {
                                ret.Status = -4;
                                ret.Response = "Unable to check";
                                return ret;
                            }
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(15));
                            var i = attemp + 1;
                            return await insta.CheckEmail(email, i);
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
                insta.ErrWrite(err, "CheckEmail");
                ret.Status = -1;
                ret.Response = "An unexpected error has occurred";
            }
            return ret;
        }

        public static async Task<IReturn> UsernameSuggestion(this Instagram insta, string name, int attemp = 0)
        {
            IReturn ret = new(0, "Username Suggestion method");
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
                    Dictionary<string, string> sender = new Dictionary<string, string>()
                    {
                        { "email", "" },
                        { "username", "" },
                        { "first_name", name },
                        { "opt_into_one_tap", "false" }
                    };
                    insta.HeaderCheck();
                    HttpRequestMessage req = new(HttpMethod.Post, "https://www.instagram.com/api/v1/web/accounts/web_create_ajax/attempt/") { Content = new FormUrlEncodedContent(sender) };
                    HttpResponseMessage res = await insta.Client.SendAsync(req);
                    string resContent = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        dynamic json = JsonConvert.DeserializeObject(resContent);
                        ret.Status = 1;
                        ret.Response = json.username_suggestions[0];
                    }
                    else
                    {
                        if (attemp >= 3)
                        {
                            if (resContent.IndexOf("Go back to Instagram") > -1)
                            {
                                ret.Status = -4;
                                ret.Response = "Unable to get username";
                                return ret;
                            }
                            else
                            {
                                ret.Status = -4;
                                ret.Response = "Unable to get username";
                                return ret;
                            }
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(15));
                            var i = attemp + 1;
                            return await insta.UsernameSuggestion(name, i);
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
                insta.ErrWrite(err, "UsernameSuggestion");
                ret.Status = -1;
                ret.Response = "An unexpected error has occurred";
            }
            return ret;
        }
    }
}
