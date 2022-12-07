using InstaAPI.Header;
using InstaAPI.Helpers.Preloads;
using InstaAPI.Helpers.WebHelper;
using InstaAPI.Model.Return;
using InstaAPI.Model.Create;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Create
{
    public static class CreateClass
    {
        /// <summary>
        /// Create instagram account
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
        public static async Task<IReturn> CreateAccount (this Instagram insta, ICreate account, int attemp = 0)
        {
            IReturn ret = new(0, "Create method");
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
