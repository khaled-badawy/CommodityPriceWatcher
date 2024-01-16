using Commodity_Prices_Watcher.DAL;
using Microsoft.Extensions.Options;
using Vodafone;

namespace Commodity_Prices_Watcher.BL
{
    public class SmsManager : ISmsManager
    {
        private readonly VodafoneConfiguration _vodaConfig;
        public SmsManager(IOptions<VodafoneConfiguration> vodafConfig)
        {
            _vodaConfig = vodafConfig.Value;
        }
        public bool SendSMS(string phoneNumber, string textMsg)
        {
            Web2SMS.Configure(_vodaConfig.SmsUrl, _vodaConfig.SmsAccountId, _vodaConfig.SmsPassword, _vodaConfig.SmsClientSecret, _vodaConfig.SmsDefaultSenderName);
            try
            {
                Recepient recepient = new()
                {
                    ReceiverMSISDN = phoneNumber,
                    SMSText = textMsg,
                    SenderName = "IDSC",
                };
                var response = Web2SMS.Send(new List<Recepient>() { recepient });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
