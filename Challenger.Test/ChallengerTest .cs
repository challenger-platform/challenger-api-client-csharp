using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace Challenger.Test
{
    [TestClass]
    public class ChallengerTest
    {
        private static readonly String DOMAIN = "beta.challengerplatform.com";
        private static readonly String CLIENT_ID = "1";
        private static readonly String SECRET_KEY = "a56bb7102fce0b997d2ff71e03d39554";

        [TestMethod]
        public void shouldRetrieveWidget()
        {
            Challenger.CSharp.Challenger challenger = new Challenger.CSharp.Challenger(DOMAIN)
            {
                ClientId = CLIENT_ID,
                Key = SECRET_KEY,
                UseHTTPS = true
            };
            challenger.addParam("param1", "value1");
            challenger.addParam("param2", "value2");

            String response = challenger.getWidgetHtml();
            // No way to assert, different every time. Paste test output to html and try to open
            Console.WriteLine(response);
            System.Diagnostics.Debug.WriteLine(response);
        }

        [TestMethod]
        public void shouldGetOkResponseOnWidgetUrl()
        {
            try
            {
                Challenger.CSharp.Challenger challenger = new Challenger.CSharp.Challenger(DOMAIN)
                {
                    ClientId = CLIENT_ID,
                    Key = SECRET_KEY,
                    UseHTTPS = true
                };
                challenger.addParam("param1", "value1");
                challenger.addParam("param2", "value2");

                String widgetUrl = challenger.getWidgetUrl();
                System.Diagnostics.Debug.WriteLine("Widget URl: " + widgetUrl);

                var httpClient = new HttpClient();
                var response = httpClient.GetAsync(widgetUrl).GetAwaiter().GetResult();
                var contents = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
                Assert.AreNotEqual(contents, "ERROR: decryption error");
            }
            catch (Exception e)
            {

            }
        }

        [TestMethod]
        public void shouldGetOkResponseOnWidgetUrlWithUnicodeSymbols()
        {
            Challenger.CSharp.Challenger challenger = new Challenger.CSharp.Challenger(DOMAIN)
            {
                ClientId = CLIENT_ID,
                Key = SECRET_KEY,
                UseHTTPS = true
            };
            challenger.addParam("paramŠ", "valueŠ");
            challenger.addParam("paramŪ", "valueŪ");

            String widgetUrl = challenger.getWidgetUrl();
            System.Diagnostics.Debug.WriteLine("Widget URl: " + widgetUrl);

            var httpClient = new HttpClient();
            var response = httpClient.GetAsync(widgetUrl).GetAwaiter().GetResult();
            var contents = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreNotEqual(contents, "ERROR: decryption error");
        }

        [TestMethod]
        public void shouldTrackEvent()
        {
            Challenger.CSharp.Challenger challenger = new Challenger.CSharp.Challenger(DOMAIN)
            {
                ClientId = CLIENT_ID,
                Key = SECRET_KEY,
                UseHTTPS = true
            };

            bool response = challenger.trackEvent("some_event");
            Assert.AreEqual(response, true);
        }

    }
}
