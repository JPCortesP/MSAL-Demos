using Microsoft.Identity.Client;
using MSAL_Auth_Utils;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace MSAL_Demos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private jpIdentityClient? IdentityClient { get; set; }
        private AuthenticationResult result;
        public MainWindow()
        {

            InitializeComponent();
            this.result = null;
            var secrets = GetSecrets();
            if (secrets == null)
            {
                this.Close();   
            }
            this.IdentityClient = new jpIdentityClient(isWindows: true, secrets: secrets);
        }
        /// <summary>
        /// Simple method that stores secrets on C: drive. Just so I don't check them in into github
        /// </summary>
        /// <returns></returns>
        private Secrets GetSecrets()
        {
            
            string path = "C:/SecretsStorage/";
            string filename = "secrets.json";
            try
            {
               
                var texto = File.ReadAllText(path+filename);
                var secrets = JsonConvert.DeserializeObject<Secrets>(texto);
                return secrets;
            }
            
            catch (Exception ex)
            {
                Directory.CreateDirectory(path);
                MessageBox.Show("File with secrets not Found. Go to Path " + path +filename+ " and fill your data");
                var secrets = new Secrets(empty: true);
                var texto = JsonConvert.SerializeObject(secrets);

                File.WriteAllText(path+filename, texto);
                return null;
            }
        }

        /// <summary>
        /// Ref.: https://learn.microsoft.com/en-us/entra/msal/dotnet/acquiring-tokens/desktop-mobile/acquiring-tokens-interactively
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.IdentityClient != null)
            {
                var app = IdentityClient.app;
                
                var accounts = await app.GetAccountsAsync();
               
                try
                {
                    //TODO: Set Cache. I want to always ask for creds, so, its fine for now. 
                    result = await app.AcquireTokenSilent(IdentityClient.ScopesReadOnly, accounts.FirstOrDefault())
                    .ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    result = await app.AcquireTokenInteractive(IdentityClient.ScopesReadOnly).ExecuteAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            if(result != null) 
            { 
                this.btn_signin.IsEnabled = false;
                this.lbl_AuthAs.Text = "Succesfully Authenticated as: " + result.Account.Username;
                using (var client= new GraphClientLite(result.AccessToken))
                {
                    var usersText = await client.InvokeGraph("https://graph.microsoft.com/v1.0/users/");
                    UsersReponse usersResponse = JsonConvert.DeserializeObject<UsersReponse>(usersText);
                    lstv_Users.DataContext = usersResponse.value;

                    //https://graph.microsoft.com/v1.0/devices
                    var devicesText = await client.InvokeGraph("https://graph.microsoft.com/v1.0/devices");
                    DeviceResponse devices = JsonConvert.DeserializeObject<DeviceResponse>(devicesText);
                    lstv_Devices.DataContext = devices.value;

                }

            }

        }

      
    }
}