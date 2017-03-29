using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMoq;
using ChatApplication;
using Moq;

namespace ChatApplicationTests
{
    [TestClass]
    public class TestClientChannel
    {
        [TestMethod]
        public void ParseMessage_EmptyText_AppendsChatError()
        {
            var mocker = new Mock<IChatUi>();
            var username = "batman";
            mocker.Setup(x => x.AddChatErrorLine(It.IsAny<String>())).Verifiable();

            var clientChannel = new MessageParserClient(username, mocker.Object);
            clientChannel.OnMessageParserError(new ArgumentException("invalid message"));

            mocker.VerifyAll();
        }

        // ... And so on and so forth
    }
}
