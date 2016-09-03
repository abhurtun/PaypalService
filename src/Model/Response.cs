using Braintree;

namespace PaypalService.Api.Model
{
    class Response
    {
        public Status Status { get; set; }
        public Transaction Transaction { get; set; }
    }
}
