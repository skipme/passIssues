using FluentEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace backend
{
    public class MailInformer
    {
        public static void SendAfterRegistration(Objects.User user)
        {
            Objects.Employee em = Objects.Employee.Get(user).FirstOrDefault();

            var mail = Email.FromDefault()
                .To(user.Email)
                .Subject("Registration info")
                .CC("cerriun.main@gmail.com")
                .Body(string.Format("Вы успешено зарегистрированы под именем: {0} {1}", em.Name, em.FamilyName));

            try
            {
                mail.Send();
            }
            catch (Exception e)
            {

            }
        }
    }
}
