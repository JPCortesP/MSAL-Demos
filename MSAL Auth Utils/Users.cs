using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAL_Auth_Utils
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public  class UsersReponse 
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<GraphUser> value { get; set; }
    }

    public class GraphUser
    {
        public List<object>? businessPhones { get; set; }
        public string? displayName { get; set; }
        public string? givenName { get; set; }
        public object? jobTitle { get; set; }
        public string? mail { get; set; }
        public object? mobilePhone { get; set; }
        public object? officeLocation { get; set; }
        public object? preferredLanguage { get; set; }
        public string? surname { get; set; }
        public string? userPrincipalName { get; set; }
        public string? id { get; set; }
    }


}
