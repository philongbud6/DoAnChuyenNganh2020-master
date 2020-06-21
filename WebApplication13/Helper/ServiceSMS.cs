using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace WebApplication13.Helper
{
    public class ServiceSMS
    {
        public void SendSMS(string ToNumber, string Token)
        {
            var accountSid = ConfigurationManager.AppSettings["SMSAccountIdentification"];
            var authToken = ConfigurationManager.AppSettings["SMSAccountPassword"];
            var fromNumber = ConfigurationManager.AppSettings["SMSAccountFrom"];

            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                body: "Your activate code is: " + Token,
                from: new Twilio.Types.PhoneNumber(fromNumber),
                to: new Twilio.Types.PhoneNumber(ToNumber)
            );
        }      
    }
}