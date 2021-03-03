using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Message;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.Consumer;
using EasyNetQ.SystemMessages;
using EasyNetQ.Topology;
using MailKit.Net.Smtp;
using MimeKit;

namespace Com.Stone.HuLuBlog.EmailService
{
    class EmailService : IConsumeAsync<EmailMessage>
    {
        static LogOperation logger = new LogOperation(typeof(EmailService));
        private static IBus bus;
        private const string ErrorQueue = "EasyNetQ_Default_Error_Queue";

        static void Main(string[] args)
        {
            try 
            {
                bus = RabbitHutch.CreateBus("host=localhost", x => x.Register<IConsumerErrorStrategy>(_ => new AlwaysRequeueErrorStrategy()));
                HandleErrors();


                var subscriber = new AutoSubscriber(bus, "EmailService.Email");
                subscriber.ConfigureSubscriptionConfiguration = c => c.WithPrefetchCount(50);
                subscriber.Subscribe(new[] { Assembly.GetExecutingAssembly() });


                Console.WriteLine("正在监听消息，按下回车退出程序...");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                logger.Error("邮件服务异常",ex.Message);
                Console.WriteLine(ex.Message);
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
            string emailoath = "QMa5iTjjcDBEnow6";

            MimeMessage mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(HuLuBlogHost));
            mail.To.Add(MailboxAddress.Parse(emailMsg.To));

            mail.Subject = emailMsg.Subject;
            mail.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = emailMsg.Body };


            var client = new SmtpClient();
            client.Connect("smtp.exmail.qq.com", 465,true);
            client.Authenticate(HuLuBlogHost, emailoath);
            client.Send(mail);
            client.Disconnect(true);
        }

        private static void HandleErrors()
        {
            Action<IMessage<Error>, MessageReceivedInfo> handleErrorMessage = HandleErrorMessage;

            IQueue queue = new Queue(ErrorQueue, false);
            bus.Advanced.Consume(queue, handleErrorMessage);
        }

        private static void HandleErrorMessage(IMessage<Error> msg, MessageReceivedInfo info)
        {
            Console.WriteLine("catch: " + msg.Body.Message);
        }
    }

    public sealed class AlwaysRequeueErrorStrategy : IConsumerErrorStrategy
    {
        public void Dispose()
        {
        }

        public AckStrategy HandleConsumerError(ConsumerExecutionContext context, Exception exception)
        {
            return AckStrategies.NackWithRequeue;
        }

        public AckStrategy HandleConsumerCancelled(ConsumerExecutionContext context)
        {
            return AckStrategies.NackWithRequeue;
        }
    }
}
