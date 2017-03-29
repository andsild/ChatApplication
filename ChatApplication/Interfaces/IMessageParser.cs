using System;

namespace ChatApplication
{
    public interface IMessageParser
    {
        void OnMessageParserError(Exception e);
        void OnMessageParseSuccess(Message message);
    }
}