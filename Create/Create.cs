using InstaAPI.Header;
using InstaAPI.Helpers.Preloads;
using InstaAPI.Helpers.WebHelper;
using InstaAPI.Model.Return;
using InstaAPI.Model.Create;
using InstaAPI.Create.Check;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InstaAPI.Helpers.Errors;

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
        ///  1 = Username available
        /// -1 = Unexpected error
        /// -2 = Unable to load instagram
        /// -3 = Email is Taken
        /// -4 = Username is Taken
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
                var checkUsername = await insta.CheckUsername(account.Username);
                if (checkUsername.Status == 1)
                {
                    var checkEmail = await insta.CheckEmail(account.Email);
                    if (checkEmail.Status == 1)
                    {

                    } else
                    {
                        ret.Status = -3;
                        ret.Response = "Email is taken";
                        return ret;
                    }
                }
                else
                {
                    ret.Status = -4;
                    ret.Response = "Username is taken";
                    return ret;
                }
            }
            catch (Exception err)
            {
                ret.Err = err;
                insta.ErrWrite(err, "CreateAccount");
                ret.Status = -1;
                ret.Response = "An unexpected error has occurred";
            }
            return ret;
        }
    }
}
