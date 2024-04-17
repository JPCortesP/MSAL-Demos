namespace MSAL_Auth_Utils
{
    /// <summary>
    /// Simple class that invokes an URL with a specific token. 
    /// </summary>
    public class GraphClientLite : IDisposable
    {
        private string token {  get; set; }
        public GraphClientLite(string token)
        {
            this.token = token;
        }
        /// <summary>
        /// Calls the URL with the token specified. Returns strings
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> InvokeGraph(string url)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void Dispose()
        {
            this.token = "";
        }
    }
}
