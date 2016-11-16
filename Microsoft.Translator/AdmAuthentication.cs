using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.Translator.Contract;
using System.Runtime.Serialization.Json;

namespace Microsoft.Translator
{
    public class AdmAuthentication
    {
        public static readonly string DatamarketAccessUri = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
        private string request;
        private AdmAccessToken token;
        private Timer accessTokenRenewer;
        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;
        private string appKey = "";
        public AdmAuthentication(string appKey)
        {
            this.appKey = appKey;
            this.token = HttpPost(DatamarketAccessUri, appKey);
            //renew the token every specfied minutes
            accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }
        public AdmAccessToken GetAccessToken()
        {
            return this.token;
        }
        private void RenewAccessToken()
        {
            AdmAccessToken newAccessToken = HttpPost(DatamarketAccessUri, this.appKey);
            this.token = newAccessToken;
        }
        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed renewing access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
        }

        private AdmAccessToken HttpPost(string DatamarketAccessUri, string appKey)
        {
            //Prepare OAuth request 
            var webRequest = (HttpWebRequest)WebRequest.Create(DatamarketAccessUri);
            webRequest.ContentType = "application/json";
            webRequest.Headers.Add("Ocp-Apim-Subscription-Key", appKey);
            webRequest.ContentLength = 0;
            webRequest.Accept = "application/json";
            webRequest.Method = "POST";
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                /*
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                */
                var admAccessToken = new AdmAccessToken();
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                string strResponse = reader.ReadToEnd();
                admAccessToken.access_token = strResponse;
                return admAccessToken;
            }
        }
    }
}
