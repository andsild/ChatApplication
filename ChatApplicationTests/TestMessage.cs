using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatApplication;

namespace ChatApplicationTests
{
    [TestClass]
    public class TestMessage
    {
        private readonly string exampleSender = "batman";
        private readonly string exampleRecipient = "superman";
        private readonly string exampleMessage = "hello, world!";

        [TestMethod]
        public void MessageToString_ValidMessage_Idempotency()
        {
            var messageFromFields = new Message(exampleSender, exampleRecipient, exampleMessage);
            var parsedMessageFromFields = Message.ParseTextToMessage(messageFromFields.ToString());
            var messageFromText = Message.ParseTextToMessage($"{exampleSender} {exampleRecipient} {exampleMessage}");

            Assert.AreEqual(messageFromFields.ToString(), parsedMessageFromFields.ToString());
            Assert.AreEqual(messageFromFields.ToString(), messageFromText.ToString());
            Assert.AreEqual(parsedMessageFromFields.ToString(), messageFromText.ToString());
        }

        [TestMethod]
        public void ParseMessage_ValidMessageFields_AssignsFieldsCorrectly()
        {
            var message = Message.ParseTextToMessage($"{exampleSender} {exampleRecipient} {exampleMessage}");
            
            Assert.AreEqual(exampleSender, message.Sender);
            Assert.AreEqual(exampleRecipient, message.Recipient);
            Assert.AreEqual(exampleMessage, message.MessageText);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateUsername_EmptyMessage_ThrowsException()
        {
            Message.ValidateUsername(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateUsername_ContainsBackslash_ThrowsException()
        {
            var invalidUsername = @"a\b";

            Message.ValidateUsername(invalidUsername);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateUsername_LongUsername_ThrowsException()
        {
            var reallyLongUsername = new string('a', 261);

            Message.ValidateUsername(reallyLongUsername);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateMessageText_EmptyMessage_ThrowsException()
        {
            Message.ValidateMessageText(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyFields_ThrowsException()
        {
            new Message(string.Empty, string.Empty, string.Empty);
        }

        [TestMethod]
        public void Constructor_SafeValues_AssignsFieldsCorrectly()
        {
            var message = new Message(exampleSender, exampleRecipient, exampleMessage);

            Assert.AreEqual(exampleSender, message.Sender);
            Assert.AreEqual(exampleRecipient, message.Recipient);
            Assert.AreEqual(exampleMessage, message.MessageText);
        }
    }
}
