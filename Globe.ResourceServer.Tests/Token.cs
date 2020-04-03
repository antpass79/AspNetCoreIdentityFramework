using Newtonsoft.Json;
using System;

namespace Globe.ResourceServer.Tests
{
    public class Token
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("auth_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
