using Hx.Common.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp.Models
{
    public class EmailManager
    {
        private static readonly EmailManager email = new EmailManager();
        public Queue<EmailHelper> emailQUeue = new Queue<EmailHelper>();
        private EmailManager()
        { }
        public static EmailManager Instance
        {
            get {
                return email;
            }
        }
        public void AddEmailQueue(string userName,string checkUrl, string toEmail)
        {
            EmailHelper email = new EmailHelper()
            {
                MailPwd = "tao58568470jie",
                MailFrom = "stjworkemail@163.com",
                MailSubject = "欢迎您注册 海星·博客",
                MailBody = EmailHelper.TempBody(userName, "请复制打开链接(或者右键新标签中打开)，激活账号",
                    "<a style='word-wrap: break-word;word-break: break-all;' href='" + checkUrl + "'>" + checkUrl + "</a>"),
                MailToArray = new string[] { toEmail }
            };
            this.emailQUeue.Enqueue(email);
        }
        private void SenEmail()
        {
            while (true)
            {
                if (this.emailQUeue.Count > 0)
                {
                    EmailHelper email = this.emailQUeue.Dequeue();
                    email.Send();
                }
            }
        }
    }
}