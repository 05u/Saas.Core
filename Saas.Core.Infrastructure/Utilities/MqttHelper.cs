using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Saas.Core.Infrastructure.Utilities
{
    public class MqttHelper
    {

        public static string address { get; set; }
        public static string username { get; set; }
        public static string password { get; set; }

        /// <summary>
        /// 发送MQTT消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool PublishMqtt(string msg, string topic)
        {
            //string clientId = Guid.NewGuid().ToString();
            string clientId = Dns.GetHostName();
            MqttClient client;
            try
            {
                // create client instance 
                client = new MqttClient(IPAddress.Parse(address));
                client.Connect(clientId, username, password);
                //发送消息
                client.Publish(topic, Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

        }
    }
}
