using Microsoft.AspNetCore.Blazor;
using System;

namespace Blazor.Realm.ReduxDevTools
{
    public class MessageEventArgs: EventArgs
    {
        public Message Message { get; set; }

        public MessageEventArgs(string message)
        {
            Message = JsonUtil.Deserialize<Message>(message);
        }

    }

    public class Message
    {
        public string Type { get; set; }
        public Payload Payload { get; set; }
        public string State { get; set; }
        public string Id { get; set; }
        public string Source { get; set; }

        public Message() { }
    }

    public class Payload
    {
        public string Type { get; set; }
        public int ActionId { get; set; }
        public int Index { get; set; }

        public Payload() { }
    }
}
