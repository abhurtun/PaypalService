using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Braintree;
using Newtonsoft.Json;
using PaypalService.Api.Config;
using PaypalService.Api.Model;

namespace PaypalService.Api.Controllers
{
    public class CheckoutsController : ApiController
    {
        private readonly IBraintreeConfiguration _config = new BraintreeConfiguration();

        private static readonly TransactionStatus[] _transactionSuccessStatuses = {
                                                                                    TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                                    TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                                                };
        [ActionName("New")]
        public IHttpActionResult GetNew()
        {
            var gateway = _config.GetGateway();
            var clientToken = gateway.ClientToken.generate();
            return Content(HttpStatusCode.OK, clientToken);
        }

        [ActionName("Create")]
        public IHttpActionResult Create(string amount, string nonce)
        {
            var gateway = _config.GetGateway();
            decimal _amount;

            try
            {
                _amount = Convert.ToDecimal(amount);
            }
            catch (FormatException)
            {
                return Content(HttpStatusCode.BadRequest, "Error: 81503: Amount is an invalid format.");
            }

            var _nonce = nonce;
            var request = new TransactionRequest
            {
                Amount = _amount,
                PaymentMethodNonce = _nonce
            };

            var result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                var transaction = result.Target;
                return Content(HttpStatusCode.OK, transaction.Id );
            }
            if (result.Transaction != null)
            {
                return Content(HttpStatusCode.OK, result.Transaction.Id );
            }
            var errorMessages = "";
            foreach (var error in result.Errors.DeepAll())
            {
                errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
            }
            return Content(HttpStatusCode.BadRequest, errorMessages);
        }

        [ActionName("Show")]
        public IHttpActionResult GetShow(string id)
        {
            var status = new Status();
            var response = new Response();
            var gateway = _config.GetGateway();
            var transaction = gateway.Transaction.Find(id);

            if (_transactionSuccessStatuses.Contains(transaction.Status))
            {
                status.header = "Sweet Success!";
                status.icon = "success";
                status.message ="Your test transaction has been successfully processed. See the Braintree API response and try again.";


            }
            else
            {
                status.header = "Transaction Failed";
                status.icon = "fail";
                status.message = $"Your test transaction has a status of {transaction.Status}. See the Braintree API response and try again.";
            };

            response.Status = status;
            response.Transaction = transaction;
             
            return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }
    }
}
