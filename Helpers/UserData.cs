using Newtonsoft.Json;

namespace InstaAPI.Helpers.User
{
    class UserData
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("enc_password")]
        public string Password { get; set; }
        public UserData (string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
