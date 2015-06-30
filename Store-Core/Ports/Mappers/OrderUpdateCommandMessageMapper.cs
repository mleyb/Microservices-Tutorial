using Newtonsoft.Json;
using paramore.brighter.commandprocessor;
using Store_Core.Ports.Commands;

namespace Store_Core.Ports.Mappers
{
    class OrderUpdateCommandMessageMapper : IAmAMessageMapper<OrderUpdateCommand>
    {
        public Message MapToMessage(OrderUpdateCommand request)
        {
            var header = new MessageHeader(messageId: request.Id, topic: "Order.Update", messageType: MessageType.MT_EVENT);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            var message = new Message(header, body);
            return message;

        }

        public OrderUpdateCommand MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<OrderUpdateCommand>(message.Body.Value);
        }
    }
}
