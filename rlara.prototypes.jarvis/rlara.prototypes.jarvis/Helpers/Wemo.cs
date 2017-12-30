using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace rlara.prototypes.jarvis.Helpers
{
    public static class Wemo
    {

        public static async void Toggle(int port)
        {
            string uri = $"http://192.168.1.{port}:49153/upnp/control/basicevent1";

            string soapAction = "\"urn:Belkin:service:basicevent:1#GetBinaryState\"";

            string requestBody =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n <s:Body>\n  <u:GetBinaryState xmlns:u=\"urn:Belkin:service:basicevent:1\">\n   \n  </u:GetBinaryState>\n </s:Body>\n</s:Envelope>";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("SOAPAction", soapAction);
                var content = new StringContent(requestBody, Encoding.UTF8, "text/xml");
                using (var response = await client.PostAsync(uri, content))
                {
                    Stream receiveStream = await response.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    var text = readStream.ReadToEnd();

                    if (text.Contains("<BinaryState>1</BinaryState>"))
                    {
                        Off(port);
                    }
                    else
                    {
                        On(port);
                    }

                }
            }

        }

        public static async void Off(int port)
        {
            string uri = $"http://192.168.1.{port}:49153/upnp/control/basicevent1";

            string soapAction = "\"urn:Belkin:service:basicevent:1#SetBinaryState\"";

            string requestBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n <s:Body>\n  <u:SetBinaryState xmlns:u=\"urn:Belkin:service:basicevent:1\">\n   <BinaryState>0</BinaryState>\n  </u:SetBinaryState>\n </s:Body>\n</s:Envelope>";


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("SOAPAction", soapAction);
                var content = new StringContent(requestBody, Encoding.UTF8, "text/xml");
                using (var response = await client.PostAsync(uri, content))
                {

                }
            }

        }

        public static async void On(int port)
        {
            string uri = $"http://192.168.1.{port}:49153/upnp/control/basicevent1";

            string soapAction = "\"urn:Belkin:service:basicevent:1#SetBinaryState\"";

            string requestBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n <s:Body>\n  <u:SetBinaryState xmlns:u=\"urn:Belkin:service:basicevent:1\">\n   <BinaryState>1</BinaryState>\n  </u:SetBinaryState>\n </s:Body>\n</s:Envelope>";


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("SOAPAction", soapAction);
                var content = new StringContent(requestBody, Encoding.UTF8, "text/xml");
                using (var response = await client.PostAsync(uri, content))
                {

                }
            }

        }

    }
}
