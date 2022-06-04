using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Helpers.Ip
{
    public static class IpHelper
    {
        public static async Task<string> GetMyIp(this Instagram insta)
        {
            HttpRequestMessage req = new(HttpMethod.Get, "http://meuip.com/api/meuip.php");
            HttpResponseMessage res = await insta.Client.SendAsync(req);
            if (res.IsSuccessStatusCode)
                return await res.Content.ReadAsStringAsync();
            else
                return "requisition err";
        }
    }
}
