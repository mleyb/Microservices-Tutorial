using Newtonsoft.Json;
using paramore.brighter.commandprocessor;
using Store_Core.Ports.Commands;

namespace Store_Core.Ports.Mappers
{
    public class AddOrderCommandMessageMapper : IAmAMessageMapper<AddOrderCommand>
    {
        public Message MapToMessage(AddOrderCommand request)
        {
            var header = new MessageHeader(messageId: request.Id, topic: "Order.Add", messageType: MessageType.MT_EVENT);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            var message = new Message(header, body);
            return message;
        }

        public AddOrderCommand MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<AddOrderCommand>(message.Body.Value);

        }
    }
}
