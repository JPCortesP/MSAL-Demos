namespace MSAL_Auth_Utils
{
    public class Secrets
    {
       
        public Secrets() { }    
        public Secrets(bool empty)
        {
            if (empty)
            {
                ClientID = "#######Your Client ID Here#######";
                TenantID = "#######Your Tenant ID Here#######";
            }
        }
        public string ClientID { get; set; }
        public string TenantID { get; set;}
    }
}
