using Newtonsoft.Json;
using paramore.brighter.commandprocessor;
using Store_Core.Ports.Commands;

namespace Store_Core.Ports.Mappers
{
    class EditOrderCommandMessageMapper : IAmAMessageMapper<EditOrderCommand>
    {
        public Message MapToMessage(EditOrderCommand request)
        {
            var header = new MessageHeader(messageId: request.Id, topic: "Order.Edit", messageType: MessageType.MT_EVENT);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            var message = new Message(header, body);
            return message;

        }

        public EditOrderCommand MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<EditOrderCommand>(message.Body.Value);
        }
    }
}
