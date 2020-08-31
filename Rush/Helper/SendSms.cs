using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmsIrRestful;

namespace Helpers
{
    public static class SendSms
    {
        public static void SendOtp(string cellNumber, string code)
        {
            var token = new Token().GetToken("db67a48dbe57332ec63980c", "rushwebSmsApi");


            var ultraFastSend = new UltraFastSend()
            {
                Mobile = Convert.ToInt64(cellNumber),
                TemplateId = 29303,
                ParameterArray = new List<UltraFastParameters>()
                {
                    new UltraFastParameters()
                    {
                        Parameter = "verifyCode" , ParameterValue = code.ToString()
                    }
                }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            if (ultraFastSendRespone.IsSuccessful)
            {

            }
            else
            {

            }
        }

        public static void SendCommonSms(string cellNumber, string message)
        {
            var token = new Token().GetToken("db67a48dbe57332ec63980c", "rushwebSmsApi");

            SmsIrRestful.MessageSend messageSend = new SmsIrRestful.MessageSend();

            var res = messageSend.Send(token, new SmsIrRestful.MessageSendObject()
            {
                MobileNumbers = new List<string>() { cellNumber }.ToArray(),
                Messages = new List<string>() { message }.ToArray(),
                LineNumber = "30002101001187",
                SendDateTime = null,
                CanContinueInCaseOfError = false
            });

            SmsIrRestful.Credit credit = new SmsIrRestful.Credit();
            SmsIrRestful.CreditResponse creditResponse = new SmsIrRestful.CreditResponse();

        }

    }
}