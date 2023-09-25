using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.DTO.Certificates;
using PayPal.Api;
using static System.Net.WebRequestMethods;

namespace Assignment4Final.Services
{
    public class PaypalService
    {
        private Payment _createdPayment;

        public async Task<Payment> CreatePayment(CertificateDto certificateDto)
        {
            var id = certificateDto.Id;
            if (certificateDto.Price == null)
            {
                certificateDto.Price = 10;
            }
            Dictionary<string, string> myDict = new Dictionary<string, string>()
            {
                { "mode", "sandbox" },
                {
                    "clientId",
                    "ARJnCIsfgnAtVixz7vm7H6prddiZwfZDoIDkoViJLTzpGChXQHidxJ_2stv2ndHBZaJVDdwSYET0THrn"
                },
                {
                    "clientSecret",
                    "EG8Zj7Olz1FrxKD5_eqq1JShD2fXITv8SFs9oY-QxBDLRNkwYavaUyhcHI5oEep5k5hobvOQC9xtov5i"
                }
            };
            var _config = ConfigManager.Instance.GetProperties();
            var _accesToken = new OAuthTokenCredential(myDict).GetAccessToken();
            var apiContext = new APIContext(_accesToken) { Config = myDict };
            try
            {
                Payment payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            amount = new Amount
                            {
                                currency = "EUR",
                                total = $"{certificateDto.Price}"
                            },
                            description = $"Certificate : {certificateDto.Description}",
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        cancel_url = "https://localhost:44473/ExamsList",
                        //return_url = "https://localhost:44473/ExamsList"
                        return_url =
                            $"https://localhost:7196/api/paypal/success/{certificateDto.Id}"
                    }
                };
                _createdPayment = payment.Create(apiContext);
            }
            catch (Exception e)
            {
                Console.WriteLine("sdsdsd");
            }
            return _createdPayment;
        }
    }
}
