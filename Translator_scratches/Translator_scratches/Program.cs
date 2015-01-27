using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Translator_scratches
{
    class Program
    {
        static void Main(string[] args)
        {

            sample_translatorservice_usage();

        }

        private static string getAccessToken()
        {
            //1.
            
            string clientID = "NotiQ1";
            string clientSecret = "lrOB6czlivrqK91fc2XmeRgDTrqf7mwO/NogL6OtNro=";

            String strTranslatorAccessURI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
            String strRequestDetails = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientID), HttpUtility.UrlEncode(clientSecret));

            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(strTranslatorAccessURI);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strRequestDetails);
            webRequest.ContentLength = bytes.Length;
            using (System.IO.Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            System.Net.WebResponse webResponse = webRequest.GetResponse();

            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(AdmAccessToken));
            //Get deserialized object from JSON stream 
            AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());

            //string headerValue = "Bearer " + token.access_token;

            return token.access_token;
        }

        private static void sample_translatorservice_usage()
        {
            TranslatorService.LanguageServiceClient client = new TranslatorService.LanguageServiceClient();

            HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
            string headerValue = "Bearer " + getAccessToken();
            httpRequestProperty.Headers.Add("Authorization", headerValue);


            // Creates a block within which an OperationContext object is in scope.
            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                string sourceText = "Use pixels to express measurements for padding and margins.";
                string translationResult;
                //Keep appId parameter blank as we are sending access token in authorization header.
                translationResult = client.Translate("", sourceText, "en", "de", "text/plain", "");
                Console.WriteLine("Translation for source {0} from {1} to {2} is", sourceText, "en", "de");
                Console.WriteLine(translationResult);
            }
        }

        /// <summary>
        /// http://blogs.msdn.com/b/translation/p/gettingstarted2.aspx
        /// </summary>
        private static void sample_usage()
        {
            //1.

            string clientID = "NotiQ1";
            string clientSecret = "lrOB6czlivrqK91fc2XmeRgDTrqf7mwO/NogL6OtNro=";

            String strTranslatorAccessURI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
            String strRequestDetails = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientID), HttpUtility.UrlEncode(clientSecret));

            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(strTranslatorAccessURI);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strRequestDetails);
            webRequest.ContentLength = bytes.Length;
            using (System.IO.Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            System.Net.WebResponse webResponse = webRequest.GetResponse();

            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(AdmAccessToken));
            //Get deserialized object from JSON stream 
            AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());

            string headerValue = "Bearer " + token.access_token;


            //2.

            string txtToTranslate = "awesome";
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(txtToTranslate) + "&from=en&to=pl";
            System.Net.WebRequest translationWebRequest = System.Net.WebRequest.Create(uri);
            translationWebRequest.Headers.Add("Authorization", headerValue);
            System.Net.WebResponse response = translationWebRequest.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            System.IO.StreamReader translatedStream = new System.IO.StreamReader(stream, encode);
            string responseStr = translatedStream.ReadToEnd();
            System.Xml.XmlDocument xTranslation = new System.Xml.XmlDocument();
            xTranslation.LoadXml(responseStr);
            string translatedText = xTranslation.InnerText;
        }
    }
}
