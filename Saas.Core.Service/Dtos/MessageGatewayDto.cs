using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 通知消息到消息网关(使用pushplus相同入参)
    /// </summary>
    public class PublishNoticeMessagePostInput
    {
        /// <summary>
        /// pushplus token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public Content content { get; set; }

        /// <summary>
        /// 消息模板
        /// </summary>
        public string template { get; set; }

        /// <summary>
        /// pushplus 群组编码
        /// </summary>
        public string topic { get; set; }
    }


    public class PushPlusInput
    {
        /// <summary>
        /// pushplus token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 消息模板
        /// </summary>
        public string template { get; set; }

        /// <summary>
        /// pushplus 群组编码
        /// </summary>
        public string topic { get; set; }


    }

    /// <summary>
    /// Mirai认证
    /// </summary>
    public class MiraiVerify
    {
        /// <summary>
        /// 认证key
        /// </summary>
        public string verifyKey { get; set; } = "ORooTW2leGI03jk8";

    }

    /// <summary>
    /// Mirai认证响应
    /// </summary>
    public class MiraiVerifyResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string session { get; set; }

    }


    /// <summary>
    /// Mirai绑定和释放
    /// </summary>
    public class MiraiBindAndRelease
    {
        /// <summary>
        /// session
        /// </summary>
        public string sessionKey { get; set; }

        /// <summary>
        /// 要绑定的Bot的qq号
        /// </summary>
        public string qq { get; set; }

    }


    /// <summary>
    /// Mirai发送消息
    /// </summary>
    public class MiraisSendFriendMessage
    {
        /// <summary>
        /// 已经激活的Session
        /// </summary>
        public string sessionKey { get; set; }

        /// <summary>
        /// 目标好友的QQ号
        /// </summary>
        public string target { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        public string qq { get; set; }

        /// <summary>
        /// 消息链
        /// </summary>
        public List<MessageChain> messageChain { get; set; }

    }

    /// <summary>
    /// Mirai发送消息响应
    /// </summary>
    public class MiraisSendFriendMessageResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int? code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string messageId { get; set; }


    }


    public class MessageChain
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; } = "Plain";

        /// <summary>
        /// 
        /// </summary>
        public string text { get; set; }
    }



    /// <summary>
    /// 消息体
    /// </summary>
    public class Content
    {
        public string 电脑 { get; set; }

        public string IP { get; set; }
    }

    /// <summary>
    /// 小爱消息体
    /// </summary>
    public class SendXiaoaiMsg
    {
        ///// <summary>
        ///// 小米平台设备Id
        ///// </summary>
        //public string deviceID { get; set; }

        /// <summary>
        /// 小爱名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string text { get; set; }
    }

    /// <summary>
    /// 发送通知消息到指定通道入参
    /// </summary>
    public class PublishNoticeMessageToInput
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// QQ通道 发送机器人QQ(可选)
        /// </summary>
        public string Qq_Sender { get; set; }

        /// <summary>
        /// QQ通道 接收者QQ
        /// </summary>
        public string Qq_Receiver { get; set; }

        /// <summary>
        /// 微信通道 接收用户Id
        /// </summary>
        public string Wx_Wxid { get; set; }

        /// <summary>
        /// 微信通道 接收群Id(可选)
        /// </summary>
        public string Wx_Roomid { get; set; }

        /// <summary>
        /// 微信通道 接收人群内昵称(可选)
        /// </summary>
        public string Wx_Nickname { get; set; }

        /// <summary>
        /// 是否保存消息记录(默认保存)
        /// </summary>
        public bool IsSaveRecord { get; set; } = true;
    }


    /// <summary>
    /// QQ消息体
    /// </summary>
    public class SendQQMsg
    {
        /// <summary>
        /// 接收者QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 发送机器人QQ
        /// </summary>
        public string SendQQ { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string text { get; set; }
    }

    /// <summary>
    /// 微信http通用请求入参
    /// </summary>
    public class WeixinApiDto
    {
        public WeixinPara para { get; set; }
    }

    public class WeixinPara
    {
        public string id { get; set; }
        public int type { get; set; }
        public string roomid { get; set; }
        public string wxid { get; set; }
        public string content { get; set; }
        public string nickname { get; set; }
        public string ext { get; set; }
    }

    /// <summary>
    /// 获取微信机器人通用出参
    /// </summary>
    public class WeixinResult<T>
    {
        public T content { get; set; }

        public string id { get; set; }

        public int type { set; get; }
        public string receiver { get; set; }

        public string sender { get; set; }

        public int? srvid { get; set; }

        public string status { get; set; }

        public string time { get; set; }

    }

    /// <summary>
    /// webSocket微信接收消息
    /// </summary>
    public class ReceiveWeixinDto
    {
        public string content { set; get; }
        public string id { set; get; }
        public string id1 { set; get; }
        public string id2 { set; get; }
        public string id3 { set; get; }
        public string receiver { set; get; }
        public string sender { set; get; }
        public int srvid { set; get; }
        public string status { set; get; }
        public string time { set; get; }
        public int type { set; get; }
        public string wxid { set; get; }

    }

    /// <summary>
    /// 好友和群列表
    /// </summary>
    public class ContactListContent
    {
        //public string authPassword { set; get; }
        public string headimg { get; set; }
        public string name { get; set; }
        public int node { get; set; }
        public string remarks { get; set; }
        public string wxcode { get; set; }
        public string wxid { get; set; }

    }

    /// <summary>
    /// 群成员昵称
    /// </summary>
    public class MemberNick
    {
        public string nick { set; get; }
        public string roomid { set; get; }
        public string wxid { set; get; }

    }

    /// <summary>
    /// 获取插件的信息返回消息体 用于QQ机器人心跳检测
    /// </summary>
    public class MiraiAbout
    {
        public int Code { set; get; }

        public string Msg { set; get; }

        public dynamic Data { set; get; }
    }
}
