using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Challenger.CSharp
{


    public class Challenger
    {
        private readonly string domain;
        private string ownerId;
        private string clientId;
        private string key;
        private bool useHTTPS = false;
        private IDictionary<string, string> @params = new Dictionary<string, string>();

        /// <summary>
        /// Construct challenger instance
        /// </summary>
        /// <param name="domain"> chalenger domain without protocol ex. yourdomain.challengerplatform.com </param>
        public Challenger(string domain)
        {
            this.domain = domain;
        }

        public virtual string OwnerId
        {
            set
            {
                this.ownerId = value;
            }
        }

        public virtual string ClientId
        {
            set
            {
                this.clientId = value;
            }
        }

        public virtual string Key
        {
            set
            {
                this.key = value;
            }
        }

        public virtual bool UseHTTPS
        {
            set
            {
                this.useHTTPS = value;
            }
        }

        public virtual void addParam(string param, string value)
        {
            @params[param] = value;
        }

        public virtual string WidgetScript
        {
            get
            {
                assertParameters();
                return widgetScript();
            }
        }

        public virtual string getWidgetHtml()
        {
            assertParameters();
            return widgetHtml();
        }

        public string getWidgetUrl()
        {
            assertParameters();
            return protocol() + domain + "/widget?data=" + urlencode(encryptedWidgetData());
        }

        public virtual string EncryptedData
        {
            get
            {
                assertParameters();
                return encryptedWidgetData();
            }
        }

        public virtual bool trackEvent(string @event)
        {
            assertParameters();
            string result = getResponseAsync(trackEventUrl(@event)).GetAwaiter().GetResult();
            return "ok".Equals(result, StringComparison.OrdinalIgnoreCase);
        }

        public virtual bool deleteClient()
        {
            assertParameters();
            string result = getResponseAsync(ClientDeletionUrl).GetAwaiter().GetResult();
            return "ok".Equals(result, StringComparison.OrdinalIgnoreCase);
        }

        public virtual string ClientDeletionUrl
        {
            get
            {

                return protocol() + domain + "/api/v1/deleteClient?data=" + urlencode(encryptedClientDeletionData());
            }
        }

        private string encryptedClientDeletionData()
        {
            StringBuilder json = new StringBuilder("{");
            json.Append(jsonString("client_id", clientId));
            return encryptWithAES(completeJson(json));
        }

        private void assertParameters()
        {
            if (string.ReferenceEquals(clientId, null))
            {
                throw new System.ArgumentException("clientId is not set");
            }
            if (string.ReferenceEquals(key, null))
            {
                throw new System.ArgumentException("key is not set");
            }
        }

        private string trackEventUrl(string @event)
        {
            string ownerIdParameter = !string.ReferenceEquals(this.ownerId, null) && !this.ownerId.Equals("") ? "owner_id=" + urlencode(this.ownerId) + "&" : "";

            return protocol() + domain + "/api/v1/trackEvent?" + ownerIdParameter + "data=" + urlencode(encryptedEventData(@event, this.ownerId));
        }

        private string protocol()
        {
            return (useHTTPS ? "https" : "http") + "://";
        }

        private string encryptedWidgetData()
        {
            return encryptWithAES(buildJson(null, null));
        }

        private string encryptedEventData(string @event, string ownerId)
        {
            return encryptWithAES(buildJson(@event, ownerId));
        }

        private string buildJson(string @event, string ownerId)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.Append(jsonString("client_id", getClientIdHash(clientId, ownerId)));
            json.Append(jsonLiteral("params", paramsJson()));
            if (!string.ReferenceEquals(@event, null))
            {
                json.Append(jsonString("event", @event));
            }
            return completeJson(json);
        }

        private static string getClientIdHash(string clientId, string ownerId)
        {
            if (string.ReferenceEquals(ownerId, null) || ownerId.Equals(""))
            {
                return clientId;
            }
            if (string.ReferenceEquals(clientId, null))
            {
                return null;
            }
            return getMd5(ownerId + ":" + clientId);
        }

        private static string getMd5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private string jsonString(string key, string value)
        {
            return string.Format("\"{0}\": \"{1}\",", key, value);
        }

        private string jsonLiteral(string key, string value)
        {
            return string.Format("\"{0}\": {1},", key, value);
        }

        private string completeJson(StringBuilder json)
        {
            if (json[json.Length - 1] == ',')
            {
                return json.ToString().Substring(0, json.Length - 1) + "}";
            }
            else
            {
                return json.Append("}").ToString();
            }
        }

        private string paramsJson()
        {
            StringBuilder paramsJson = new StringBuilder("{");
            foreach (string key in @params.Keys)
            {
                paramsJson.Append(jsonString(key, @params[key]));
            }
            return completeJson(paramsJson);
        }

        private string urlencode(string data)
        {
            string encoded = Uri.EscapeDataString(data);
            return encoded;
        }

        private string encryptWithAES(string data)
        {
            byte[] encrypted;
            string IvString;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Encoding.ASCII.GetBytes(key);
                rijAlg.GenerateIV();
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.Mode = CipherMode.CBC;
                IvString = Convert.ToBase64String(rijAlg.IV);
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {

                    swEncrypt.Write(data);
                    if (data.Length < 16)
                    {
                        for (int i = data.Length; i < 16; i++)
                            swEncrypt.Write((byte)0x0);
                    }
                    swEncrypt.Flush();
                    csEncrypt.FlushFinalBlock();
                    encrypted = msEncrypt.ToArray();
                }
            }
            return Convert.ToBase64String(encrypted).Replace("(?:\\r\\n|\\n\\r|\\n|\\r)", "") + ":" + IvString;
        }

        private async System.Threading.Tasks.Task<string> getResponseAsync(string urlToRead)
        {

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(urlToRead);
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }

        private string widgetScript()
        {
            return string.Format("_chw = typeof _chw == \"undefined\" ? {{}} : _chw;\n" + "\t_chw.type = \"iframe\";\n" + "\t_chw.domain = \"{0}\";\n" + "\t_chw.data = \"{1}\";\n" + "\t(function() {{\n" + "\tvar ch = document.createElement(\"script\"); ch.type = \"text/javascript\"; ch.async = true;\n" + "\tch.src = \"//{2}/v1/widget/script.js\";\n" + "\tvar s = document.getElementsByTagName(\"script\")[0]; s.parentNode.insertBefore(ch, s);\n" + "\t}})();\n", domain, encryptedWidgetData(), domain);
        }

        private string widgetHtml()
        {
            return string.Format("<div id=\"_chWidget\"></div>\n" + "<script type=\"text/javascript\">\n" + "\t<!--\n" + "\t{0}\n" + "\t//-->\n" + "</script>\n", widgetScript());
        }
    }

}
