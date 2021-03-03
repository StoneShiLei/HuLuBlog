using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Message;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;

namespace Com.Stone.HuLuBlog.EmailService
{
    class EmailService : IConsumeAsync<EmailMessage>
    {
        static LogOperation logger = new LogOperation(typeof(EmailService));

        static void Main(string[] args)
        {
            try 
            {
                using (var bus = RabbitHutch.CreateBus("host=localhost"))
                {
                    var subscriber = new AutoSubscriber(bus, "EmailService.Email");
                    subscriber.ConfigureSubscriptionConfiguration = c => c.WithPrefetchCount(50);
                    subscriber.Subscribe(new[] { Assembly.GetExecutingAssembly() });


                    Console.WriteLine("正在监听消息，按下回车退出程序...");
                    Console.ReadLine();
                }
            }
            catch(Exception ex)
            {
                logger.Error("邮件服务异常",ex.Message);
            }
        }

        [AutoSubscriberConsumer(SubscriptionId = "EmailService.Email")]
        public Task ConsumeAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            SendEmail(message);

            string log = string.Format("发件人：{0}，收件人：{1}，主题：{2},发送时间：{3}", message.From, message.To, message.Subject, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(log);
            Console.ResetColor();

            logger.Debug(log);

            return Task.CompletedTask;
        }

        public static void SendEmail(EmailMessage emailMsg)
        {
            string HuLuBlogHost = "hulu@hafuhafu.cn";
            string emailoath = "邮箱授权码";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(HuLuBlogHost);
            mail.To.Add(new MailAddress(emailMsg.To));

            mail.Subject = emailMsg.Subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = emailMsg.Body;
            mail.BodyEncoding = Encoding.UTF8;

            SmtpClient client = new SmtpClient("smtp.exmail.qq.com")
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(HuLuBlogHost, emailoath)
            };
            client.Send(mail);
            


        }
    }
}
