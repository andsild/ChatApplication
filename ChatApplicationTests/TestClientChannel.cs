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
            var mocker = new Mock<IChatApplication>();
            var username = "batman";
            mocker.Setup(x => x.AddChatErrorLine(username, It.IsAny<String>())).Verifiable();

            var clientChannel = new MessageParserClient(username, mocker.Object);
            clientChannel.ParseReceivedMessage(username);

            mocker.VerifyAll();
        }

        // ... And so on and so forth
    }
}
