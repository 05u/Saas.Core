using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// 注入配置
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 日志记录
        /// </summary>
        private readonly ILogger<EmailService> _logger;
        /// <summary>
        /// 发送方
        /// </summary>
        private MailboxAddress fromMail;
        /// <summary>
        /// Smpt邮件服务器
        /// </summary>
        private string smtpServer;
        /// <summary>
        /// Smpt端口
        /// </summary>
        private int smtpPort = 0;
        /// <summary>
        /// 密码
        /// </summary>
        private string password;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger)
        {
            this.Configuration = configuration;
            this._logger = logger;

            fromMail = new MailboxAddress(Encoding.UTF8, Configuration["EmailServer:FromMailName"], Configuration["EmailServer:FromMail"]);
            smtpServer = Configuration["EmailServer:SmtpServer"];
            if (int.TryParse(Configuration["EmailServer:SmtpPort"], out int result))
                smtpPort = result;
            password = Configuration["EmailServer:FromPassword"];
        }
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="sendTo">收件方</param>
        /// <param name="subject">主题</param>
        /// <param name="mimePart">正文</param>
        /// <returns></returns>
        public bool Send(
            List<MailboxAddress> sendTo,
            string subject,
            MimePart mimePart)
        {
            var message = new MimeMessage();
            message.From.Add(fromMail);
            message.To.AddRange(sendTo);
            message.Subject = subject;
            message.Body = mimePart;

            return Send(message, fromMail.Address, password, smtpServer, smtpPort);

        }
        /// <summary>
        /// 邮件发送异步
        /// </summary>
        /// <param name="sendTo">收件方</param>
        /// <param name="subject">主题</param>
        /// <param name="mimePart">正文</param>
        /// <returns></returns>
        public async Task<bool> SendAsync(
            List<MailboxAddress> sendTo,
            string subject,
            MimePart mimePart)
        {
            var message = new MimeMessage();
            message.From.Add(fromMail);
            message.To.AddRange(sendTo);
            message.Subject = subject;
            message.Body = mimePart;
            return await SendAsync(message, fromMail.Address, password, smtpServer, smtpPort);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发送方</param>
        /// <param name="to">收件方</param>
        /// <param name="password">密码</param>
        /// <param name="subject">标题</param>
        /// <param name="mimePart">正文</param>
        /// <param name="SmtpServer">Smpt服务器</param>
        /// <param name="SmtpPort">Smtp端口</param>
        /// <returns></returns>
        public bool Send(
            MailboxAddress from,
            List<MailboxAddress> to,
            string password,
            string subject,
            MimePart mimePart,
            string SmtpServer,
            int SmtpPort = 0)
        {
            var message = new MimeMessage();
            message.From.Add(from);
            message.To.AddRange(to);
            message.Subject = subject;
            message.Body = mimePart;

            return Send(message, from.Address, password, SmtpServer, SmtpPort);
        }


        /// <summary>
        /// 邮件发送异步
        /// </summary>
        /// <param name="from">发件方</param>
        /// <param name="to">收件方</param>
        /// <param name="password">密码</param>
        /// <param name="subject">主题</param>
        /// <param name="mimePart">正文</param>
        /// <param name="SmtpServer">Smtp邮件服务器</param>
        /// <param name="SmtpPort">Smtp邮件端口</param>
        /// <returns></returns>
        public async Task<bool> SendAsync(
            MailboxAddress from,
            List<MailboxAddress> to,
            string password,
            string subject,
            MimePart mimePart,
            string SmtpServer,
            int SmtpPort = 0)
        {
            var message = new MimeMessage();
            message.From.Add(from);
            message.To.AddRange(to);
            message.Subject = subject;
            message.Body = mimePart;

            return await SendAsync(message, from.Address, password, SmtpServer, SmtpPort);
        }


        private bool Send(MimeMessage message, string username, string password, string smtpServer, int smtpPort = 0)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(smtpServer, smtpPort, true);
                }
                catch
                {
                    throw new BusinessException("连接Smtp服务器失败，请检查配置！");
                }
                try
                {
                    client.Authenticate(username, password);
                }
                catch
                {
                    throw new BusinessException("登录发送账号失败，请检查账户与密码！");
                }
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    throw new BusinessException("发送邮件时出现异常" + ex.Message);
                }

                client.Disconnect(true);
            }
            return true;
        }


        private async Task<bool> SendAsync(MimeMessage message, string username, string password, string smtpServer, int smtpPort = 0)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(smtpServer, smtpPort, true);
                }
                catch
                {
                    throw new BusinessException("连接Smtp服务器失败，请检查配置！");
                }
                try
                {
                    await client.AuthenticateAsync(username, password);
                }
                catch
                {
                    throw new BusinessException("登录发送账号失败，请检查账户与密码！");
                }
                try
                {
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    throw new BusinessException("发送邮件时出现异常" + ex.Message);
                }

                await client.DisconnectAsync(true);
            }
            return true;
        }
    }
}
