using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using paramore.brighter.commandprocessor;
using Store_Core.Ports.Commands;
using Store_Core.Ports.Resources;

namespace Store_API.Adapters.Controllers
{
    public class StoreFrontController : ApiController
    {
        private readonly IAmACommandProcessor _commandProcessor;

        public StoreFrontController(IAmACommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }


        [HttpPost]
        public HttpResponseMessage CreateOrder(AddOrderModel addOrderModel)
        {
            _commandProcessor.Send(new AddOrderCommand(addOrderModel.CustomerName, addOrderModel.Description, DateTime.Parse(addOrderModel.DueDate)));
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
