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
        /// 1 =
        /// -1 =
        /// -2 =
        /// -3 =
        /// -4 =
        /// -5 =
        /// -6 =
        /// -7 =
        /// -8 =
        /// 0 = Default - Error if send it
        /// </returns>
        public static async Task<IReturn> CheckUsername(this Instagram insta, string username, int attemp = 0)
        {
            IReturn ret = new(0, "Check Username method");
            try
            {

            }
            catch (Exception err)
            {

            }
            return ret;
        }

        public static async Task<IReturn> CheckEmail(this Instagram insta, string email, int attemp = 0)
        {
            IReturn ret = new(0, "Check Email method");
            try
            {

            }
            catch (Exception err)
            {

            }
            return ret;
        }

        public static async Task<IReturn> UsernameSuggestion(this Instagram insta, string name, int attemp = 0)
        {
            IReturn ret = new(0, "Username Suggestion method");
            try
            {

            }
            catch (Exception err)
            {

            }
            return ret;
        }
    }
}
