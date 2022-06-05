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
using System.Threading.Tasks;

namespace InstaAPI.Profile.Login
{
    public static class LoginClass
    {
        /// <summary>
        /// Login to insta account
        /// </summary>
        /// <param name="insta">InstaAPI instace</param>
        /// <returns>
        /// 1 = Sucess
        /// -1 = Err
        /// -2 = Not load
        /// -3 = Incorrect user
        /// -4 = Unable to login
        /// -5 = Challenge
        /// -6 = Feedback
        /// -7 = disabled
        /// -8 = IP
        /// 0 = Default - Error if send it
        /// </returns>
        public static async Task<IReturn> Login(this Instagram insta, int attemp = 0)
        {
            IReturn ret = new(0, "Login method");
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
                        { "username", insta.User.Username },
                        { "enc_password", insta.User.Password }
                    };
                    insta.HeaderLogin();
                    HttpRequestMessage req = new(HttpMethod.Post, "https://instagram.com/accounts/login/ajax/") { Content = new FormUrlEncodedContent(sender) };
                    HttpResponseMessage res = await insta.Client.SendAsync(req);
                    string resContent = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        if (resContent.Contains("\"authenticated\":false"))
                        {
                            ret.Status = -3;
                            ret.Response = "Incorrect user or password";
                            return ret;
                        }
                        IEnumerable<string> headerValues = res.Headers.GetValues("x-ig-set-www-claim");
                        insta.SetClaim(headerValues.FirstOrDefault());
                        insta.ClearHeader();
                        if (resContent.Contains("\"authenticated\":true"))
                        {
                            ret.Status = 1;
                            ret.Response = "Success";
                            string[] aux3 = resContent.Split('"');
                            insta.UserID = long.Parse(aux3[5]);
                            insta.ClearHeader();
                            insta.HeaderDefault();
                            req = new(HttpMethod.Get, "https://instagram.com/");
                            HttpResponseMessage firstLoad = await insta.Client.SendAsync(req);
                            if (firstLoad.IsSuccessStatusCode)
                            {
                                if (WebHelperClass.CanReadJson(content))
                                {
                                    insta._sharedData = JsonConvert.DeserializeObject(WebHelperClass.GetJson(content));
                                    string ajax = !String.IsNullOrEmpty(insta._sharedData.rollout_hash.ToString()) ? insta._sharedData.rollout_hash.ToString() : "1";
                                    if (ajax == "1")
                                        insta.SetAjax(WebHelperClass.GetAjax(content));
                                    else
                                        insta.SetAjax(ajax);
                                }
                            }
                            return ret;
                        }
                        ret.Status = -4;
                        ret.Response = "Unable to login";
                    }
                    else
                    {
                        if (resContent.IndexOf("\"authenticated\":false") > -1)
                        {
                            ret.Status = -3;
                            ret.Response = "incorrect user or password";
                            return ret;
                        }
                        if (resContent.IndexOf("permanently disabled") > -1)
                        {
                            ret.Status = -7;
                            ret.Response = "Account disabled";
                            return ret;
                        }
                        if (resContent.IndexOf("Your account has been disabled") > -1)
                        {
                            ret.Status = -7;
                            ret.Response = "Account disabled";
                            return ret;
                        }
                        if (resContent.IndexOf("feedback_required") > -1)
                        {
                            ret.Status = -6;
                            ret.Response = "Account with temporary lock";
                            return ret;
                        }
                        if (resContent.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            var serializado = resContent;
                            dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                            insta.Challenge_URL = dataJson.checkpoint_url;
                            ret.Status = -5;
                            ret.Response = "Account with lock";
                            return ret;
                        }
                        else
                        {
                            if (attemp >= 3)
                            {
                                if (resContent.IndexOf("Go back to Instagram") > -1)
                                {
                                    ret.Status = -4;
                                    ret.Response = "Unable to login";
                                    return ret;
                                }
                                else
                                {
                                    ret.Status = -4;
                                    ret.Response = "Unable to login";
                                    return ret;
                                }
                            }
                            else
                            {
                                await Task.Delay(TimeSpan.FromSeconds(15));
                                var i = attemp + 1;
                                return await insta.Login(i);
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
                insta.ErrWrite(err, "Login");
                ret.Status = -1;
                ret.Response = "An unexpected error has occurred";
            }
            return ret;
        }
    }
}
